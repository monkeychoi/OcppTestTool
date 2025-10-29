using OcppTestTool.Models.Entities.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcppTestTool.Services.Http
{
    public interface IApiClient
    {
        Task<ApiResult<T>> GetAsync<T>(string uri, CancellationToken ct = default);
        Task<ApiResult<TResp>> PostAsync<TReq, TResp>(string uri, TReq body, CancellationToken ct = default);
        Task<ApiResult<TResp>> PutAsync<TReq, TResp>(string uri, TReq body, CancellationToken ct = default);
        Task<ApiResult<bool>> DeleteAsync(string uri, CancellationToken ct = default);
    }
}
