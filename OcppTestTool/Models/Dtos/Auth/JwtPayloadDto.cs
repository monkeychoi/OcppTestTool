using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OcppTestTool.Models.Dtos.Auth
{
    public sealed class JwtPayloadDto
    {
        [JsonPropertyName("userId")] public string? UserId { get; init; }
        [JsonPropertyName("username")] public string? Username { get; init; }
        [JsonPropertyName("role")] public string? Role { get; init; }
        [JsonPropertyName("iat")] public long? Iat { get; init; } // seconds
        [JsonPropertyName("exp")] public long? Exp { get; init; } // seconds
    }
}
