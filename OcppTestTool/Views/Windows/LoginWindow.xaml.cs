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
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false; // 취소
            Close();
        }

        private void OnLoginClick(object sender, RoutedEventArgs e)
        {
            var id = UserId.Text?.Trim();
            var pw = UserPassword.Password;

            // 임시 자격 증명: test / 1234
            if (id == "test" && pw == "1234")
            {
                DialogResult = true; // 성공
                Close();
            }
            else
            {
                // 간단 알림
                System.Windows.MessageBox.Show("아이디 또는 비밀번호가 올바르지 않습니다.", "로그인 실패",
                    System.Windows.MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
