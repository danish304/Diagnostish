using Microsoft.Extensions.Options;
using Serilog;
using System.Management;

using static Infrastructure.Shared.Wmi.Executor.WmiExecutorMessages;

namespace Infrastructure.Shared.Wmi.Executor;

public class WmiExecutor(
    ILogger logger,
    IOptions<WmiSettings> settings)
    : IWmiExecutor
{
    public async Task ExecuteSafeQueryAsync(
        string query,
        string context,
        List<string> warnings,
        List<string> criticalErrors,
        Action<ManagementObjectCollection> wmiAction,
        CancellationToken cancellationToken = default)
    {
        try
        {
            cancellationToken.ThrowIfCancellationRequested();

            var options = new System.Management.EnumerationOptions
            {
                ReturnImmediately = true,
                Timeout = settings.Value.WmiQueryTimeout
            };

            using var searcher = new ManagementObjectSearcher(query)
            {
                Options = options
            };

            using var collection = await Task.Run(() =>
                searcher.Get(), cancellationToken);

            if (collection.Count == 0)
            {
                warnings.Add(EmptyCollection(context));
                logger.Warning("Не удалось получить данные по запросу: {Query}.", query);

                return;
            }
            wmiAction(collection);
        }
        catch (OperationCanceledException)
        {
            warnings.Add(Cancelled(context));
            logger.Information("Запрос отменён: {Query}.", query);
        }
        catch (ManagementException mex) when (mex.ErrorCode == ManagementStatus.Timedout)
        {
            criticalErrors.Add(Timeout(context));
            logger.Error(mex, "Критический таймаут WMI при запросе: {Query}.", query);
        }
        catch (Exception ex)
        {
            warnings.Add(GenericFail(context, ex.Message));
            logger.Error(ex, "Сбой выполнения WMI запроса: {Query}.", query);
        }
    }
}