namespace Diagnostish.Infrastructure.Shared.Registry.Executor;

internal static class RegistryExecutorMessages
{
    public static string Cancelled(string context) => $"Чтение данных {context} из реестра отменено.";
    public static string KeyNotFound(string context) => $"Не удалось получить данные {context} (ключ не найден).";
    public static string GenericFail(string context, string exceptionMessage) => $"Ошибка получения данных {context} через реестр: {exceptionMessage}.";
}