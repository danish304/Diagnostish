namespace Diagnostish.Infrastructure.Shared.Registry.Executor;

internal static class RegistryExecutorMessages
{
    internal static string Cancelled(string context) =>
        $"Чтение данных {context} из реестра отменено.";

    internal static string KeyNotFound(string context) =>
        $"Не удалось получить данные {context} (ключ не найден).";

    internal static string GenericFail(string context, string exceptionMessage) =>
        $"Ошибка получения данных {context} через реестр: {exceptionMessage}.";
}