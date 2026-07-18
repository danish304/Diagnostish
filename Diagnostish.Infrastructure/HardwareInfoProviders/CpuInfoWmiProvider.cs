using Diagnostish.Domain.Interfaces;
using Diagnostish.Domain.Models.Reports;
using Diagnostish.Infrastructure.WmiHelpers;

namespace Diagnostish.Infrastructure.HardwareInfoProviders
{
    public class CpuInfoWmiProvider(IExecutorWmi executor) : IProvideHardwareInfo
    {
        public HardwareReport ProvideInfo(HardwareReport hardwareReport)
        {
            const string CPUNAME = "Name";
            const string COUNTCORES = "NumberOfCores";
            const string SPEED = "CurrentClockSpeed";

            string query = $"SELECT {CPUNAME}, {COUNTCORES}, {SPEED} FROM Win32_Processor";

            executor.ExecuteSafeQuery(query, "процессоре", hardwareReport.Errors, hardwareReport.CriticalErrors, collection =>
            {
                foreach (var item in collection)
                {
                    using (item)
                    {
                        hardwareReport.CpuName = Parser.ToSafeString(item[CPUNAME]);

                        int? cores = Parser.ToSafeInt(item[COUNTCORES]);
                        if (!cores.HasValue || cores.Value <= 0)
                        {
                            hardwareReport.Errors.Add("Не удалось получить корректное количество ядер процессора!");
                        }
                        else
                        {
                            hardwareReport.CpuCores = cores.Value;
                        }

                        int? clockspeed = Parser.ToSafeInt(item[SPEED]);
                        if (!clockspeed.HasValue || clockspeed.Value <= 0)
                        {
                            hardwareReport.Errors.Add("Не удалось получить корректную частоту процессора!");
                        }
                        else
                        {
                            hardwareReport.CpuClockspeed = clockspeed.Value;
                        }
                    }
                }
            });

            return hardwareReport;
        }
    }
}
