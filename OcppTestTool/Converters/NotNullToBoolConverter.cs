using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace OcppTestTool.Converters
{
    public sealed class NotNullToBoolConverter : IValueConverter
    {
        public bool Invert { get; set; }  // 필요하면 반전 옵션

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => Invert ? value is null : value is not null;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}
