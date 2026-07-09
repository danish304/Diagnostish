using System.Globalization;
using System.Management;

namespace Diagnostish.Helpers
{
    public static class Parser
    {
        public static double? ToSafeDouble(object? value)
        {
            if (value == null || value == DBNull.Value) return null;

            if (double.TryParse(value.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out double result))
            {
                return result;
            }

            return null;
        }

        public static int? ToSafeInt(object? value)
        {
            if (value == null || value == DBNull.Value) return null;

            if (int.TryParse(value.ToString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out int result))
            {
                return result;
            }

            return null;
        }

        public static string ToSafeString(object? value)
        {
            if (value == null || value == DBNull.Value) return "Unknown";
            string? result = value.ToString()?.Trim();
            return string.IsNullOrEmpty(result) ? "Unknown" : result;
        }

        public static DateTime? ToSafeDateTime(object? value)
        {
            if (value == null || value == DBNull.Value) return null;

            try
            {
                string dtmString = value.ToString() ?? string.Empty;
                if (string.IsNullOrWhiteSpace(dtmString)) return null;

                return ManagementDateTimeConverter.ToDateTime(dtmString);
            }
            catch
            {
                return null;
            }
        }
    }
}
