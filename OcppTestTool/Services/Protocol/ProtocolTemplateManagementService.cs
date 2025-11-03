using OcppTestTool.Models.Dtos.Protocol;
using OcppTestTool.Models.Entities.Http;
using OcppTestTool.Models.Entities.Protocol;
using OcppTestTool.Services.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcppTestTool.Services.Protocol
{
    public sealed class ProtocolTemplateManagementService : IProtocolTemplateManagementService
    {
        private readonly IApiClient _api;

        public ProtocolTemplateManagementService(IApiClient api) => _api = api;

        /// <summary>
        /// 프로토콜 목록
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<IReadOnlyList<OcppProtocolTemplate>> GetProtocolTemplatesAsync(int protocolId, CancellationToken ct = default)
        {
            //TODO: protocolId 없을 떄 처리 해야함
            var resp = await _api.GetAsync<List<OcppProtocolTemplateDto>>($"/admin/protocols/{protocolId}/templates", ct);
            if (!resp.Success || resp.Data is null) return Array.Empty<OcppProtocolTemplate>();            
            var entities = await Task.Run(() => resp.Data.ToEntities(), ct);
            return entities;
        }

        //public async Task<ApiResult<OcppProtocol>> CreateProtocolAsync(OcppProtocolCreateDto dto, CancellationToken ct = default)
        //{
        //    var resp = await _api.PostAsync<OcppProtocolCreateDto, OcppProtocolDto>("/admin/protocols", dto, ct);

            //    if (!resp.Success || resp.Data is null)
            //        return ApiResult<OcppProtocol>.Fail(resp.Error ?? "Failed to create protocol.");

            //    var entity = resp.Data.ToEntity();
            //    return ApiResult<OcppProtocol>.Ok(entity);
            //}

            //public async Task<ApiResult<OcppProtocol>> EditProtocolAsync(OcppProtocolEditDto dto, CancellationToken ct = default)
            //{
            //    var resp = await _api.PutAsync<OcppProtocolEditDto, OcppProtocolDto>($"/admin/protocols/{dto.Id}", dto, ct);

            //    if (!resp.Success || resp.Data is null)
            //        return ApiResult<OcppProtocol>.Fail(resp.Error ?? "Failed to edit protocol.");

            //    var entity = resp.Data.ToEntity();
            //    return ApiResult<OcppProtocol>.Ok(entity);
            //}

            //public async Task<ApiResult<bool>> DeleteProtocolAsync(int id, CancellationToken ct = default)
            //{
            //    if (id <= 0)
            //        return ApiResult<bool>.Fail("잘못된 프로토콜 ID입니다.", 400);


            //    var res = await _api.DeleteAsync($"/admin/protocols/{id}", ct);
            //    if (res.Success) return res;

            //    if (res.StatusCode == 404)
            //        return ApiResult<bool>.Fail("항목을 찾을 수 없습니다.\n이미 삭제되었거나 존재하지 않을 수 있습니다.", res.StatusCode, res.ReasonPhrase);

            //    if (res.StatusCode is 409 or 412)
            //        return ApiResult<bool>.Fail("다른 작업과 충돌했습니다.\n화면을 새로고침 후 다시 시도해주세요.", res.StatusCode, res.ReasonPhrase);

            //    return res;
            //}
    }
}
