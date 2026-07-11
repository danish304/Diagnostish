using Diagnostish.Helpers;
using Diagnostish.Models;
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
                        rep.OpSystemName = Parser.ToSafeString(item["Caption"]);
                        rep.OpSystemVersion = Parser.ToSafeString(item["Version"]);
                        rep.OpSystemManufacturer = Parser.ToSafeString(item["Manufacturer"]);
                        rep.OpSystemRegisteredUser = Parser.ToSafeString(item["RegisteredUser"]);

                        rep.OpSystemInstallDate = Parser.ToSafeDateTime(item["InstallDate"]);
                        if (!rep.OpSystemInstallDate.HasValue || (rep.OpSystemInstallDate.Value == DateTime.MinValue))
                        {
                            rep.Errors.Add("Не удалось определить корректную дату установки ОС!");
                        }

                        rep.OpSystemLastBootUpTime = Parser.ToSafeDateTime(item["LastBootUpTime"]);
                        if (!rep.OpSystemLastBootUpTime.HasValue || (rep.OpSystemLastBootUpTime.Value == DateTime.MinValue))
                        {
                            rep.Errors.Add("Не удалось определить корректную дату последнего запуска ОС!");
                        }
                    }
                }
            });
        }
    }
}
