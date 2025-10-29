using OcppTestTool.Models;
using OcppTestTool.Services.Auth;
using OcppTestTool.ViewModels.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Wpf.Ui.Controls;

namespace OcppTestTool.Views.Windows
{
    
    public partial class LoginWindow : FluentWindow
    {
        public LoginWindowViewModel ViewModel { get; }

        public LoginWindow(LoginWindowViewModel vm)
        {
            InitializeComponent();
            ViewModel = vm;
            DataContext = this;
            ViewModel.CloseRequested += (_, ok) =>
            {
                DialogResult = ok;
                Close();
            };
        }
        
    }
}
