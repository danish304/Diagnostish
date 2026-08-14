using Application.Mappers.Common;
using Domain.Models.Entities.OperatingSystem;
using Domain.Models.Reports.Components;

namespace Application.Mappers.OperatingSystem;

public class OperatingSystemReportMapper
    : IReportMapper<OperatingSystemReport, OperSystem>
{
    public void MapInto(
        OperatingSystemReport report,
        ProvideResult<OperSystem> result)
    {
        if (report.TryExtractData(result, out var data))
        {
            report.OperatingSystemName = data.Caption;
            report.OperatingSystemManufacturer = data.Manufacturer;
            report.OperatingSystemVersion = data.Version;
            report.OperatingSystemInstallDate = data.InstallDate;
            report.OperatingSystemRegisteredUser = data.User;
            report.OperatingSystemLastBootUpTime = data.LastBoot;
        }
    }
}