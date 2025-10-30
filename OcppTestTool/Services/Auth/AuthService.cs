using OcppTestTool.Helpers;
using OcppTestTool.Models.Dtos.Auth;
using OcppTestTool.Models.Entities.Auth;
using OcppTestTool.Services.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace OcppTestTool.Services.Auth
{
    public sealed class AuthService : IAuthService
    {
        private readonly IApiClient _api;

        public AuthService(IApiClient api)
        {
            _api = api;
        }

        public async Task<AuthUser?> LoginAsync(string userId, string password, CancellationToken ct = default)
        {
            var req = new LoginRequest { UserName = userId, Password = password };
            var result = await _api.PostAsync<LoginRequest, LoginResponse>("/auth/login", req, ct);

            if (!result.Success || result.Data?.Token is null) return null;

            var token = result.Data.Token;

            // JWT payload 디코드
            if (!JwtHelper.TryReadPayload(token, out var p) || p is null) return null;

            // 만료/유효성(선택) 체크
            if (p.Exp is long expSeconds)
            {
                var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                if (now >= expSeconds) return null; // 만료된 토큰
            }

            // Role 표준화(대문자/소문자 혼재 대비)
            var normRole = NormalizeRole(p.Role);

            return new AuthUser
            {
                UserId = p.UserId ?? "",
                UserName = p.Username ?? "",
                Email = "",
                Role = normRole,
                Token = token,
                IssuedAt = p.Iat is long i ? DateTimeOffset.FromUnixTimeSeconds(i) : null,
                ExpiresAt = p.Exp is long e ? DateTimeOffset.FromUnixTimeSeconds(e) : null
            };
        }

        private static string NormalizeRole(string? role)
        {
            if (string.IsNullOrWhiteSpace(role)) return "User";
            // ADMIN/USER 등 서버 표기를 UI에 맞게 정리
            var upper = role.Trim().ToUpperInvariant();
            return upper switch
            {
                "ADMIN" => "Admin",
                "USER" => "User",
                _ => role // 그대로 사용하거나 기본값 "User"
            };
        }
    }
}
