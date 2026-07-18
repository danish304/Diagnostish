using Diagnostish.Domain.Interfaces;
using Diagnostish.Domain.Models.Reports;
using Diagnostish.Infrastructure.WmiHelpers;

namespace Diagnostish.Infrastructure.HardwareInfoProviders
{
    public class GpuInfoWmiProvider(IExecutorWmi executor) : IProvideHardwareInfo
    {
        public HardwareReport ProvideInfo(HardwareReport hardwareReport)
        {
            const string GPUNAME = "Name";
            const string GPURAM = "AdapterRAM";

            string query = $"SELECT {GPUNAME}, {GPURAM} FROM Win32_VideoController";

            executor.ExecuteSafeQuery(query, "видеокартах", hardwareReport.Errors, hardwareReport.CriticalErrors, collection =>
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
                            sizeGB = Math.Round(size.Value / (1024 * 1024 * 1024), 0);
                        }
                        else
                        {
                            hardwareReport.Errors.Add($"Не удалось определить объем видеопамяти у: {gpu}");
                        }

                        hardwareReport.Gpus.Add(new HardwareReport.ComponentInfo(gpu, sizeGB));
                    }
                }
            });

            return hardwareReport;
        }
    }
}