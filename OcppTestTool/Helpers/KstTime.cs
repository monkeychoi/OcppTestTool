using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcppTestTool.Helpers
{
    public static class KstTime
    {
        private static readonly TimeZoneInfo Kst = GetKst();

        private static TimeZoneInfo GetKst()
        {
            // Linux/macOS: "Asia/Seoul", Windows: "Korea Standard Time"
            try { return TimeZoneInfo.FindSystemTimeZoneById("Asia/Seoul"); }
            catch { return TimeZoneInfo.FindSystemTimeZoneById("Korea Standard Time"); }
        }

        public static DateTimeOffset ToKst(DateTimeOffset utc)
        {
            var kstOffset = Kst.GetUtcOffset(utc.UtcDateTime);
            return utc.ToOffset(kstOffset);
        }

        public static string ToKstString(DateTimeOffset utc, string format = "yyyy-MM-dd HH:mm:ss")
            => ToKst(utc).ToString(format);
    }
}
