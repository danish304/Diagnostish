using Diagnostish.Models;
using System.Management;

namespace Diagnostish.Services
{
    public class OSCheck
    {
        public OSReport CheckOSCFG()
        {
            var rep = new OSReport();

            try
            {
                using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem"))
                {
                    foreach (ManagementObject item in searcher.Get())
                    {
                        rep.Name = item["Caption"]?.ToString() ?? "Unknown";
                        rep.Version = item["Version"]?.ToString() ?? "Unknown";
                        rep.Manufacturer = item["Manufacturer"]?.ToString() ?? "Unknown";
                        rep.RegisteredUser = item["RegisteredUser"]?.ToString() ?? "Unknown"; 
                        rep.InstallDate = ManagementDateTimeConverter.ToDateTime(item["InstallDate"]?.ToString());
                        rep.LastBootUpTime = ManagementDateTimeConverter.ToDateTime(item["LastBootUpTime"]?.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка получения данных об ОС: " + ex.Message);
            }

            return rep;
        }
    }
}
