using OcppTestTool.ViewModels.Pages;
using Wpf.Ui.Abstractions.Controls;

namespace OcppTestTool.Views.Pages
{
    public partial class ProtocolManagementPage : INavigableView<ProtocolManagementViewModel>
    {
        public ProtocolManagementViewModel ViewModel { get; }

        public ProtocolManagementPage(ProtocolManagementViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();
        }
    }
}
