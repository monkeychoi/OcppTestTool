using OcppTestTool.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace OcppTestTool.Converters
{
    public sealed class UtcToKstConverter : IValueConverter
    {
        public string Format { get; set; } = "yyyy-MM-dd HH:mm:ss";

        public object Convert(object value, Type t, object p, CultureInfo c)
        {
            if (value is DateTimeOffset dto)
                return KstTime.ToKst(dto).ToString(Format);
            if (value is DateTime dt && dt.Kind != DateTimeKind.Unspecified)
                return KstTime.ToKst(new DateTimeOffset(dt)).ToString(Format);
            return "";
        }

        public object ConvertBack(object v, Type t, object p, CultureInfo c) => Binding.DoNothing;
    }
}
