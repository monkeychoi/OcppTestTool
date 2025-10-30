using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OcppTestTool.Models.Dtos.Auth
{
    public sealed class LoginRequest
    {
        [JsonPropertyName("username")]
        public string UserName { get; init; } = "";

        [JsonPropertyName("password")]
        public string Password { get; init; } = "";
    }

    public sealed class LoginResponse
    {
        [JsonPropertyName("token")]
        public string? Token { get; init; }  // 토큰형 인증 대비
    }
}
