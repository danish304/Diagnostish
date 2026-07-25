using Diagnostish.Application.Mappers.Common;
using Diagnostish.Domain.Common;
using Diagnostish.Domain.Interfaces;
using Diagnostish.Domain.Models.Entities.Hardware;
using Diagnostish.Domain.Models.Reports;

namespace Diagnostish.Application.Mappers.HardwareMappers;

public class BiosReportMapper : IReportMapper<HardwareReport, BiosInfo>
{
    public void MapInto(HardwareReport hardwareReport, ProvideResult<BiosInfo> analysisBiosData)
    {
        if (hardwareReport.TryExtractData(analysisBiosData, out var data))
        {
            hardwareReport.BiosVersion = data.Version;
            hardwareReport.BiosReleaseDate = data.ReleaseDate;
            hardwareReport.BiosManufacturer = data.Manufacturer;
        }
    }
}