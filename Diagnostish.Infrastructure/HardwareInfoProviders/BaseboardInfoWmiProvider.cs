using Diagnostish.Domain.Interfaces;
using Diagnostish.Domain.Models.Reports;
using Diagnostish.Infrastructure.WmiHelpers;

namespace Diagnostish.Infrastructure.HardwareInfoProviders
{
    public class BaseboardInfoWmiProvider(IExecutorWmi executor) : IProvideHardwareInfo
    {
        public HardwareReport ProvideInfo(HardwareReport hardwareReport)
        {
            const string MODEL = "Product";
            const string MANUFACTURER = "Manufacturer";
            const string VERSION = "Version";
            const string STATUS = "Status";

            string query = $"SELECT {MODEL}, {MANUFACTURER}, {VERSION}, {STATUS} FROM Win32_BaseBoard";

            executor.ExecuteSafeQuery(query, "основной плате", hardwareReport.Errors, hardwareReport.CriticalErrors, collection =>
            {
                foreach (var item in collection)
                {
                    using (item)
                    {
                        hardwareReport.BaseboardModel = Parser.ToSafeString(item[MODEL]);
                        hardwareReport.BaseboardManufacturer = Parser.ToSafeString(item[MANUFACTURER]);
                        hardwareReport.BaseboardVersion = Parser.ToSafeString(item[VERSION]);
                        hardwareReport.BaseboardStatus = Parser.ToSafeString(item[STATUS]);
                    }
                }
            });

            return hardwareReport;
        }
    }
}