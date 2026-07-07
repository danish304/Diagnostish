using System.Management;

namespace Diagnostish.Helpers
{
    public static class Parser
    {
        public static double? ToSafeDouble(object? value)
        {
            if (value == null || value == DBNull.Value) return null;
            try
            {
                return Convert.ToDouble(value);
            }
            catch
            {
                return null;
            }
        }

        public static int? ToSafeInt(object? value)
        {
            if (value == null || value == DBNull.Value) return null;
            try
            {
                return Convert.ToInt32(value);
            }
            catch
            {
                return null;
            }
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
