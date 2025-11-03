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
    public interface IProtocolTemplateManagementService
    {
        Task<IReadOnlyList<OcppProtocolTemplate>> GetProtocolTemplatesAsync(int protocolId, CancellationToken ct = default);
        //Task<ApiResult<OcppProtocol>> CreateProtocolAsync(OcppProtocolCreateDto dto, CancellationToken ct = default);
        //Task<ApiResult<OcppProtocol>> EditProtocolAsync(OcppProtocolEditDto dto, CancellationToken ct = default);
        //Task<ApiResult<bool>> DeleteProtocolAsync(int id, CancellationToken ct = default);
    }
}
