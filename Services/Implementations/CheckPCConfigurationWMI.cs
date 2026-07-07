using Diagnostish.Models;
using Diagnostish.Helpers;
using Diagnostish.Services.Interfaces;
using Serilog;

namespace Diagnostish.Services.Implementations
{
    public class CheckPCConfigurationWMI : IHWCheck
    {
        public HWReport CheckPCCFG()
        {
            var rep = new HWReport();

            GetCpuInfo(rep);
            GetRamInfo(rep);
            GetGpuInfo(rep);
            GetDrivesInfo(rep);

            return rep;
        }

        private void GetCpuInfo(HWReport rep)
        {
            string query = "SELECT Name, NumberOfCores, CurrentClockSpeed FROM Win32_Processor";

            SafeExecutor.ExecuteSafeQuery(query, "процессоре", rep.Errors, rep.CriticalErrors, searcher =>
            {
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
            });
        }

        private void GetRamInfo(HWReport rep)
        {
            string query = "SELECT Capacity, Speed FROM Win32_PhysicalMemory";
            var speeds = new List<int>();
            double totalBytes = 0;

            SafeExecutor.ExecuteSafeQuery(query, "ОЗУ", rep.Errors, rep.CriticalErrors, searcher =>
            {
                foreach (var item in searcher.Get())
                {
                    using (item)
                    {
                        double? capacity = Parser.ToDouble(item["Capacity"]);
                        if (capacity.HasValue) totalBytes += capacity.Value;
                        else rep.Errors.Add("Не удалось определить емкость одной из планок ОЗУ!");

                        int? speed = Parser.ToInt(item["Speed"]);
                        if (speed.HasValue) speeds.Add(speed.Value);
                        else rep.Errors.Add("Не удалось определить скорость ОЗУ!");
                    }
                }

                if (totalBytes > 0)
                {
                    rep.RAMSize = Math.Round(totalBytes / WMISettings.BytesInGigabyte, 2);
                }

                if (speeds.Count > 0)
                {
                    int minSpeed = speeds.Min(); 
                    rep.RAMSpeed = minSpeed;

                    if (speeds.Any(s => s != minSpeed))
                    {
                        rep.Errors.Add("Установлены модули ОЗУ с разной скоростью. Система ограничена самой медленной планкой.");
                        Log.Warning("Обнаружен конфликт частот RAM: {Speeds}. Выбрана минимальная: {MinSpeed} MHz", string.Join(", ", speeds), minSpeed);
                    }
                }
            });
        }

        private void GetGpuInfo(HWReport rep)
        {
            string query = "SELECT Name FROM Win32_VideoController";

            SafeExecutor.ExecuteSafeQuery(query, "видеокартах", rep.Errors, rep.CriticalErrors, searcher =>
            {
                foreach (var item in searcher.Get())
                {
                    using (item)
                    {
                        string gpu = Parser.ToString(item["Name"]);
                        rep.VideoCards.Add(gpu);
                    }
                }
            });
        }

        private void GetDrivesInfo(HWReport rep)
        {
            string query = "SELECT Model, Size FROM Win32_DiskDrive";

            SafeExecutor.ExecuteSafeQuery(query, "накопителях", rep.Errors, rep.CriticalErrors, searcher =>
            {
                foreach (var item in searcher.Get())
                {
                    using (item)
                    {
                        string model = Parser.ToString(item["Model"]);
                        double? size = Parser.ToDouble(item["Size"]);

                        if (size.HasValue)
                        {
                            double sizeGB = Math.Round(size.Value / WMISettings.BytesInGigabyte, 0);
                            rep.Drives.Add($"{model} ({sizeGB} GB)");
                        }
                        else
                        {
                            rep.Drives.Add($"{model} (-)");
                            rep.Errors.Add($"Не удалось определить емкость накопителя: {model}");
                        }
                    }
                }
            });
        }
    }
}
