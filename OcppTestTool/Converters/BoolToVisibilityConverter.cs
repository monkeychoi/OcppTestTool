using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace OcppTestTool.Converters
{
    public sealed class BoolToVisibilityConverter : IValueConverter
    {
        // false일 때 Visibility. 기본은 Collapsed
        public Visibility FalseVisibility { get; set; } = Visibility.Collapsed;

        // true/false를 뒤집고 싶을 때
        public bool Invert { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool b = value is bool bo && bo;

            // 파라미터로 FalseVisibility 지정 가능: "Hidden"|"Collapsed"
            if (parameter is string p)
            {
                if (string.Equals(p, "Hidden", StringComparison.OrdinalIgnoreCase))
                    FalseVisibility = Visibility.Hidden;
                else if (string.Equals(p, "Collapsed", StringComparison.OrdinalIgnoreCase))
                    FalseVisibility = Visibility.Collapsed;
            }

            if (Invert) b = !b;

            return b ? Visibility.Visible : FalseVisibility;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => value is Visibility v && v == Visibility.Visible;
    }
}
