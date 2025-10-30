using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.Ui.Controls;

namespace OcppTestTool.Models.Entities.Common
{
    // Menu 정의(도메인 모델)
    public sealed record MenuDef(
        string Title,
        SymbolRegular Icon,
        Type PageType,
        string? RequiredRole = null
    );

    public static class MenuCatalog
    {
        public static readonly MenuDef[] All =
        {
            new("대시보드",   SymbolRegular.Home24,   typeof(Views.Pages.DashboardPage)),
            new("프로토콜관리", SymbolRegular.Settings24, typeof(Views.Pages.ProtocolManagementPage), "Admin"),
            new("사용자관리", SymbolRegular.People32, typeof(Views.Pages.UserManagementPage), "Admin"),
        };
    }
}
