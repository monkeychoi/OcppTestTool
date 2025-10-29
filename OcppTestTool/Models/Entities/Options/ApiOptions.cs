using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcppTestTool.Models.Entities.Options
{
    public sealed class ApiOptions
    {
        public string BaseUrl { get; init; } = "";
        public int TimeoutSeconds { get; init; } = 30;
    }
}
