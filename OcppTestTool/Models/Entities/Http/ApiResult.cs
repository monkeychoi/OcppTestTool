﻿using System;
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

        public static ApiResult<T> Ok(T data) => new() { Success = true, Data = data };
        public static ApiResult<T> Fail(string? error) => new() { Success = false, Error = error };
    }
}
