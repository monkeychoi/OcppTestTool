using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcppTestTool.Services.Windows
{
    public interface IWindowService
    {
        Task<TResult?> ShowDialogAsync<TViewModel, TResult>(
            Action<TViewModel>? init = null)
            where TViewModel : class, IModalViewModel<TResult>;
    }
}
