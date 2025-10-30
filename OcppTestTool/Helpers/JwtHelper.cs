using OcppTestTool.Models.Dtos.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OcppTestTool.Helpers
{
    public static class JwtHelper
    {
        private static readonly JsonSerializerOptions _json = new(JsonSerializerDefaults.Web)
        {
            PropertyNameCaseInsensitive = true
        };

        public static bool TryReadPayload(string jwt, out JwtPayloadDto? payload)
        {
            payload = null;
            if (string.IsNullOrWhiteSpace(jwt)) return false;

            var parts = jwt.Split('.');
            if (parts.Length < 2) return false;

            try
            {
                var json = Encoding.UTF8.GetString(Base64UrlDecode(parts[1]));
                payload = JsonSerializer.Deserialize<JwtPayloadDto>(json, _json);
                return payload != null;
            }
            catch
            {
                return false;
            }
        }

        private static byte[] Base64UrlDecode(string input)
        {
            string s = input.Replace('-', '+').Replace('_', '/');
            switch (s.Length % 4)
            {
                case 2: s += "=="; break;
                case 3: s += "="; break;
            }
            return Convert.FromBase64String(s);
        }
    }
}
