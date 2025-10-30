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
    public sealed class ProtocolManagementService : IProtocolManagementService
    {
        private readonly IApiClient _api;

        public ProtocolManagementService(IApiClient api) => _api = api;

        /// <summary>
        /// 프로토콜 목록
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<IReadOnlyList<OcppProtocol>> GetProtocolsAsync(CancellationToken ct = default)
        {
            var resp = await _api.GetAsync<List<OcppProtocolDto>>("/admin/protocols", ct);
            if (!resp.Success || resp.Data is null) return Array.Empty<OcppProtocol>();
            return resp.Data.ToEntities();
        }

        public async Task<ApiResult<OcppProtocol>> CreateProtocolAsync(OcppProtocolCreateDto dto, CancellationToken ct = default)
        {
            var resp = await _api.PostAsync<OcppProtocolCreateDto, OcppProtocolDto>("/admin/protocols", dto, ct);

            if (!resp.Success || resp.Data is null)
                return ApiResult<OcppProtocol>.Fail(resp.Error ?? "Failed to create protocol.");

            var entity = resp.Data.ToEntity();
            return ApiResult<OcppProtocol>.Ok(entity);
        }

        public async Task<ApiResult<OcppProtocol>> EditProtocolAsync(OcppProtocolEditDto dto, CancellationToken ct = default)
        {
            var resp = await _api.PutAsync<OcppProtocolEditDto, OcppProtocolDto>($"/admin/protocols/{dto.Id}", dto, ct);

            if (!resp.Success || resp.Data is null)
                return ApiResult<OcppProtocol>.Fail(resp.Error ?? "Failed to create protocol.");

            var entity = resp.Data.ToEntity();
            return ApiResult<OcppProtocol>.Ok(entity);
        }
    }
}
