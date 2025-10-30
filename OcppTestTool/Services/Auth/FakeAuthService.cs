using OcppTestTool.Models.Entities.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcppTestTool.Services.Auth
{
    public sealed class FakeAuthService : IAuthService
    {
        public Task<AuthUser?> LoginAsync(string userId, string password, CancellationToken ct = default)
        {
            // TODO: 필요 시 조건 더 추가 (락/지연/에러 시뮬)
            if (userId == "1" && password == "1")
            {
                var user = new AuthUser
                {
                    UserId = userId,
                    UserName = "김개똥",
                    Email = "test@example.com",
                    Role = "Admin"
                };
                return Task.FromResult<AuthUser?>(user);
            }

            return Task.FromResult<AuthUser?>(null);
        }
    }
}
