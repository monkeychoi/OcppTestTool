using Microsoft.Extensions.DependencyInjection;
using OcppTestTool.Models;
using OcppTestTool.Services;
using OcppTestTool.Views.Windows;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Wpf.Ui.Controls;

namespace OcppTestTool.ViewModels.Windows
{
    public partial class MainWindowViewModel : ObservableObject
    {
        private readonly ISessionService _session;
        private readonly ISessionStorage _storage;

        [ObservableProperty]
        private string _applicationTitle = "Ocpp Test Tool";

        [ObservableProperty]
        private ObservableCollection<object> _menuItems = new()
        {
            new NavigationViewItem()
            {
                Content = "Home",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Home24 },
                TargetPageType = typeof(Views.Pages.DashboardPage)
            },
            new NavigationViewItem()
            {
                Content = "Data",
                Icon = new SymbolIcon { Symbol = SymbolRegular.DataHistogram24 },
                TargetPageType = typeof(Views.Pages.DataPage)
            }
        };

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

        public MainWindowViewModel(
            ISessionService session,
            ISessionStorage storage)
        {
            _session = session;
            _storage = storage;

            // 세션 변경 시 UI 갱신
            _session.PropertyChanged += OnSessionPropertyChanged;

            // 앱이 메인으로 들어왔는데 메모리 세션이 비어 있으면 저장소에서 복원
            _ = InitializeAsync();
        }

        private async Task InitializeAsync()
        {
            if (_session.CurrentUser == null)
            {
                var loaded = await _storage.LoadAsync();
                if (loaded != null)
                {
                    _session.SignIn(loaded);
                    OnPropertyChanged(nameof(CurrentUser));
                }
            }
        }

        private void OnSessionPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ISessionService.CurrentUser))
                OnPropertyChanged(nameof(CurrentUser));
        }

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
        private async Task Logout()
        {
            _session.SignOut();
            await _storage.ClearAsync();

            Application.Current.MainWindow?.Close();
        }
    }
}
