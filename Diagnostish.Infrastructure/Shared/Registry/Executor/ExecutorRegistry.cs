using Microsoft.Win32;

namespace Diagnostish.Infrastructure.Shared.Registry.Executor;

public class ExecutorRegistry(Serilog.ILogger logger) : IExucutorRegistry
{
    public async Task ExecuteSafeReadAsync(string rootPath, string context,
                                           List<string> warnings, List<string> criticalErrors,
                                           Action<RegistryKey> registryAction,
                                           CancellationToken cancellationToken = default)
    {
        try
        {
            cancellationToken.ThrowIfCancellationRequested();

            using var key = await Task.Run(() =>
                RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey(rootPath),
                cancellationToken);

            if (key is null)
            {
                warnings.Add(RegistryExecutorMessages.KeyNotFound(context));
                logger.Warning("Ключ реестра не найден: {Path}.", rootPath);
                return;
            }

            registryAction(key);
        }
        catch (OperationCanceledException)
        {
            warnings.Add(RegistryExecutorMessages.Cancelled(context));
            logger.Information("Чтение реестра отменено: {Path}.", rootPath);
        }
        catch (Exception ex)
        {
            criticalErrors.Add(RegistryExecutorMessages.GenericFail(context, ex.Message));
            logger.Error(ex, "Ошибка при выполнении действия с реестром: {Path}.", rootPath);
        }
    }
}