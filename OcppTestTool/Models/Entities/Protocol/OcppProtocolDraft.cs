using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcppTestTool.Models.Entities.Protocol
{
    public partial class OcppProtocolDraft : ObservableObject
    {
        [ObservableProperty] private int id;
        [ObservableProperty] private string name = "";
        [ObservableProperty] private string version = "";
        [ObservableProperty] private string transport = "WebSocket";
        [ObservableProperty] private string codec = "JSON";
        [ObservableProperty] private string? description;
    }
}
