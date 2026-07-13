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
            const string CAPTION = "Caption";
            const string VERSION = "Version";
            const string MANUFACTURER = "Manufacturer";
            const string USER = "RegisteredUser";
            const string INSTALL = "InstallDate";
            const string LASTBOOT = "LastBootUpTime";

            string query = "SELECT" + CAPTION + VERSION + MANUFACTURER + USER + INSTALL + LASTBOOT + "FROM Win32_OperatingSystem";

            SafeExecutor.ExecuteSafeQuery(query, "данных ОС", rep.Errors, rep.CriticalErrors, collection =>
            {
                foreach (var item in collection)
                {
                    using (item)
                    {
                        rep.OpSystemName = Parser.ToSafeString(item[CAPTION]);
                        rep.OpSystemVersion = Parser.ToSafeString(item[VERSION]);
                        rep.OpSystemManufacturer = Parser.ToSafeString(item[MANUFACTURER]);
                        rep.OpSystemRegisteredUser = Parser.ToSafeString(item[USER]);

                        rep.OpSystemInstallDate = Parser.ToSafeDateTime(item[INSTALL]);
                        if (!rep.OpSystemInstallDate.HasValue || (rep.OpSystemInstallDate.Value == DateTime.MinValue))
                        {
                            rep.Errors.Add("Не удалось определить корректную дату установки ОС!");
                        }

                        rep.OpSystemLastBootUpTime = Parser.ToSafeDateTime(item[LASTBOOT]);
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
