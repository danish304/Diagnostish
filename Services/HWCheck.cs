using System.Management;
using Diagnostish.Models;

namespace Diagnostish.Services
{
    public class HWCheck
    {
        public HWReport CheckPCCFG()
        {
            var rep = new HWReport();

            // Распознавание названия процессора, количества его ядер, базовой частоты
            try
            {
                using (var searcher = new ManagementObjectSearcher("SELECT Name, NumberOfCores, CurrentClockSpeed FROM Win32_Processor"))
                {
                    foreach (var item in searcher.Get())
                    {
                        rep.ProcessorName = item["Name"]?.ToString() ?? "Unknown";
                        rep.CoresCount = Convert.ToInt32(item["NumberOfCores"]);
                        rep.CurrentClockSpeed = Convert.ToInt32(item["CurrentClockSpeed"]);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка получения данных о процессоре: " + ex.Message);
            }

            // Распознавание общего количества ОЗУ и ее скорости
            try
            {
                using (var searcher = new ManagementObjectSearcher("SELECT Capacity, Speed FROM Win32_PhysicalMemory"))
                {
                    double totalBytes = 0;
                    foreach (var item in searcher.Get())
                    {
                        totalBytes += Convert.ToDouble(item["Capacity"]);
                        rep.RAMSpeed = Convert.ToInt32(item["Speed"]);
                    }
                    rep.RAMSize = Math.Round(totalBytes / (1024 * 1024 * 1024), 2);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка получения данных об ОЗУ: " + ex.Message);
            }

            // Распознавание названий видеокарт
            try
            {
                using (var searcher = new ManagementObjectSearcher("SELECT Name FROM Win32_VideoController"))
                {
                    foreach (var item in searcher.Get())
                    {
                        rep.VideoCards.Add(item["Name"]?.ToString() ?? "Unknown");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка получения данных о видеокартах: " + ex.Message);
            }

            // Распознавание названия и объема накопителей
            try
            {
                using (var searcher = new ManagementObjectSearcher("SELECT Model, Size FROM Win32_DiskDrive"))
                {
                    foreach (var item in searcher.Get())
                    {
                        string model = item["Model"]?.ToString() ?? "Unknown drive";
                        double sizeGb = Math.Round(Convert.ToDouble(item["Size"]) / (1024 * 1024 * 1024), 0);

                        rep.Drives.Add($"{model} ({sizeGb} GB)");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка получения данных о накопителях: " + ex.Message);
            }

            return rep;
        }
    }
}
