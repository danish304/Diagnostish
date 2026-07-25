namespace Diagnostish.Infrastructure.Analyzers.HardwareInfoAnalyzers.Messages;

internal static class BaseBoardAnalyzerMessages
{
    public const string UnknownModel = "Не удалось определить модель материнской платы.";
    public const string UnknownManufacturer = "Не удалось определить производителя материнской платы.";
    public const string UnknownVersion = "Не удалось определить версию материнской платы.";
    public const string UnknownStatus = "Не удалось определить статус материнской платы.";

    public const string BadStatus = "Статус материнской платы отличается от нормы!";
}