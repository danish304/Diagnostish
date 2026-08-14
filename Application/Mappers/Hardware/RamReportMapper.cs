using Application.Mappers.Common;
using Domain.Models.Entities.Hardware;
using Domain.Models.Reports.Components;

namespace Application.Mappers.Hardware;

public class RamReportMapper
    : IReportMapper<HardwareReport, Ram>
{
    public void MapInto(
        HardwareReport report,
        ProvideResult<Ram> result)
    {
        if (report.TryExtractData(result, out var data))
        {
            report.RamType = data.Type;
            report.RamCapacity = data.Capacity;
            report.RamSpeed = data.Speed;
        }
    }
}