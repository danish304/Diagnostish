using Diagnostish.Application.Mappers.Common;
using Diagnostish.Domain.Models.Entities.Hardware;

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