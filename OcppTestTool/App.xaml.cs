using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using OcppTestTool.Models.Entities.Options;
using OcppTestTool.Services;
using OcppTestTool.Services.Auth;
using OcppTestTool.Services.Http;
using OcppTestTool.ViewModels.Pages;
using OcppTestTool.ViewModels.Windows;
using OcppTestTool.Views.Pages;
using OcppTestTool.Views.Windows;
using System.IO;
using System.Net.Http.Headers;
using System.Reflection;
using System.Windows.Threading;
using Wpf.Ui;
using Wpf.Ui.DependencyInjection;

namespace OcppTestTool
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        // The.NET Generic Host provides dependency injection, configuration, logging, and other services.
        // https://docs.microsoft.com/dotnet/core/extensions/generic-host
        // https://docs.microsoft.com/dotnet/core/extensions/dependency-injection
        // https://docs.microsoft.com/dotnet/core/extensions/configuration
        // https://docs.microsoft.com/dotnet/core/extensions/logging
        private static readonly IHost _host = Host
            .CreateDefaultBuilder()
            .ConfigureAppConfiguration((ctx, c) =>
            {
                var basePath = Path.GetDirectoryName(AppContext.BaseDirectory)!;
                c.SetBasePath(basePath);
                c.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                c.AddJsonFile($"appsettings.{ctx.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true);
                c.AddEnvironmentVariables();
            })
            .ConfigureAppConfiguration(c => { c.SetBasePath(Path.GetDirectoryName(AppContext.BaseDirectory)); })
            .ConfigureServices((context, services) =>
            {
                services.AddNavigationViewPageProvider();
                
                services.AddHostedService<ApplicationHostService>();

                // Theme manipulation
                services.AddSingleton<IThemeService, ThemeService>();

                // TaskBar manipulation
                services.AddSingleton<ITaskBarService, TaskBarService>();

                // Service containing navigation, same as INavigationWindow... but without window
                services.AddSingleton<INavigationService, NavigationService>();

                // Main window with navigation
                services.AddTransient<LoginWindow>();
                services.AddTransient<LoginWindowViewModel>();

                services.AddTransient<INavigationWindow, MainWindow>();
                services.AddTransient<MainWindowViewModel>();

                // Pages
                services.AddTransient<DashboardPage>();
                services.AddTransient<UserManagementPage>();
                services.AddTransient<DataPage>();
                services.AddTransient<SettingsPage>();

                // ViewModels
                services.AddTransient<DashboardViewModel>();
                services.AddTransient<UserManagementViewModel>();
                services.AddTransient<DataViewModel>();
                services.AddTransient<SettingsViewModel>();

                // Services
                services.AddSingleton<ISessionService, SessionService>();

                // 1) ApiOptions 바인딩 (appsettings.json의 "Api" 섹션)
                services.Configure<ApiOptions>(context.Configuration.GetSection("Api"));

                // 2) (선택) 인증 헤더 핸들러 — 세션 토큰 자동 첨부 용
                services.AddTransient<AuthHeaderHandler>();

                // 3) 공통 API 클라이언트 (GET/POST/PUT/DELETE 일반화)
                services.AddHttpClient<IApiClient, ApiClient>((sp, http) =>
                {
                    var opt = sp.GetRequiredService<IOptions<ApiOptions>>().Value;
                    if (!string.IsNullOrWhiteSpace(opt.BaseUrl))
                        http.BaseAddress = new Uri(opt.BaseUrl);
                    if (opt.TimeoutSeconds > 0)
                        http.Timeout = TimeSpan.FromSeconds(opt.TimeoutSeconds);

                    http.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));
                })
                .AddHttpMessageHandler<AuthHeaderHandler>(); // 필요 없으면 제거 가능

                // 4) 도메인 API 등록
                services.AddTransient<IAuthService, AuthService>();

               

            }).Build();

        /// <summary>
        /// Gets services.
        /// </summary>
        public static IServiceProvider Services
        {
            get { return _host.Services; }
        }

        /// <summary>
        /// Occurs when the application is loading.
        /// </summary>
        private async void OnStartup(object sender, StartupEventArgs e)
        {
            await _host.StartAsync();
        }

        /// <summary>
        /// Occurs when the application is closing.
        /// </summary>
        private async void OnExit(object sender, ExitEventArgs e)
        {
            await _host.StopAsync();

            _host.Dispose();
        }

        /// <summary>
        /// Occurs when an exception is thrown by an application but not handled.
        /// </summary>
        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            // For more info see https://docs.microsoft.com/en-us/dotnet/api/system.windows.application.dispatcherunhandledexception?view=windowsdesktop-6.0
        }
    }
}
