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
                using var resp = await _http
                    .GetAsync(uri, HttpCompletionOption.ResponseHeadersRead, ct)
                    .ConfigureAwait(false);

                return await ToResult<T>(resp, ct).ConfigureAwait(false);
            }
            catch (TaskCanceledException) { return ApiResult<T>.Fail("요청이 취소되었습니다."); }
            catch (HttpRequestException ex) { return ApiResult<T>.Fail($"네트워크 오류: {ex.Message}"); }
        }

        public async Task<ApiResult<TResp>> PostAsync<TReq, TResp>(string uri, TReq body, CancellationToken ct = default)
        {
            try
            {
                using var resp = await _http
                    .PostAsJsonAsync(uri, body, _json, ct)
                    .ConfigureAwait(false);

                return await ToResult<TResp>(resp, ct).ConfigureAwait(false);
            }
            catch (TaskCanceledException) { return ApiResult<TResp>.Fail("요청이 취소되었습니다."); }
            catch (HttpRequestException ex) { return ApiResult<TResp>.Fail($"네트워크 오류: {ex.Message}"); }
        }

        public async Task<ApiResult<TResp>> PutAsync<TReq, TResp>(string uri, TReq body, CancellationToken ct = default)
        {
            try
            {
                using var resp = await _http
                    .PutAsJsonAsync(uri, body, _json, ct)
                    .ConfigureAwait(false);

                return await ToResult<TResp>(resp, ct).ConfigureAwait(false);
            }
            catch (TaskCanceledException) { return ApiResult<TResp>.Fail("요청이 취소되었습니다."); }
            catch (HttpRequestException ex) { return ApiResult<TResp>.Fail($"네트워크 오류: {ex.Message}"); }
        }

        public async Task<ApiResult<bool>> DeleteAsync(string uri, CancellationToken ct = default)
        {
            try
            {
                using var resp = await _http
                    .DeleteAsync(uri, ct)
                    .ConfigureAwait(false);

                var code = (int)resp.StatusCode;
                var reason = resp.ReasonPhrase;

                if (resp.IsSuccessStatusCode)
                    return ApiResult<bool>.Ok(true, code, reason);

                var err = await ReadErrorAsync(resp, ct).ConfigureAwait(false);
                return ApiResult<bool>.Fail(err, code, reason);
            }
            catch (TaskCanceledException) { return ApiResult<bool>.Fail("요청이 취소되었습니다."); }
            catch (HttpRequestException ex) { return ApiResult<bool>.Fail($"네트워크 오류: {ex.Message}"); }
        }

        private static async Task<ApiResult<T>> ToResult<T>(HttpResponseMessage resp, CancellationToken ct)
        {
            var code = (int)resp.StatusCode;
            var reason = resp.ReasonPhrase;

            if (resp.IsSuccessStatusCode)
            {
                if (resp.StatusCode == HttpStatusCode.NoContent)
                    return ApiResult<T>.Ok(default!, code, reason);

                await using var stream = await resp.Content.ReadAsStreamAsync(ct).ConfigureAwait(false);
                try
                {
                    var data = await JsonSerializer
                        .DeserializeAsync<T>(stream, _json, ct)
                        .ConfigureAwait(false);

                    return data is not null
                        ? ApiResult<T>.Ok(data, code, reason)
                        : ApiResult<T>.Fail("서버 응답 파싱 실패", code, reason);
                }
                catch (JsonException jx)
                {
                    // 파싱 실패 시 원문을 읽어 메시지로 돌려줌(디버깅/로그 용이)
                    var raw = await ReadContentAsStringSafe(resp, ct).ConfigureAwait(false);
                    return ApiResult<T>.Fail($"JSON 파싱 실패: {jx.Message}\n{raw}", code, reason);
                }
            }

            var error = await ReadErrorAsync(resp, ct).ConfigureAwait(false);
            return ApiResult<T>.Fail(error, code, reason);
        }

        private static async Task<string> ReadErrorAsync(HttpResponseMessage resp, CancellationToken ct)
        {
            var status = (int)resp.StatusCode;
            try
            {
                var content = await ReadContentAsStringSafe(resp, ct).ConfigureAwait(false);
                if (string.IsNullOrWhiteSpace(content))
                    return $"{status} {resp.ReasonPhrase}";

                try
                {
                    using var doc = JsonDocument.Parse(content);
                    var root = doc.RootElement;

                    // RFC 7807 우선
                    if (root.TryGetProperty("detail", out var d) && d.ValueKind == JsonValueKind.String)
                        return d.GetString() ?? $"HTTP {status}";

                    if (root.TryGetProperty("title", out var t) && t.ValueKind == JsonValueKind.String)
                        return t.GetString() ?? $"HTTP {status}";

                    // 일반적 키
                    if (root.TryGetProperty("error", out var e) && e.ValueKind == JsonValueKind.String)
                        return e.GetString() ?? $"HTTP {status}";

                    if (root.TryGetProperty("message", out var m) && m.ValueKind == JsonValueKind.String)
                        return m.GetString() ?? $"HTTP {status}";

                    // ModelState 스타일
                    if (root.TryGetProperty("errors", out var errs) && errs.ValueKind == JsonValueKind.Object)
                    {
                        foreach (var prop in errs.EnumerateObject())
                        {
                            if (prop.Value.ValueKind == JsonValueKind.Array && prop.Value.GetArrayLength() > 0)
                            {
                                var first = prop.Value[0];
                                if (first.ValueKind == JsonValueKind.String)
                                    return first.GetString() ?? $"HTTP {status}";
                            }
                        }
                    }

                    // 그 외에는 원문
                    return $"{content}";
                }
                catch
                {
                    return $"{content}";
                }
            }
            catch
            {
                return $"{status} {resp.ReasonPhrase}";
            }
        }

        // 문자열 읽기 헬퍼(예외 억제)
        private static async Task<string> ReadContentAsStringSafe(HttpResponseMessage resp, CancellationToken ct)
        {
            try
            {
                return await resp.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
