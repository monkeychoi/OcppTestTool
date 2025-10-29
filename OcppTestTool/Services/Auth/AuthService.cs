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

            if (!result.Success || result.Data is null) return null;

            var data = result.Data;

            return new AuthUser
            {
                UserId = data.UserId,
                DisplayName = data.DisplayName,
                Email = data.Email,
                Role = data.Role,
                Token = data.Token ?? ""
            };
        }
    }
}
