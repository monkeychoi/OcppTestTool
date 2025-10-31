using OcppTestTool.Models.Entities.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OcppTestTool.Services.Http
{
    public sealed class ApiClient : IApiClient
    {
        private readonly HttpClient _http;
        private static readonly JsonSerializerOptions _json = new(JsonSerializerDefaults.Web)
        {
            PropertyNameCaseInsensitive = true
        };

        public ApiClient(HttpClient http) => _http = http;

        public async Task<ApiResult<T>> GetAsync<T>(string uri, CancellationToken ct = default)
        {
            try
            {
                using var resp = await _http.GetAsync(uri, ct);
                return await ToResult<T>(resp, ct);
            }
            catch (TaskCanceledException) { return ApiResult<T>.Fail("요청이 취소되었습니다."); }
            catch (HttpRequestException ex) { return ApiResult<T>.Fail($"네트워크 오류: {ex.Message}"); }
        }

        public async Task<ApiResult<TResp>> PostAsync<TReq, TResp>(string uri, TReq body, CancellationToken ct = default)
        {
            try
            {
                using var resp = await _http.PostAsJsonAsync(uri, body, _json, ct);
                return await ToResult<TResp>(resp, ct);
            }
            catch (TaskCanceledException) { return ApiResult<TResp>.Fail("요청이 취소되었습니다."); }
            catch (HttpRequestException ex) { return ApiResult<TResp>.Fail($"네트워크 오류: {ex.Message}"); }
        }

        public async Task<ApiResult<TResp>> PutAsync<TReq, TResp>(string uri, TReq body, CancellationToken ct = default)
        {
            try
            {
                using var resp = await _http.PutAsJsonAsync(uri, body, _json, ct);
                return await ToResult<TResp>(resp, ct);
            }
            catch (TaskCanceledException) { return ApiResult<TResp>.Fail("요청이 취소되었습니다."); }
            catch (HttpRequestException ex) { return ApiResult<TResp>.Fail($"네트워크 오류: {ex.Message}"); }
        }

        public async Task<ApiResult<bool>> DeleteAsync(string uri, CancellationToken ct = default)
        {
            try
            {
                using var resp = await _http.DeleteAsync(uri, ct);
                var code = (int)resp.StatusCode;
                var reason = resp.ReasonPhrase;

                if (resp.IsSuccessStatusCode) 
                    return ApiResult<bool>.Ok(true, code, reason);

                var err = await ReadErrorAsync(resp, ct);
                return ApiResult<bool>.Fail(err, code, reason);
            }
            catch (TaskCanceledException) { return ApiResult<bool>.Fail("요청이 취소되었습니다."); }
            catch (HttpRequestException ex) { return ApiResult<bool>.Fail($"네트워크 오류: {ex.Message}"); }
        }

        private static async Task<ApiResult<T>> ToResult<T>(HttpResponseMessage resp, CancellationToken ct)
        {
            if (resp.IsSuccessStatusCode)
            {
                // NoContent 대응
                if (resp.StatusCode == HttpStatusCode.NoContent)
                    return ApiResult<T>.Ok(default!);

                var data = await resp.Content.ReadFromJsonAsync<T>(_json, ct);
                return data is not null
                    ? ApiResult<T>.Ok(data)
                    : ApiResult<T>.Fail("서버 응답 파싱 실패");
            }
            var error = await ReadErrorAsync(resp, ct);
            return ApiResult<T>.Fail(error);
        }

        private static async Task<string> ReadErrorAsync(HttpResponseMessage resp, CancellationToken ct)
        {
            // RFC 7807 ProblemDetails 대응 시도
            try
            {
                var content = await resp.Content.ReadAsStringAsync(ct);
                var status = (int)resp.StatusCode;

                if (string.IsNullOrWhiteSpace(content))
                    return $"{status} {resp.ReasonPhrase}";

                string? msg = null;

                try
                {
                    using var doc = JsonDocument.Parse(content);
                    var root = doc.RootElement;

                    // 1) 문제 상세 우선
                    if (root.TryGetProperty("detail", out var d) && d.ValueKind == JsonValueKind.String)
                        msg = d.GetString();

                    // 2) 타이틀
                    if (msg is null && root.TryGetProperty("title", out var t) && t.ValueKind == JsonValueKind.String)
                        msg = t.GetString();

                    // 3) 일반적인 에러 키들
                    if (msg is null && root.TryGetProperty("error", out var e) && e.ValueKind == JsonValueKind.String)
                        msg = e.GetString();

                    if (msg is null && root.TryGetProperty("message", out var m) && m.ValueKind == JsonValueKind.String)
                        msg = m.GetString();

                    // 4) ModelState 스타일의 errors 객체가 있을 수 있음
                    if (msg is null && root.TryGetProperty("errors", out var errs) && errs.ValueKind == JsonValueKind.Object)
                    {
                        foreach (var prop in errs.EnumerateObject())
                        {
                            // ["메시지1","메시지2"...] 중 첫 개만 사용
                            if (prop.Value.ValueKind == JsonValueKind.Array && prop.Value.GetArrayLength() > 0)
                            {
                                var first = prop.Value[0];
                                if (first.ValueKind == JsonValueKind.String)
                                {
                                    msg = first.GetString();
                                    break;
                                }
                            }
                        }
                    }
                }
                catch { /* 무시하고 원문 반환 */ }

                // 최종 메시지 구성: 내용 + (HTTP 코드)
                msg ??= content;
                return $"{msg} (HTTP {status})";
            }
            catch
            {
                return $"{(int)resp.StatusCode} {resp.ReasonPhrase}";
            }
        }
    }
}
