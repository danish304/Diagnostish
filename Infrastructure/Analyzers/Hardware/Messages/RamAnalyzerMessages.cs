namespace Infrastructure.Analyzers.Hardware.Messages;

internal static class RamAnalyzerMessages
{
    internal const string UNKNOWN_TYPE = "Не удалось определить тип модулей ОЗУ.";
    internal const string UNKNOWN_CAPACITY = "Не удалось определить ёмкость модулей ОЗУ.";
    internal const string UNKNOWN_SPEED = "Не удалось определить частоту модулей ОЗУ.";

    internal const string TYPE_CONFLICT = "Установлены модули ОЗУ разного типа! Неизвестно с каким типом работает ПК!";
    internal const string SPEED_CONFLICT = "Установлены модули ОЗУ с разной частотой.";
}