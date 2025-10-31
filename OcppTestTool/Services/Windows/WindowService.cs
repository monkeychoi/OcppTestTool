using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcppTestTool.Services.Windows
{
    public sealed class WindowService : IWindowService
    {
        private readonly IServiceProvider _sp;
        private readonly IViewLocator _locator;

        public WindowService(IServiceProvider sp, IViewLocator locator)
        {
            _sp = sp;
            _locator = locator;
        }

        public Task<TResult?> ShowDialogAsync<TViewModel, TResult>(
            Action<TViewModel>? init = null)
            where TViewModel : class, IModalViewModel<TResult>
        {
            // VM 만들기 (DI 주입 포함)
            var vm = ActivatorUtilities.CreateInstance<TViewModel>(_sp);
            init?.Invoke(vm);

            // Window 찾고 생성
            var win = _locator.CreateWindowForViewModel(typeof(TViewModel), _sp);

            // Owner = 활성 창 or MainWindow
            var owner = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive)
                        ?? Application.Current.MainWindow;
            if (owner is not null)
            {
                win.Owner = owner;
                win.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            }

            // 닫힘 처리
            var tcs = new TaskCompletionSource<TResult?>();
            EventHandler<TResult?>? handler = null;

            void Cleanup()
            {
                vm.CloseRequested -= handler;
                win.Closed -= onClosed;
            }
            void onClosed(object? s, EventArgs e)
            {
                // 사용자가 X로 닫은 경우 → 취소로 간주
                if (!tcs.Task.IsCompleted) tcs.TrySetResult(default);
                Cleanup();
            }

            handler = (s, result) =>
            {
                Cleanup();
                try { win.DialogResult = result is not null; } catch { /* ignore */ }
                win.Close();
                tcs.TrySetResult(result);
            };
            vm.CloseRequested += handler;
            win.Closed += onClosed;

            win.DataContext = vm;

            // 모달 표시
            var _ = win.ShowDialog(); // 동기 반환, 하지만 우리는 tcs로 결과 받음
                                      
            // CloseRequested가 호출되지 않았다면 취소로 간주
            if (!tcs.Task.IsCompleted) tcs.TrySetResult(default);

            return tcs.Task;
        }
    }
}
