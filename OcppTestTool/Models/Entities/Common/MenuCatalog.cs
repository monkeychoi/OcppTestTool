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
            new("프로토콜 관리", SymbolRegular.Settings24, typeof(Views.Pages.ProtocolManagementPage), "Admin"),
            new("프로토콜 템플릿 관리", SymbolRegular.Settings24, typeof(Views.Pages.ProtocolTemplateManagementPage), "Admin"),
            new("사용자 관리", SymbolRegular.People32, typeof(Views.Pages.UserManagementPage), "Admin"),
        };
    }
}
