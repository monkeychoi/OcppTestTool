using Microsoft.Extensions.DependencyInjection;
using OcppTestTool.Helpers;
using OcppTestTool.Models.Entities.Auth;
using OcppTestTool.Models.Entities.Common;
using OcppTestTool.Services.Auth;
using OcppTestTool.Views.Windows;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Threading;
using Wpf.Ui.Controls;

namespace OcppTestTool.ViewModels.Windows
{
    public partial class MainWindowViewModel : ObservableObject
    {
        private readonly ISessionService _session;

        // 토큰 만료 타이머 관련 필드
        private readonly DispatcherTimer _tokenTimer = new() { Interval = TimeSpan.FromSeconds(1) };
        private DateTimeOffset _tokenExpUtc = DateTimeOffset.MinValue;

        [ObservableProperty] private string _tokenRemainingText = "00:00";
        [ObservableProperty] private bool _tokenIsExpiringSoon;

        [ObservableProperty]
        private string _applicationTitle = "Ocpp Test Tool";

        [ObservableProperty]
        private ObservableCollection<object> _menuItems = new();       

        [ObservableProperty]
        private ObservableCollection<object> _footerMenuItems = new()
        {
            new NavigationViewItem()
            {
                Content = "Settings",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Settings24 },
                TargetPageType = typeof(Views.Pages.SettingsPage)
            }
        };

        [ObservableProperty]
        private ObservableCollection<MenuItem> _trayMenuItems = new()
        {
            new MenuItem { Header = "Home", Tag = "tray_home" }
        };

        /// <summary>
        /// 현재 로그인 사용자. XAML에선 ViewModel.CurrentUser.DisplayName 같이 바인딩.
        /// </summary>
        public AuthUser? CurrentUser => _session.CurrentUser;

        public MainWindowViewModel(ISessionService session)
        {
            _session = session;

            RebuildMenu();

            // 토큰 만료 타이머 구독
            _tokenTimer.Tick += (_, _) => TokenTick();

            // 세션에서 액세스 토큰 또는 payload 확보
            // 예시 1) 세션이 액세스 토큰 문자열을 제공하는 경우:
            var jwt = _session.CurrentUser?.Token;

            if (!string.IsNullOrWhiteSpace(jwt) && JwtHelper.TryReadPayload(jwt, out var payload) && payload is not null)
            {
                if (payload.Exp is long exp)
                {
                    var expUtc = JwtHelper.FromUnixSeconds(exp);
                    StartTokenCountdown(expUtc);
                }
                else
                {
                    // exp 없음 → 타이머 시작하지 않음 / "00:00" 유지
                    StopTokenCountdown(); // 또는 표시만 초기화
                }

            }
            else
            {
                // 토큰이 없거나 파싱 실패 시: 표시값 초기화
                TokenRemainingText = "00:00:00";
                TokenIsExpiringSoon = false;
            }


        }

        #region 세션 카운트 다운

        private void StartTokenCountdown(DateTimeOffset expUtc)
        {
            _tokenExpUtc = expUtc.ToUniversalTime();
            TokenTick();           // 즉시 1회 갱신
            _tokenTimer.Start();
        }

        private void ResetTokenCountdown(DateTimeOffset newExpUtc)
        {
            _tokenExpUtc = newExpUtc.ToUniversalTime();
            if (_tokenTimer.IsEnabled) TokenTick(); // 표시 즉시 갱신
        }

        private void StopTokenCountdown()
        {
            _tokenTimer.Stop();
            TokenRemainingText = "00:00";
            TokenIsExpiringSoon = false;
        }

        private void TokenTick()
        {
            if (_tokenExpUtc == DateTimeOffset.MinValue)
            {
                StopTokenCountdown();
                return;
            }

            var remain = _tokenExpUtc - DateTimeOffset.UtcNow;

            if (remain <= TimeSpan.Zero)
            {
                StopTokenCountdown();
                // 만료 즉시 로그아웃 진행
                Logout();
                return;
            }

            // 1시간 이상이면 HH:mm:ss, 아니면 00:mm:ss
            TokenRemainingText = remain.TotalHours >= 1
                ? $"{(int)remain.TotalHours:00}:{remain.Minutes:00}:{remain.Seconds:00}"
                : $"00:{remain.Minutes:00}:{remain.Seconds:00}";

            TokenIsExpiringSoon = remain <= TimeSpan.FromMinutes(5);
        }

        #endregion

        #region 메뉴 처리

        private void RebuildMenu()
        {
            var role = _session.CurrentUser?.Role;
            bool hasAccess(MenuDef d) => d.RequiredRole is null
                || string.Equals(d.RequiredRole, role, StringComparison.OrdinalIgnoreCase);

            var items = MenuCatalog.All
                .Where(hasAccess)
                .Select(d => new NavigationViewItem
                {
                    Content = d.Title,
                    Icon = new SymbolIcon { Symbol = d.Icon },
                    TargetPageType = d.PageType
                })
                .ToList();

            foreach (var item in items)
            {
                MenuItems.Add(item);
            }
        }

        #endregion

        #region 로그인 사용자 전용 메뉴

        // === 사용자 전용 메뉴 커맨드 ===

        [RelayCommand]
        private void EditProfile()
        {
            // TODO: Dialog 또는 전용 페이지로 이동
            System.Windows.MessageBox.Show("정보 변경 화면을 띄웁니다.", "정보 변경");
        }

        [RelayCommand]
        private void ChangePassword()
        {
            // TODO: Dialog 또는 전용 페이지로 이동
            System.Windows.MessageBox.Show("비밀번호 변경 화면을 띄웁니다.", "비밀번호 변경");
        }

        [RelayCommand]
        private void Logout()
        {
            StopTokenCountdown();

            _session.SignOut();
            Application.Current.MainWindow?.Close();
        }

        #endregion


    }
}
