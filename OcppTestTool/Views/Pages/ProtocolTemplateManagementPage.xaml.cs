using OcppTestTool.ViewModels.Pages;
using Wpf.Ui.Abstractions.Controls;

namespace OcppTestTool.Views.Pages
{
    public partial class ProtocolTemplateManagementPage : INavigableView<ProtocolTemplateManagementViewModel>
    {
        public ProtocolTemplateManagementViewModel ViewModel { get; }

        public ProtocolTemplateManagementPage(ProtocolTemplateManagementViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();
        }
    }
}
