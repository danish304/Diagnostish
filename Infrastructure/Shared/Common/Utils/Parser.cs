using System.Globalization;
using System.Management;

namespace Infrastructure.Shared.Common.Utils;

public static class Parser
{
    public static int? ToSafeInt(object? value)
    {
        if (value is null or DBNull)
        {
            return null;
        }

        return int.TryParse(
            value.ToString(),
            NumberStyles.Integer,
            CultureInfo.InvariantCulture,
            out int result) ? result : null;
    }

    public static double? ToSafeDouble(object? value)
    {
        if (value is null or DBNull)
        {
            return null;
        }

        return double.TryParse(
            value.ToString(),
            NumberStyles.Any,
            CultureInfo.InvariantCulture,
            out double result) ? result : null;
    }

    public static string? ToSafeString(object? value)
    {
        if (value is null or DBNull)
        {
            return null;
        }

        string result = value.ToString() ?? string.Empty;
        return string.IsNullOrWhiteSpace(result) ? null : result.Trim();
    }

    public static string? ToSafeArrayString(object? value)
    {
        if (value is null or DBNull)
        {
            return null;
        }

        if (value is string[] array)
        {
            return array.Length > 0 ? string.Join(", ", array) : null;
        }

        string result = value.ToString() ?? string.Empty;
        return string.IsNullOrWhiteSpace(result) ? null : result.Trim();
    }

    public static DateTime? ToSafeDateTime(object? value)
    {
        if (value is null or DBNull)
        {
            return null;
        }

        string dtmString = value.ToString() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(dtmString))
        {
            return null;
        }

        try
        {
            return ManagementDateTimeConverter.ToDateTime(dtmString);
        }
        catch
        {
            return null;
        }
    }
}