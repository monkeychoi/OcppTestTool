using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace OcppTestTool.Helpers
{
    public static class SecurePasswordBoxAssistant
    {
        public static readonly DependencyProperty BindSecurePasswordProperty =
            DependencyProperty.RegisterAttached(
                "BindSecurePassword",
                typeof(bool),
                typeof(SecurePasswordBoxAssistant),
                new PropertyMetadata(false, OnBindSecurePasswordChanged));

        public static readonly DependencyProperty BoundSecurePasswordProperty =
            DependencyProperty.RegisterAttached(
                "BoundSecurePassword",
                typeof(SecureString),
                typeof(SecurePasswordBoxAssistant),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnBoundSecurePasswordChanged));

        private static readonly DependencyProperty IsUpdatingProperty =
            DependencyProperty.RegisterAttached(
                "IsUpdating",
                typeof(bool),
                typeof(SecurePasswordBoxAssistant),
                new PropertyMetadata(false));

        public static bool GetBindSecurePassword(DependencyObject obj) => (bool)obj.GetValue(BindSecurePasswordProperty);
        public static void SetBindSecurePassword(DependencyObject obj, bool value) => obj.SetValue(BindSecurePasswordProperty, value);

        public static SecureString GetBoundSecurePassword(DependencyObject obj) => (SecureString)obj.GetValue(BoundSecurePasswordProperty);
        public static void SetBoundSecurePassword(DependencyObject obj, SecureString value) => obj.SetValue(BoundSecurePasswordProperty, value);

        private static bool GetIsUpdating(DependencyObject obj) => (bool)obj.GetValue(IsUpdatingProperty);
        private static void SetIsUpdating(DependencyObject obj, bool value) => obj.SetValue(IsUpdatingProperty, value);

        private static void OnBindSecurePasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not PasswordBox box) return;

            if ((bool)e.NewValue)
                box.PasswordChanged += HandlePasswordChanged;
            else
                box.PasswordChanged -= HandlePasswordChanged;
        }

        private static void OnBoundSecurePasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // 보안상: VM -> View로 다시 그려 넣는 건 지양 (PasswordBox.Password 설정 필요 없음)
            // 필요 시 별도 정책을 정의하세요.
        }

        private static void HandlePasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is not PasswordBox box) return;

            if (GetIsUpdating(box)) return;

            SetIsUpdating(box, true);
            // PasswordBox.SecurePassword는 복제하여 넘기는 것이 안전
            var copy = box.SecurePassword?.Copy() ?? new SecureString();
            SetBoundSecurePassword(box, copy);
            SetIsUpdating(box, false);
        }
    }
}
