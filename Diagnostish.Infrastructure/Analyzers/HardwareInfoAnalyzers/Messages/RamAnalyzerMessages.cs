namespace Diagnostish.Infrastructure.Analyzers.HardwareInfoAnalyzers.Messages;

internal static class RamAnalyzerMessages
{
    public const string UnknownType = "Не удалось определить тип модулей ОЗУ.";
    public const string UnknownCapacity = "Не удалось определить ёмкость модулей ОЗУ.";
    public const string UnknownSpeed = "Не удалось определить частоту модулей ОЗУ.";

    public const string TypeConflict = "Установлены модули ОЗУ разного типа! Неизвестно с каким типом работает ПК!";
    public const string SpeedConflict = "Установлены модули ОЗУ с разной частотой.";
}