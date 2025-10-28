using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OcppTestTool.Views.Pages;
using OcppTestTool.Views.Windows;
using Wpf.Ui;

namespace OcppTestTool.Services
{
    /// <summary>
    /// Managed host of the application.
    /// </summary>
    public class ApplicationHostService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        private INavigationWindow _navigationWindow;

        public ApplicationHostService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Triggered when the application host is ready to start the service.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await HandleActivationAsync();
        }

        /// <summary>
        /// Triggered when the application host is performing a graceful shutdown.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the shutdown process should no longer be graceful.</param>
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }

        /// <summary>
        /// Creates main window during activation.
        /// </summary>
        private async Task HandleActivationAsync()
        {
            var app = Application.Current;

            // 0) 종료 모드 보관 & 로그인 동안 앱 종료 금지
            var prevShutdownMode = app.ShutdownMode;
            app.ShutdownMode = ShutdownMode.OnExplicitShutdown;

            // 1) 로그인 먼저
            var login = _serviceProvider.GetRequiredService<LoginWindow>();
            bool? result = login.ShowDialog();

            if (result != true)
            {
                // 로그인 취소/실패: 앱 종료
                app.Shutdown();
                return;
            }

            // 2) 로그인 성공 → 메인 네비게이션 윈도우 띄우기
            if (!app.Windows.OfType<MainWindow>().Any())
            {
                //_navigationWindow = (
                //    _serviceProvider.GetService(typeof(INavigationWindow)) as INavigationWindow
                //)!;
                //_navigationWindow!.ShowWindow();

                //_navigationWindow.Navigate(typeof(Views.Pages.DashboardPage));

                _navigationWindow = _serviceProvider.GetRequiredService<INavigationWindow>();
                var window = (Window)_navigationWindow;

                // MainWindow로 지정 (중요)
                app.MainWindow = window;

                // Loaded 이후에 Navigate (null 방지)
                window.Loaded += (_, __) =>
                {
                    // 원하는 시작 페이지
                    _navigationWindow.Navigate(typeof(Views.Pages.DashboardPage));
                };

                _navigationWindow.ShowWindow();
            }

            // 3) 종료 모드 원복 (보통 OnMainWindowClose 또는 이전값)
            app.ShutdownMode = prevShutdownMode == ShutdownMode.OnExplicitShutdown
                ? ShutdownMode.OnMainWindowClose
                : prevShutdownMode;

            await Task.CompletedTask;
        }
    }
}
