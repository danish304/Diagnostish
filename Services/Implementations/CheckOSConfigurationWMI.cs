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

            SafeExecutor.ExecuteSafeQuery(query, "базовых данных ОС", rep.Errors, rep.CriticalErrors, collection =>
            {
                foreach (ManagementObject item in collection)
                {
                    using (item)
                    {
                        rep.Name = Parser.ToSafeString(item["Caption"]);
                        rep.Version = Parser.ToSafeString(item["Version"]);
                        rep.Manufacturer = Parser.ToSafeString(item["Manufacturer"]);
                        rep.RegisteredUser = Parser.ToSafeString(item["RegisteredUser"]);
                    }
                }
            });
        }

        private void GetOsDateTimeInfo(OSReport rep)
        {
            string query = "SELECT InstallDate, LastBootUpTime FROM Win32_OperatingSystem";

            SafeExecutor.ExecuteSafeQuery(query, "временных метках ОС", rep.Errors, rep.CriticalErrors, collection =>
            {
                foreach (ManagementObject item in collection)
                {
                    using (item)
                    {
                        rep.InstallDate = Parser.ToSafeDateTime(item["InstallDate"]);
                        if (!rep.InstallDate.HasValue || (rep.InstallDate.Value == DateTime.MinValue))
                        {
                            rep.Errors.Add("Не удалось определить корректную дату установки ОС!");
                        }

                        rep.LastBootUpTime = Parser.ToSafeDateTime(item["LastBootUpTime"]);
                        if (!rep.LastBootUpTime.HasValue || (rep.LastBootUpTime.Value == DateTime.MinValue))
                        {
                            rep.Errors.Add("Не удалось определить корректную дату последнего запуска ОС!");
                        }
                    }
                }
            });
        }
    }
}
