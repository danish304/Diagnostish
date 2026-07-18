using Diagnostish.Domain.Interfaces;
using Diagnostish.Domain.Models.Reports;
using Diagnostish.Infrastructure.WmiHelpers;

namespace Diagnostish.Infrastructure.OperatingSystemInfoProviders
{
    public class OperatingSystemInfoWmiProvider(IExecutorWmi executor) : IProvideOperatingSystemInfo
    {
        public OperatingSystemReport ProvideInfo(OperatingSystemReport operatingSystemReport)
        {
            const string CAPTION = "Caption";
            const string VERSION = "Version";
            const string MANUFACTURER = "Manufacturer";
            const string USER = "RegisteredUser";
            const string INSTALL = "InstallDate";
            const string LASTBOOT = "LastBootUpTime";

            string query = $"SELECT {CAPTION}, {VERSION}, {MANUFACTURER}, {USER}, {INSTALL}, {LASTBOOT} FROM Win32_OperatingSystem";

            executor.ExecuteSafeQuery(query, "данных ОС", operatingSystemReport.Errors, operatingSystemReport.CriticalErrors, collection =>
            {
                foreach (var item in collection)
                {
                    using (item)
                    {
                        operatingSystemReport.OperatingSystemName = Parser.ToSafeString(item[CAPTION]);
                        operatingSystemReport.OperatingSystemVersion = Parser.ToSafeString(item[VERSION]);
                        operatingSystemReport.OperatingSystemManufacturer = Parser.ToSafeString(item[MANUFACTURER]);
                        operatingSystemReport.OperatingSystemRegisteredUser = Parser.ToSafeString(item[USER]);

                        operatingSystemReport.OperatingSystemInstallDate = Parser.ToSafeDateTime(item[INSTALL]);
                        if (!operatingSystemReport.OperatingSystemInstallDate.HasValue || (operatingSystemReport.OperatingSystemInstallDate.Value == DateTime.MinValue))
                        {
                            operatingSystemReport.Errors.Add("Не удалось определить корректную дату установки ОС!");
                        }

                        operatingSystemReport.OperatingSystemLastBootUpTime = Parser.ToSafeDateTime(item[LASTBOOT]);
                        if (!operatingSystemReport.OperatingSystemLastBootUpTime.HasValue || (operatingSystemReport.OperatingSystemLastBootUpTime.Value == DateTime.MinValue))
                        {
                            operatingSystemReport.Errors.Add("Не удалось определить корректную дату последнего запуска ОС!");
                        }
                    }
                }
            });

            return operatingSystemReport;
        }
    }
}
