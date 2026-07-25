using System.Management;

namespace Diagnostish.Infrastructure.Shared.Wmi.Executor;

public class ExecutorWmi(Serilog.ILogger logger) : IExecutorWmi
{
    public void ExecuteSafeQuery(string query, string context, 
                                 List<string> warnings, List<string> criticalErrors, 
                                 Action<ManagementObjectCollection> wmiAction)
    {
        try
        {
            var options = new System.Management.EnumerationOptions
            {
                ReturnImmediately = true,
                Timeout = WmiSettings.RequestTimeout
            };

            using var searcher = new ManagementObjectSearcher(query) { Options = options };
            using var collection = searcher.Get();
            if (collection.Count == 0)
            {
                warnings.Add(ExecutorMessages.EmptyCollection(context));
                logger.Warning("Не удалось получить данные по запросу: {Query}.", query);
                return;
            }
            wmiAction(collection);
        }
        catch (ManagementException mex) when (mex.ErrorCode == ManagementStatus.Timedout)
        {
            criticalErrors.Add(ExecutorMessages.Timeout(context));
            logger.Error(mex, "Критический таймаут WMI при запросе: {Query}.", query);
        }
        catch (Exception ex)
        {
            warnings.Add(ExecutorMessages.GenericFail(context, ex.Message));
            logger.Error(ex, "Сбой выполнения WMI запроса: {Query}.", query);
        }
    }
}