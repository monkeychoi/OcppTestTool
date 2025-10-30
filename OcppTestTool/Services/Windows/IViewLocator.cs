using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcppTestTool.Services.Windows
{
    public interface IViewLocator
    {
        Window CreateWindowForViewModel(Type viewModelType, IServiceProvider sp);
    }
}
