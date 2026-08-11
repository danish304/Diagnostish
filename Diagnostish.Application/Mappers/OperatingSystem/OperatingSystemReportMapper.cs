using Diagnostish.Application.Mappers.Common;
using Diagnostish.Domain.Models.Reports.Components;

using OsEntity = Diagnostish.Domain.Models.Entities.OperatingSystem.OperatingSystem;

namespace Diagnostish.Application.Mappers.OperatingSystem;

public class OperatingSystemReportMapper 
    : IReportMapper<OperatingSystemReport, OsEntity>
{
    public void MapInto(
        OperatingSystemReport report, 
        ProvideResult<OsEntity> result)
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