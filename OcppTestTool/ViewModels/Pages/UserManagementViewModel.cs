using OcppTestTool.Models;
using OcppTestTool.Models.Entities.User;
using System.Collections.ObjectModel;
using System.Windows.Media;
using Wpf.Ui.Abstractions.Controls;

namespace OcppTestTool.ViewModels.Pages
{
    public partial class UserManagementViewModel : ObservableObject, INavigationAware
    {
        private bool _isInitialized = false;

        [ObservableProperty]
        private ObservableCollection<User> _users;

        public Task OnNavigatedToAsync()
        {
            if (!_isInitialized)
                InitializeViewModel();

            return Task.CompletedTask;
        }

        public Task OnNavigatedFromAsync() => Task.CompletedTask;

        private void InitializeViewModel()
        {
            Users = new ObservableCollection<User>
            {
                new User { UserId = "user1", UserName = "Alice", Email = "aaa@aaa.com", Role = "Admin" },
                new User { UserId = "user2", UserName = "Suzi", Email = "bbb@bbb.com", Role = "Admin" },
            };

            _isInitialized = true;
        }
    }
}
