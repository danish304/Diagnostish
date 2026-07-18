using Diagnostish.Domain.Interfaces;
using Diagnostish.Domain.Models.Entities;
using Diagnostish.Domain.Models.Reports;
using Diagnostish.Infrastructure.WmiHelpers;

namespace Diagnostish.Infrastructure.HardwareInfoProviders
{
    public class RamInfoWmiProvider(IExecutorWmi executor, IAnalyzeHardwareInfo<RamInfo, HardwareReport> analyzer) : IProvideHardwareInfo
    {
        public HardwareReport ProvideInfo(HardwareReport hardwareReport)
        {
            var ramInfo = new List<RamInfo>();

            const string TYPE = "SMBIOSMemoryType";
            const string CAPACITY = "Capacity";
            const string SPEED = "Speed";

            string query = $"SELECT {TYPE}, {CAPACITY}, {SPEED} FROM Win32_PhysicalMemory";

            executor.ExecuteSafeQuery(query, "ОЗУ", hardwareReport.Errors, hardwareReport.CriticalErrors, collection =>
            {
                foreach (var item in collection)
                {
                    using (item)
                    {
                        ramInfo.Add(new RamInfo(Parser.ToSafeString(item[TYPE]), Parser.ToSafeDouble(item[CAPACITY]) ?? 0, Parser.ToSafeInt(item[SPEED]) ?? 0));
                    }
                }
            });

            analyzer.AnalyzeInfo(ramInfo, hardwareReport);

            return hardwareReport;
        }
    }
}