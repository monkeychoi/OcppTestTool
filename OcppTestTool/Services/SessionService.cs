using OcppTestTool.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OcppTestTool.Services
{
    public sealed class SessionService : ISessionService
    {
        private AuthUser? _currentUser;
        public AuthUser? CurrentUser
        {
            get => _currentUser;
            private set { _currentUser = value; OnPropertyChanged(); }
        }

        public void SignIn(AuthUser user) => CurrentUser = user;
        public void SignOut() => CurrentUser = null;

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
