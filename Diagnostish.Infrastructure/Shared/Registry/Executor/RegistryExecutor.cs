using Microsoft.Win32;
using Serilog;

using static Diagnostish.Infrastructure.Shared.Registry.Executor.RegistryExecutorMessages;

namespace Diagnostish.Infrastructure.Shared.Registry.Executor;

public class RegistryExecutor(ILogger logger) : IRegistryExecutor
{
    public async Task ExecuteSafeReadAsync(
        string rootPath, 
        string context,
        List<string> warnings, 
        List<string> criticalErrors,
        Action<RegistryKey> registryAction,
        CancellationToken cancellationToken = default)
    {
        try
        {
            cancellationToken.ThrowIfCancellationRequested();

            using var key = await Task.Run(() =>
                RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64)
                    .OpenSubKey(rootPath),
                cancellationToken);

            if (key is null)
            {
                warnings.Add(KeyNotFound(context));
                logger.Warning("Ключ реестра не найден: {Path}.", rootPath);

                return;
            }

            registryAction(key);
        }
        catch (OperationCanceledException)
        {
            warnings.Add(Cancelled(context));
            logger.Information("Чтение реестра отменено: {Path}.", rootPath);
        }
        catch (Exception ex)
        {
            criticalErrors.Add(GenericFail(context, ex.Message));
            logger.Error(ex, "Ошибка при выполнении действия с реестром: {Path}.", rootPath);
        }
    }
}