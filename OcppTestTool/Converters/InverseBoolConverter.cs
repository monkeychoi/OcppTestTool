using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace OcppTestTool.Converters
{
    /// <summary>
    /// bool 값을 반전시켜 반환합니다. (true -> false, false -> true)
    /// </summary>
    public sealed class InverseBoolConverter : IValueConverter
    {
        /// <summary>
        /// null 입력을 어떻게 처리할지 선택합니다. 기본값은 false 로 간주합니다.
        /// </summary>
        public bool TreatNullAsTrue { get; set; } = false;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool b = value is bool v ? v : (TreatNullAsTrue ? true : false);
            return !b;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // 양방향 바인딩이 필요하다면 반전해서 돌려줍니다.
            bool b = value is bool v && v;
            return !b;
        }
    }
}
