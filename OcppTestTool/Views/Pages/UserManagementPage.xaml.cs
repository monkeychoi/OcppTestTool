using OcppTestTool.ViewModels.Pages;
using Wpf.Ui.Abstractions.Controls;

namespace OcppTestTool.Views.Pages
{
    public partial class UserManagementPage : INavigableView<UserManagementViewModel>
    {
        public UserManagementViewModel ViewModel { get; }

        public UserManagementPage(UserManagementViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();
        }
    }
}
