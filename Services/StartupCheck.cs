using System.Management;
using Diagnostish.Models;

namespace Diagnostish.Services
{
    public class StartupCheck
    {
        public List<StartupReport> GetStartupPrograms()
        {
            List<StartupReport> list = new List<StartupReport>();
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_StartupCommand");
                foreach (ManagementObject obj in searcher.Get())
                {
                    list.Add(new StartupReport
                    {
                        Name = obj["Name"]?.ToString() ?? "Unknown",
                        Command = obj["Command"]?.ToString() ?? "Unknown",
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка чтения автозагрузки: {ex.Message}");
            }
            return list;
        }
    }
}
