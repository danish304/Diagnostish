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

            GetOsInfo(rep);

            return rep;
        }

        private static void GetOsInfo(OSReport rep)
        {
            string query = "SELECT Caption, Version, Manufacturer, RegisteredUser, InstallDate, LastBootUpTime FROM Win32_OperatingSystem";

            SafeExecutor.ExecuteSafeQuery(query, "данных ОС", rep.Errors, rep.CriticalErrors, collection =>
            {
                foreach (var item in collection)
                {
                    using (item)
                    {
                        rep.Name = Parser.ToSafeString(item["Caption"]);
                        rep.Version = Parser.ToSafeString(item["Version"]);
                        rep.Manufacturer = Parser.ToSafeString(item["Manufacturer"]);
                        rep.RegisteredUser = Parser.ToSafeString(item["RegisteredUser"]);

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
