using Application.Mappers.Common;
using Domain.Models.Entities.Hardware;
using Domain.Models.Reports.Components;

namespace Application.Mappers.Hardware;

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