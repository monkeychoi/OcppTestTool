using OcppTestTool.Models.Entities.Auth;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcppTestTool.Services.Auth
{
    public interface ISessionService
    {
        AuthUser? CurrentUser { get; }
        void SignIn(AuthUser user);
        void SignOut();
    }
}
