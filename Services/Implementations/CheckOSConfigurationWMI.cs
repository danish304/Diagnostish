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

            try
            {
                using var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem");

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
                        if (!rep.LastBootUpTime.HasValue) rep.Errors.Add("Ну удалось определить дату последнего запуска ОС!");
                    }
                }
            }
            catch (Exception ex)
            {
                rep.Errors.Add("Ошибка получения данных об ОС: " + ex.Message);
            }

            return rep;
        }
    }
}
