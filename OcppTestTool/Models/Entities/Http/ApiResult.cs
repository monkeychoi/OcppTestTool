using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcppTestTool.Models.Entities.Http
{
    public sealed class ApiResult<T>
    {
        public bool Success { get; init; }        
        public T? Data { get; init; }
        public string? Error { get; init; }

        public int? StatusCode { get; init; }        // 200, 204, 404, 409, 500...
        public string? ReasonPhrase { get; init; }   // "No Content", "Not Found"...

        public static ApiResult<T> Ok(T data, int? statusCode = null, string? reason = null)
            => new() { Success = true, Data = data, StatusCode = statusCode, ReasonPhrase = reason };
        public static ApiResult<T> Fail(string? error, int? statusCode = null, string? reason = null)
            => new() { Success = false, Error = error, StatusCode = statusCode, ReasonPhrase = reason };
    }
}
