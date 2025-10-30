using OcppTestTool.Models.Dtos.Protocol;
using OcppTestTool.Models.Entities.Http;
using OcppTestTool.Models.Entities.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcppTestTool.Services.Protocol
{
    public interface IProtocolManagementService
    {
        Task<IReadOnlyList<OcppProtocol>> GetProtocolsAsync(CancellationToken ct = default);
        Task<ApiResult<OcppProtocol>> CreateProtocolAsync(OcppProtocolCreateDto dto, CancellationToken ct = default);
    }
}
