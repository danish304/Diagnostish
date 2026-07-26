using Serilog;
using System.Numerics;

namespace Diagnostish.Infrastructure.Analyzers.Common;

public static class AnalyzerValidationExtensions
{
    public static string GetValueOrWarning(this string? value,
                                           List<string> warnings, ILogger logger,
                                           string warningMessage,
                                           string defaultValue = "Неизвестно")
    {
        if (value is not null) return value;

        LogWarning(warnings, logger, warningMessage);
        return defaultValue;
    }

    public static T GetValueOrWarning<T>(this T? value,
                                         List<string> warnings, ILogger logger,
                                         string warningMessage,
                                         T defaultValue = default) where T : struct, INumber<T>
    {
        if (!value.HasValue || value.Value <= T.Zero)
        {
            LogWarning(warnings, logger, warningMessage);
            return defaultValue;
        }

        return value.Value;
    }

    public static DateTime GetValueOrWarning(this DateTime? value,
                                             List<string> warnings, ILogger logger,
                                             string warningMessage,
                                             DateTime defaultValue = default,
                                             Func<DateTime, bool>? condition = null)
    {
        if (!value.HasValue || (condition is not null && !condition(value.Value)))
        {
            LogWarning(warnings, logger, warningMessage);
            return defaultValue;
        }

        return value.Value;
    }

    private static void LogWarning(List<string> warnings, ILogger logger, string warningMessage)
    {
        warnings.Add(warningMessage);
        logger.Warning("{WarningMessage}", warningMessage);
    }
}