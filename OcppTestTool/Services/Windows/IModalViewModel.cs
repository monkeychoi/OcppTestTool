using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcppTestTool.Services.Windows
{
    public interface IModalViewModel<TResult>
    {
        event EventHandler<TResult?>? CloseRequested;
    }
}
