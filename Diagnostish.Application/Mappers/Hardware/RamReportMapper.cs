using Diagnostish.Application.Mappers.Common;
using Diagnostish.Domain.Models.Entities.Hardware;
using Diagnostish.Domain.Models.Reports.Components;

namespace Diagnostish.Application.Mappers.Hardware;

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