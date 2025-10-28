using OcppTestTool.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcppTestTool.Services
{
    public interface ISessionService : INotifyPropertyChanged
    {
        AuthUser? CurrentUser { get; }
        void SignIn(AuthUser user);
        void SignOut();
    }
}
