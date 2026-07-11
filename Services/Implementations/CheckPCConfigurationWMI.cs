using Diagnostish.Models;
using Diagnostish.Helpers;
using Diagnostish.Services.Interfaces;
using Serilog;

namespace Diagnostish.Services.Implementations
{
    public class CheckPCConfigurationWMI : IHWCheck
    {
        private const double BytesInGigabyte = 1024 * 1024 * 1024;

        public HWReport CheckPCCFG()
        {
            var rep = new HWReport();

            GetCpuInfo(rep);
            GetRamInfo(rep);
            GetGpuInfo(rep);
            GetDrivesInfo(rep);
            GetBaseBoardInfo(rep);
            GetBIOSInfo(rep);

            return rep;
        }

        private static void GetCpuInfo(HWReport rep)
        {
            string query = "SELECT Name, NumberOfCores, CurrentClockSpeed FROM Win32_Processor";

            SafeExecutor.ExecuteSafeQuery(query, "процессоре", rep.Errors, rep.CriticalErrors, collection =>
            {
                foreach (var item in collection)
                {
                    using (item)
                    {
                        rep.ProcessorName = Parser.ToSafeString(item["Name"]);

                        int? cores = Parser.ToSafeInt(item["NumberOfCores"]);
                        if (!cores.HasValue || cores.Value <= 0)
                        {
                            rep.Errors.Add("Не удалось получить корректное количество ядер процессора!");
                        }
                        else
                        {
                            rep.CoresCount = cores.Value;
                        }

                        int? clockspeed = Parser.ToSafeInt(item["CurrentClockSpeed"]);
                        if (!clockspeed.HasValue || clockspeed.Value <= 0)
                        {
                            rep.Errors.Add("Не удалось получить корректную частоту процессора!");
                        }
                        else
                        {
                            rep.CurrentClockSpeed = clockspeed.Value;
                        }
                    }
                }
            });
        }

        private static void GetRamInfo(HWReport rep)
        {
            string query = "SELECT SMBIOSMemoryType, Capacity, Speed FROM Win32_PhysicalMemory";
            var speeds = new List<int>();
            double totalBytes = 0;

            var RAMTypes = new Dictionary<string, string>
            {
                { "20", "DDR" },
                { "21", "DDR2" },
                { "24", "DDR3" },
                { "26", "DDR4" },
                { "34", "DDR5" }
             };

            string detectedType = "Unknown";

            SafeExecutor.ExecuteSafeQuery(query, "ОЗУ", rep.Errors, rep.CriticalErrors, collection =>
            {
                foreach (var item in collection)
                {
                    using (item)
                    {
                        string rawType = Parser.ToSafeString(item["SMBIOSMemoryType"]);
                        if (RAMTypes.TryGetValue(rawType, out var humanReadableType))
                        {
                            detectedType = humanReadableType; 
                        }
                        rep.RAMType = detectedType;

                        double? capacity = Parser.ToSafeDouble(item["Capacity"]);
                        if (capacity.HasValue && capacity.Value > 0)
                        {
                            totalBytes += capacity.Value;
                        }
                        else
                        {
                            rep.Errors.Add("Не удалось определить корректную емкость одной из планок ОЗУ!");
                        }

                        int? speed = Parser.ToSafeInt(item["Speed"]);
                        if (speed.HasValue && speed.Value > 0)
                        {
                            speeds.Add(speed.Value);
                        }
                        else
                        {
                            rep.Errors.Add("Не удалось определить скорость ОЗУ!");
                        }
                    }
                }

                if (totalBytes > 0)
                {
                    rep.RAMSize = Math.Round(totalBytes / BytesInGigabyte, 2);
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

        private static void GetGpuInfo(HWReport rep)
        {
            string query = "SELECT Name, AdapterRAM FROM Win32_VideoController";

            SafeExecutor.ExecuteSafeQuery(query, "видеокартах", rep.Errors, rep.CriticalErrors, collection =>
            {
                foreach (var item in collection)
                {
                    using (item)
                    {
                        string gpu = Parser.ToSafeString(item["Name"]);
                        rep.VideoCards.Add(gpu);

                        double? size = Parser.ToSafeDouble(item["AdapterRAM"]);
                        if (size.HasValue && size.Value > 0)
                        {
                            rep.AdaptersRAM.Add(Math.Round(size.Value / BytesInGigabyte, 0));
                        }
                        else
                        {
                            rep.AdaptersRAM.Add(0);
                            rep.Errors.Add($"Не удалось определить объем видеопамяти у: {gpu}");
                        }
                    }
                }
            });
        }

        private static void GetDrivesInfo(HWReport rep)
        {
            string query = "SELECT Model, Size FROM Win32_DiskDrive";

            SafeExecutor.ExecuteSafeQuery(query, "накопителях", rep.Errors, rep.CriticalErrors, collection =>
            {
                foreach (var item in collection)
                {
                    using (item)
                    {
                        string model = Parser.ToSafeString(item["Model"]);
                        rep.ModelsDrives.Add(model);

                        double? size = Parser.ToSafeDouble(item["Size"]);
                        if (size.HasValue && size.Value > 0)
                        {
                            rep.DrivesSize.Add(Math.Round(size.Value / BytesInGigabyte, 0));
                        }
                        else
                        {
                            rep.DrivesSize.Add(0);
                            rep.Errors.Add($"Не удалось определить емкость накопителя: {model}");
                        }
                    }
                }
            });
        }

        private static void GetBaseBoardInfo(HWReport rep)
        {
            string query = "SELECT Product, Manufacturer, Version, Status FROM Win32_BaseBoard";

            SafeExecutor.ExecuteSafeQuery(query, "основной плате", rep.Errors, rep.CriticalErrors, collection =>
            {
                foreach (var item in collection)
                {
                    using (item)
                    {
                        rep.BaseBoardModel = Parser.ToSafeString(item["Product"]);
                        rep.BaseBoardManufacturer = Parser.ToSafeString(item["Manufacturer"]);
                        rep.BaseBoardVersion = Parser.ToSafeString(item["Version"]);
                        rep.BaseBoardStatus = Parser.ToSafeString(item["Status"]);
                    }
                }
            });
        }

        private static void GetBIOSInfo(HWReport rep)
        {
            string query = "SELECT Version, ReleaseDate, Manufacturer FROM Win32_BIOS";

            SafeExecutor.ExecuteSafeQuery(query, "BIOS", rep.Errors, rep.CriticalErrors, collection =>
            {
                foreach(var item in collection)
                {
                    using (item)
                    {
                        rep.BIOSVersion = Parser.ToSafeString(item["Version"]);

                        rep.BIOSReleaseDate = Parser.ToSafeDateTime(item["ReleaseDate"]);
                        if (!rep.BIOSReleaseDate.HasValue || (rep.BIOSReleaseDate.Value == DateTime.MinValue))
                        {
                            rep.Errors.Add("Не удалось определить корректную дату релиза BIOS!");
                        }

                        rep.BIOSManufacturer = Parser.ToSafeString(item["Manufacturer"]);
                    }
                }
            });
        }
    }
}
