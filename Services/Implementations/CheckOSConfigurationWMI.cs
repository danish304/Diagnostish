using Diagnostish.Helpers;
using Diagnostish.Models;
using System.Management;
using Diagnostish.Services.Interfaces;

namespace Diagnostish.Services.Implementations
{
    public class CheckOSConfigurationWMI : IOSCheck
    {
        public OSReport CheckOSCFG()
        {
            var rep = new OSReport();

            GetOsGeneralInfo(rep);
            GetOsDateTimeInfo(rep);

            return rep;
        }

        private void GetOsGeneralInfo(OSReport rep)
        {
            string query = "SELECT Caption, Version, Manufacturer, RegisteredUser FROM Win32_OperatingSystem";

            SafeExecutor.ExecuteSafeQuery(query, "базовых данных ОС", rep.Errors, rep.CriticalErrors, searcher =>
            {
                foreach (ManagementObject item in searcher.Get())
                {
                    using (item)
                    {
                        rep.Name = Parser.ToString(item["Caption"]);
                        rep.Version = Parser.ToString(item["Version"]);
                        rep.Manufacturer = Parser.ToString(item["Manufacturer"]);
                        rep.RegisteredUser = Parser.ToString(item["RegisteredUser"]);
                    }
                }
            });
        }

        private void GetOsDateTimeInfo(OSReport rep)
        {
            string query = "SELECT InstallDate, LastBootUpTime FROM Win32_OperatingSystem";

            SafeExecutor.ExecuteSafeQuery(query, "временных метках ОС", rep.Errors, rep.CriticalErrors, searcher =>
            {
                foreach (ManagementObject item in searcher.Get())
                {
                    using (item)
                    {
                        rep.InstallDate = Parser.ToDateTime(item["InstallDate"]);
                        if (!rep.InstallDate.HasValue)
                        {
                            rep.Errors.Add("Не удалось определить дату установки ОС!");
                        }

                        rep.LastBootUpTime = Parser.ToDateTime(item["LastBootUpTime"]);
                        if (!rep.LastBootUpTime.HasValue)
                        {
                            rep.Errors.Add("Не удалось определить дату последнего запуска ОС!");
                        }
                    }
                }
            });
        }
    }
}
