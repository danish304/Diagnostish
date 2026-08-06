using Diagnostish.Application.Mappers.Common;
using Diagnostish.Domain.Models.Entities;

namespace Diagnostish.Application.Mappers.OperatingSystemMappers;

public class OperatingSystemReportMapper : IReportMapper<OperatingSystemReport, OperatingSystemInfo>
{
    public void MapInto(OperatingSystemReport operatingSystemReport, ProvideResult<OperatingSystemInfo> analysisOperatingSystemData)
    {
        if (operatingSystemReport.TryExtractData(analysisOperatingSystemData, out var data))
        {
            operatingSystemReport.OperatingSystemName = data.Caption;
            operatingSystemReport.OperatingSystemManufacturer = data.Manufacturer;
            operatingSystemReport.OperatingSystemVersion = data.Version;
            operatingSystemReport.OperatingSystemInstallDate = data.InstallDate;
            operatingSystemReport.OperatingSystemRegisteredUser = data.User;
            operatingSystemReport.OperatingSystemLastBootUpTime = data.LastBoot;
        }
    }
}