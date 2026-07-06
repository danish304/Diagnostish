using Diagnostish.Helpers;
using Diagnostish.Models;
using System.Management;

namespace Diagnostish.Services
{
    public class CheckOSConfigurationWMI : IOSCheck
    {
        public OSReport CheckOSCFG()
        {
            var rep = new OSReport();

            var options = new System.Management.EnumerationOptions
            {
                Timeout = TimeSpan.FromSeconds(3) 
            };

            try
            {
                using var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem");
                searcher.Options = options;

                foreach (ManagementObject item in searcher.Get())
                {
                    using (item)
                    {
                        rep.Name = Parser.ToString(item["Caption"]);
                        rep.Version = Parser.ToString(item["Version"]);
                        rep.Manufacturer = Parser.ToString(item["Manufacturer"]);
                        rep.RegisteredUser = Parser.ToString(item["RegisteredUser"]);

                        rep.InstallDate = Parser.ToDateTime(item["InstallDate"]);
                        if (!rep.InstallDate.HasValue) rep.Errors.Add("Не удалось определить дату установки ОС!");

                        rep.LastBootUpTime = Parser.ToDateTime(item["LastBootUpTime"]);
                        if (!rep.LastBootUpTime.HasValue) rep.Errors.Add("Не удалось определить дату последнего запуска ОС!");
                    }
                }
            }
            catch (ManagementException mex) when (mex.ErrorCode == ManagementStatus.Timedout)
            {
                rep.CriticalErrors.Add("Получение данных об ОС остановлено по таймауту (WMI завис).");
            }
            catch (Exception ex)
            {
                rep.Errors.Add("Ошибка получения данных об ОС: " + ex.Message);
            }

            return rep;
        }
    }
}
