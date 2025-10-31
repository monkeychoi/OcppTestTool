using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageBox = Wpf.Ui.Controls.MessageBox;
using MessageBoxResult = Wpf.Ui.Controls.MessageBoxResult;

namespace OcppTestTool.Services.UI
{
    public interface IUiMessageService
    {
        Task ShowAsync(string title, string message);          // OK only
        Task<bool> ConfirmAsync(string title, string message); // OK/Cancel
    }

    public sealed class WpfUiMessageService : IUiMessageService
    {
        private static Window? FindOwner()
            => Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive);

        public async Task ShowAsync(string title, string message)
        {
            var box = new MessageBox
            {
                Title = title,
                Content = message,
                CloseButtonText = "확인",
                Owner = FindOwner(),
                MaxWidth = 560
            };
            await box.ShowDialogAsync();
        }

        public async Task<bool> ConfirmAsync(string title, string message)
        {
            var box = new MessageBox
            {
                Title = title,
                Content = message,
                PrimaryButtonText = "확인",
                CloseButtonText = "취소",
                Owner = FindOwner(),
                MaxWidth = 560
            };
            var result = await box.ShowDialogAsync();
            return result is MessageBoxResult.Primary;
        }
    }
}
