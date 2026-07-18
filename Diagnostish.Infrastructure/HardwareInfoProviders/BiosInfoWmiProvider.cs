using Diagnostish.Domain.Interfaces;
using Diagnostish.Domain.Models.Reports;
using Diagnostish.Infrastructure.WmiHelpers;

namespace Diagnostish.Infrastructure.HardwareInfoProviders
{
    public class BiosInfoWmiProvider(IExecutorWmi executor) : IProvideHardwareInfo
    {
        public HardwareReport ProvideInfo(HardwareReport hardwareReport)
        {
            const string VERSION = "Version";
            const string RELEASE = "ReleaseDate";
            const string MANUFACTURER = "Manufacturer";

            string query = $"SELECT {VERSION}, {RELEASE}, {MANUFACTURER} FROM Win32_BIOS";

            executor.ExecuteSafeQuery(query, "BIOS", hardwareReport.Errors, hardwareReport.CriticalErrors, collection =>
            {
                foreach (var item in collection)
                {
                    using (item)
                    {
                        hardwareReport.BiosVersion = Parser.ToSafeString(item[VERSION]);

                        hardwareReport.BiosReleaseDate = Parser.ToSafeDateTime(item[RELEASE]);
                        if (!hardwareReport.BiosReleaseDate.HasValue || (hardwareReport.BiosReleaseDate.Value == DateTime.MinValue))
                        {
                            hardwareReport.Errors.Add("Не удалось определить корректную дату релиза BIOS!");
                        }

                        hardwareReport.BiosManufacturer = Parser.ToSafeString(item[MANUFACTURER]);
                    }
                }
            });

            return hardwareReport;
        }
    }
}