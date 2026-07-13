using Serilog;
using System.Management;

namespace Diagnostish.Helpers
{
    public static class SafeExecutor
    {
        public static void ExecuteSafeQuery(string query, string context,
                                            List<string> errors, List<string> criticalErrors,
                                            Action<ManagementObjectCollection> wmiAction)
        {
            try
            {
                var options = new System.Management.EnumerationOptions
                {
                    ReturnImmediately = true,
                    Timeout = WMISettings.RequestTimeout
                };

                using var searcher = new ManagementObjectSearcher(query) { Options = options };
                using var collection = searcher.Get();
                if (collection.Count == 0)
                {
                    errors.Add($"Не удалось получить данные о/об {context} (WMI вернул пустой результат).");
                    return;
                }
                wmiAction(collection);
            }
            catch (UnauthorizedAccessException)
            {
                criticalErrors.Add($"Нет прав доступа для получения данных о/об {context}. Запустите утилиту от имени администратора!");
                Log.Error($"Отказано в доступе к WMI при запросе `{query}` для {context}.");
            }
            catch (ManagementException mex) when (mex.ErrorCode == ManagementStatus.AccessDenied)
            {
                criticalErrors.Add($"Доступ к WMI для {context} запрещён. Проверьте права администратора.");
                Log.Error($"WMI AccessDenied при запросе `{query}`.");
            }
            catch (ManagementException mex) when (mex.ErrorCode == ManagementStatus.Timedout)
            {
                criticalErrors.Add($"Получение данных о/об {context} остановлено по таймауту (WMI завис).");
                Log.Fatal(mex, $"Критический таймаут WMI при запросе `{query}` для {context}");
            }
            catch (Exception ex)
            {
                errors.Add($"Ошибка получения данных о/об {context}: {ex.Message}");
                Log.Error(ex, $"Сбой выполнения WMI запроса `{query}`");
            }
        }
    }
}
