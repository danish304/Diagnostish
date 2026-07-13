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
            const string CPUNAME = "Name";
            const string COUNTCORES = "NumberOfCores";
            const string SPEED = "CurrentClockSpeed";

            string query = "SELECT" + CPUNAME + COUNTCORES + SPEED + "FROM Win32_Processor";

            SafeExecutor.ExecuteSafeQuery(query, "процессоре", rep.Errors, rep.CriticalErrors, collection =>
            {
                foreach (var item in collection)
                {
                    using (item)
                    {
                        rep.ProcessorName = Parser.ToSafeString(item[CPUNAME]);

                        int? cores = Parser.ToSafeInt(item[COUNTCORES]);
                        if (!cores.HasValue || cores.Value <= 0)
                        {
                            rep.Errors.Add("Не удалось получить корректное количество ядер процессора!");
                        }
                        else
                        {
                            rep.CoresCount = cores.Value;
                        }

                        int? clockspeed = Parser.ToSafeInt(item[SPEED]);
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
            const string TYPE = "SMBIOSMemoryType";
            const string CAPACITY = "Capacity";
            const string SPEED = "Speed";

            string query = "SELECT" + TYPE + CAPACITY + SPEED + "FROM Win32_PhysicalMemory";

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
                        string rawType = Parser.ToSafeString(item[TYPE]);
                        if (RAMTypes.TryGetValue(rawType, out var humanReadableType))
                        {
                            detectedType = humanReadableType; 
                        }
                        rep.RAMType = detectedType;

                        double? capacity = Parser.ToSafeDouble(item[CAPACITY]);
                        if (capacity.HasValue && capacity.Value > 0)
                        {
                            totalBytes += capacity.Value;
                        }
                        else
                        {
                            rep.Errors.Add("Не удалось определить корректную емкость одной из планок ОЗУ!");
                        }

                        int? speed = Parser.ToSafeInt(item[SPEED]);
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
            const string GPUNAME = "Name";
            const string GPURAM = "AdapterRAM";

            string query = "SELECT" + GPUNAME + GPURAM + "FROM Win32_VideoController";

            SafeExecutor.ExecuteSafeQuery(query, "видеокартах", rep.Errors, rep.CriticalErrors, collection =>
            {
                foreach (var item in collection)
                {
                    using (item)
                    {
                        string gpu = Parser.ToSafeString(item[GPUNAME]);
                        double sizeGB = 0;

                        double? size = Parser.ToSafeDouble(item[GPURAM]);
                        if (size.HasValue && size.Value > 0)
                        {
                            sizeGB = Math.Round(size.Value / BytesInGigabyte, 0);
                        }
                        else
                        {
                            rep.Errors.Add($"Не удалось определить объем видеопамяти у: {gpu}");
                        }

                        rep.VideoCards.Add(new HWReport.ComponentInfo(gpu, sizeGB));
                    }
                }
            });
        }

        private static void GetDrivesInfo(HWReport rep)
        {
            const string MODEL = "Model";
            const string SIZE = "Size";

            string query = "SELECT" + MODEL + SIZE + "FROM Win32_DiskDrive";

            SafeExecutor.ExecuteSafeQuery(query, "накопителях", rep.Errors, rep.CriticalErrors, collection =>
            {
                foreach (var item in collection)
                {
                    using (item)
                    {
                        string model = Parser.ToSafeString(item[MODEL]);
                        double sizeGB = 0;

                        double? size = Parser.ToSafeDouble(item[SIZE]);
                        if (size.HasValue && size.Value > 0)
                        {
                            sizeGB = Math.Round(size.Value / BytesInGigabyte, 0);
                        }
                        else
                        {
                            rep.Errors.Add($"Не удалось определить емкость накопителя: {model}");
                        }

                        rep.Drives.Add(new HWReport.ComponentInfo(model, sizeGB));
                    }
                }
            });
        }

        private static void GetBaseBoardInfo(HWReport rep)
        {
            const string MODEL = "Product";
            const string MANUFACTURER = "Manufacturer";
            const string VERSION = "VersionVersion";
            const string STATUS = "Status";

            string query = "SELECT" + MODEL + MANUFACTURER + VERSION + STATUS + "FROM Win32_BaseBoard";

            SafeExecutor.ExecuteSafeQuery(query, "основной плате", rep.Errors, rep.CriticalErrors, collection =>
            {
                foreach (var item in collection)
                {
                    using (item)
                    {
                        rep.BaseBoardModel = Parser.ToSafeString(item[MODEL]);
                        rep.BaseBoardManufacturer = Parser.ToSafeString(item[MANUFACTURER]);
                        rep.BaseBoardVersion = Parser.ToSafeString(item[VERSION]);
                        rep.BaseBoardStatus = Parser.ToSafeString(item[STATUS]);
                    }
                }
            });
        }

        private static void GetBIOSInfo(HWReport rep)
        {
            const string VERSION = "Version";
            const string RELEASE = "ReleaseDate";
            const string MANUFACTURER = "Manufacturer";

            string query = "SELECT" + VERSION + RELEASE + MANUFACTURER + "FROM Win32_BIOS";

            SafeExecutor.ExecuteSafeQuery(query, "BIOS", rep.Errors, rep.CriticalErrors, collection =>
            {
                foreach(var item in collection)
                {
                    using (item)
                    {
                        rep.BIOSVersion = Parser.ToSafeString(item[VERSION]);

                        rep.BIOSReleaseDate = Parser.ToSafeDateTime(item[RELEASE]);
                        if (!rep.BIOSReleaseDate.HasValue || (rep.BIOSReleaseDate.Value == DateTime.MinValue))
                        {
                            rep.Errors.Add("Не удалось определить корректную дату релиза BIOS!");
                        }

                        rep.BIOSManufacturer = Parser.ToSafeString(item[MANUFACTURER]);
                    }
                }
            });
        }
    }
}
