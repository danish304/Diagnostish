using System.Management;
using Diagnostish.Models;
using Diagnostish.Helpers;

namespace Diagnostish.Services
{
    public class CheckPCConfigurationWMI : IHWCheck
    {
        public HWReport CheckPCCFG()
        {
            const double BytesInGigabytes = 1024 * 1024 * 1024;    // Константа для перевода из байт в гигабайты
            var rep = new HWReport();

            // Распознавание названия процессора, количества его ядер, базовой частоты
            try
            {
                using var searcher = new ManagementObjectSearcher("SELECT Name, NumberOfCores, CurrentClockSpeed FROM Win32_Processor");

                foreach (var item in searcher.Get())
                {
                    using (item)
                    {
                        rep.ProcessorName = Parser.ToString(item["Name"]);

                        int? cores = Parser.ToInt(item["NumberOfCores"]);
                        if (cores.HasValue) rep.CoresCount = cores.Value;
                        else rep.Errors.Add("Не удалось получить количество ядер процессора!");

                        int? clockspeed = Parser.ToInt(item["CurrentClockSpeed"]);
                        if (clockspeed.HasValue) rep.CurrentClockSpeed = clockspeed.Value;
                        else rep.Errors.Add("Не удалось получить частоту процессора!");
                    }
                }
            }
            catch (Exception ex)
            {
                rep.Errors.Add("Ошибка получения данных о процессоре: " + ex.Message);
            }

            // Распознавание общего количества ОЗУ и ее скорости
            try
            {
                using var searcher = new ManagementObjectSearcher("SELECT Capacity, Speed FROM Win32_PhysicalMemory");

                double totalBytes = 0;
                foreach (var item in searcher.Get())
                {
                    using (item)
                    {
                        double? capacity = Parser.ToDouble(item["Capacity"]);
                        if (capacity.HasValue) totalBytes += capacity.Value;
                        else rep.Errors.Add("Не удалось определить емкость одной из планок ОЗУ!");

                        int? speed = Parser.ToInt(item["Speed"]);
                        if (speed.HasValue) rep.RAMSpeed = speed.Value;
                        else rep.Errors.Add("Не удалось определить скорость ОЗУ!");
                    }
                }
                if (totalBytes > 0)
                {
                    rep.RAMSize = Math.Round(totalBytes / BytesInGigabytes, 2);
                }
            }
            catch (Exception ex)
            {
                rep.Errors.Add("Ошибка получения данных об ОЗУ: " + ex.Message);
            }

            // Распознавание названий видеокарт
            try
            {
                using var searcher = new ManagementObjectSearcher("SELECT Name FROM Win32_VideoController");

                foreach (var item in searcher.Get())
                {
                    using (item)
                    {
                        string gpu = Parser.ToString(item["Name"]);
                        rep.VideoCards.Add(gpu);
                    }
                }
            }
            catch (Exception ex)
            {
                rep.Errors.Add("Ошибка получения данных о видеокартах: " + ex.Message);
            }

            // Распознавание названия и объема накопителей
            try
            {
                using var searcher = new ManagementObjectSearcher("SELECT Model, Size FROM Win32_DiskDrive");

                foreach (var item in searcher.Get())
                {
                    using (item)
                    {
                        string model = Parser.ToString(item["Model"]);
                        double? size = Parser.ToDouble(item["Size"]);
                        if (size.HasValue)
                        {
                            double sizeGB = Math.Round(size.Value / BytesInGigabytes, 0);
                            string drive = $"{model} ({sizeGB} GB)";
                            rep.Drives.Add(drive);
                        }
                        else rep.Drives.Add($"{model} (size not found)");
                    }
                }
            }
            catch (Exception ex)
            {
                rep.Errors.Add("Ошибка получения данных о накопителях: " + ex.Message);
            }

            return rep;
        }
    }
}
