using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcppTestTool.Models.Entities.Auth
{
    public sealed class AuthUser
    {
        public string UserId { get; init; } = "";
        public string UserName { get; init; } = "";
        public string Email { get; init; } = "";
        public string Role { get; init; } = "";
        public string Token { get; set; } = "";
        public DateTimeOffset? IssuedAt { get; init; } // iat
        public DateTimeOffset? ExpiresAt { get; init; } // exp
    }
}
