namespace Diagnostish.Infrastructure.Shared.Wmi.Executor;

internal static class WmiExecutorMessages
{
    internal static string Cancelled(string context) =>
        $"Получение данных {context} было отменено.";

    internal static string EmptyCollection(string context) =>
        $"Не удалось получить данные {context} (WMI вернул пустой результат).";

    internal static string Timeout(string context) =>
        $"Получение данных {context} остановлено по таймауту (WMI завис).";

    internal static string GenericFail(string context, string exceptionMessage) =>
        $"Ошибка получения данных {context} через WMI: {exceptionMessage}.";
}