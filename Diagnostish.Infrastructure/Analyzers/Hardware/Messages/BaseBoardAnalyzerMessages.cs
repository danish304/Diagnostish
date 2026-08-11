namespace Diagnostish.Infrastructure.Analyzers.Hardware.Messages;

internal static class BaseBoardAnalyzerMessages
{
    internal const string UNKNOWN_MODEL        = "Не удалось определить модель материнской платы.";
    internal const string UNKNOWN_MANUFACTURER = "Не удалось определить производителя материнской платы.";
    internal const string UNKNOWN_VERSION      = "Не удалось определить версию материнской платы.";
    internal const string UNKNOWN_STATUS       = "Не удалось определить статус материнской платы.";

    internal const string BAD_STATUS           = "Статус материнской платы отличается от нормы!";
}