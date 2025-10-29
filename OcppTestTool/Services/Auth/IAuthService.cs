using OcppTestTool.Models.Entities.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcppTestTool.Services.Auth
{
    public interface IAuthService
    {
        Task<AuthUser?> LoginAsync(string userId, string password, CancellationToken ct = default);
    }
}
