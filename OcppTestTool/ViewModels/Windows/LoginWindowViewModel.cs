using OcppTestTool.Services.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcppTestTool.ViewModels.Windows
{
    public partial class LoginWindowViewModel : ObservableObject
    {
        private readonly IAuthService _authService;
        private readonly ISessionService _session;

        public event EventHandler<bool>? CloseRequested; // true=OK, false=Cancel

        [ObservableProperty] private string _userId = "kappa10";
        [ObservableProperty] private string _userPassword = "Passw0rd!0"; // 필요 시 SecureString으로 교체
        [ObservableProperty] private bool _isBusy;
        [ObservableProperty] private string _errorMessage = "";

        public LoginWindowViewModel(IAuthService authService, ISessionService session)
        {
            _authService = authService;
            _session = session;
        }

        [RelayCommand]
        private void Cancel()
        {
            CloseRequested?.Invoke(this, false);
        }

        [RelayCommand]
        private async Task LoginAsync()
        {
            ErrorMessage = "";
            if (string.IsNullOrWhiteSpace(UserId) || string.IsNullOrWhiteSpace(UserPassword))
            {
                ErrorMessage = "아이디/비밀번호를 입력하세요.111";
                return;
            }

            IsBusy = true;
            try
            {
                var user = await _authService.LoginAsync(UserId.Trim(), UserPassword);
                if (user is null)
                {
                    ErrorMessage = "아이디 또는 비밀번호가 올바르지 않습니다.";
                    return;
                }

                _session.SignIn(user);
                CloseRequested?.Invoke(this, true);
            }
            catch (Exception ex)
            {
                ///ErrorMessage = $"로그인 오류: {ex.Message}";
                MessageBox.Show($"로그인 오류: {ex.Message}\n{ex.StackTrace}", "오류", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
