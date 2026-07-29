namespace Diagnostish.Infrastructure.Shared.Wmi.Executor;

internal static class ExecutorMessages
{
    public static string Cancelled(string context) => $"Получение данных {context} было отменено.";
    public static string EmptyCollection(string context) => $"Не удалось получить данные {context} (WMI вернул пустой результат).";
    public static string Timeout(string context) => $"Получение данных {context} остановлено по таймауту (WMI завис).";
    public static string GenericFail(string context, string exceptionMessage) => $"Ошибка получения данных {context}: {exceptionMessage}.";
}