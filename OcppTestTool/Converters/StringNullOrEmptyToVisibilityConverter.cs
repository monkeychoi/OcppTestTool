using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace OcppTestTool.Converters
{
    public sealed class StringNullOrEmptyToVisibilityConverter : IValueConverter
    {
        public Visibility FalseVisibility { get; set; } = Visibility.Collapsed;
        public bool Invert { get; set; } // true면: 비어있으면 Visible

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string s = value as string ?? string.Empty;

            // 파라미터로 FalseVisibility 제어
            if (parameter is string p)
            {
                if (string.Equals(p, "Hidden", StringComparison.OrdinalIgnoreCase))
                    FalseVisibility = Visibility.Hidden;
                else if (string.Equals(p, "Collapsed", StringComparison.OrdinalIgnoreCase))
                    FalseVisibility = Visibility.Collapsed;
            }

            bool hasText = !string.IsNullOrWhiteSpace(s);
            if (Invert) hasText = !hasText;

            return hasText ? Visibility.Visible : FalseVisibility;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => Binding.DoNothing;
    }
}
