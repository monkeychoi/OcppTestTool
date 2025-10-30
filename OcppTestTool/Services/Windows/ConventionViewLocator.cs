using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcppTestTool.Services.Windows
{
    public sealed class ConventionViewLocator : IViewLocator
    {
        public Window CreateWindowForViewModel(Type vmType, IServiceProvider sp)
        {
            // 규칙 예: OcppTestTool.ViewModels.Windows.CreateProtocolWindowViewModel
            //   ->    OcppTestTool.Views.Windows.CreateProtocolWindow
            var vmNs = vmType.Namespace ?? "";
            var viewNs = vmNs.Replace(".ViewModels.", ".Views.");
            var viewName = vmType.Name.Replace("ViewModel", "");
            var fullName = $"{viewNs}.{viewName}";

            var viewType = vmType.Assembly.GetType(fullName, throwOnError: true)!;
            var window = (Window)ActivatorUtilities.CreateInstance(sp, viewType);
            return window;
        }
    }
}
