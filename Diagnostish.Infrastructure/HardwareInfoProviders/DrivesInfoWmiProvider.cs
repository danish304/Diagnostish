using Diagnostish.Domain.Interfaces;
using Diagnostish.Domain.Models.Reports;
using Diagnostish.Infrastructure.WmiHelpers;

namespace Diagnostish.Infrastructure.HardwareInfoProviders
{
    public class DrivesInfoWmiProvider(IExecutorWmi executor) : IProvideHardwareInfo
    {
        public HardwareReport ProvideInfo(HardwareReport hardwareReport)
        {
            const string MODEL = "Model";
            const string SIZE = "Size";

            string query = $"SELECT {MODEL}, {SIZE} FROM Win32_DiskDrive";

            executor.ExecuteSafeQuery(query, "накопителях", hardwareReport.Errors, hardwareReport.CriticalErrors, collection =>
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
                            sizeGB = Math.Round(size.Value / (1024 * 1024 * 1024), 0);
                        }
                        else
                        {
                            hardwareReport.Errors.Add($"Не удалось определить емкость накопителя: {model}");
                        }

                        hardwareReport.Drives.Add(new HardwareReport.ComponentInfo(model, sizeGB));
                    }
                }
            });

            return hardwareReport;
        }
    }
}