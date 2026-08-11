using Diagnostish.Application.Mappers.Common;
using Diagnostish.Domain.Models.Entities.Hardware;
using Diagnostish.Domain.Models.Reports.Components;

namespace Diagnostish.Application.Mappers.Hardware;

public class BiosReportMapper
    : IReportMapper<HardwareReport, Bios>
{
    public void MapInto(
        HardwareReport report,
        ProvideResult<Bios> result)
    {
        if (report.TryExtractData(result, out var data))
        {
            report.BiosVersion = data.Version;
            report.BiosReleaseDate = data.ReleaseDate;
            report.BiosManufacturer = data.Manufacturer;
        }
    }
}