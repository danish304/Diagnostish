using Serilog;
using System.Management;

namespace Diagnostish.Helpers
{
    public static class SafeExecutor
    {
        public static void ExecuteSafeQuery(string query, string context,
                                            System.Management.EnumerationOptions options,
                                            List<string> errors, List<string> criticalErrors,
                                            Action<ManagementObjectSearcher> wmiAction)
        {
            try
            {
                options.Timeout = WMISettings.RequestTimeout;

                using var searcher = new ManagementObjectSearcher(query) { Options = options };
                wmiAction(searcher);
            }
            catch (ManagementException mex) when (mex.ErrorCode == ManagementStatus.Timedout)
            {
                criticalErrors.Add($"Получение данных о/об {context} остановлено по таймауту (WMI завис).");
                Log.Fatal(mex, $"Критический таймаут WMI при запросе '{query}' для {context}");
            }
            catch (Exception ex)
            {
                errors.Add($"Ошибка получения данных о/об {context}: {ex.Message}");
                Log.Error(ex, $"Сбой выполнения WMI запроса '{query}'");
            }
        }
    }
}
