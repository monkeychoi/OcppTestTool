using Microsoft.Extensions.DependencyInjection;
using OcppTestTool.Models.Entities.Auth;
using OcppTestTool.Models.Entities.Common;
using OcppTestTool.Services.Auth;
using OcppTestTool.Views.Windows;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Wpf.Ui.Controls;

namespace OcppTestTool.ViewModels.Windows
{
    public partial class MainWindowViewModel : ObservableObject
    {
        private readonly ISessionService _session;
       

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
        }

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
            _session.SignOut();

            Application.Current.MainWindow?.Close();
        }
    }
}
