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
        private readonly IServiceProvider _sp;
        private bool _isTransitioning;

        private INavigationWindow _navigationWindow;

        public ApplicationHostService(IServiceProvider sp)
        {
            _sp = sp;
        }

        /// <summary>
        /// Triggered when the application host is ready to start the service.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            // 🔒 로그인/전환 동안 자동 종료 방지
            Application.Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;

            // 앱 시작 루프
            Application.Current.Dispatcher.Invoke(ShowLoginWindow);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Triggered when the application host is performing a graceful shutdown.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the shutdown process should no longer be graceful.</param>
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }


        private void ShowLoginWindow()
        {
            if (_isTransitioning) return;
            _isTransitioning = true;

            try
            {
                var app = Application.Current;
                var original = app.ShutdownMode;                

                var loginWindow = _sp.GetRequiredService<LoginWindow>();
                bool? result = loginWindow.ShowDialog();   // 성공 시 LoginWindow에서 DialogResult = true

                if (result == true)
                {
                    // 메인 창 새로 생성
                    var navWindow = _sp.GetRequiredService<INavigationWindow>();
                    var mainWindow = (Window)navWindow;

                    // 이전 핸들러 중복 방지(혹시나)
                    mainWindow.Closed -= MainWindow_Closed;

                    app.MainWindow = mainWindow;

                    // 시작 페이지 이동 등 필요한 초기화
                    mainWindow.Loaded += (_, __) =>
                    {
                        navWindow.Navigate(typeof(Views.Pages.DashboardPage));
                    };

                    navWindow.ShowWindow();  // 또는 mainWindow.Show()

                    // 메인 닫히면 다시 로그인 띄우기
                    mainWindow.Closed += MainWindow_Closed;                    

                }
                else
                {
                    app.Shutdown(); // 로그인 취소/실패
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"창 생성 실패: {ex.Message}\n{ex.StackTrace}",
                    "오류",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
                Application.Current.Shutdown();
            }
            finally
            {
                _isTransitioning = false;
            }
        }

        private void MainWindow_Closed(object? sender, EventArgs e)
        {
            if (sender is Window w)
                w.Closed -= MainWindow_Closed;

            // 메인 닫히면 다시 로그인 루프 재진입
            //Application.Current.Dispatcher.BeginInvoke(new Action(ShowLoginWindow));

            ShowLoginWindow();
        }


    }
}
