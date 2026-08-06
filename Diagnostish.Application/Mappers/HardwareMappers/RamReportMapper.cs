using Diagnostish.Application.Mappers.Common;
using Diagnostish.Domain.Models.Entities.Hardware;

namespace Diagnostish.Application.Mappers.HardwareMappers;

public class RamReportMapper : IReportMapper<HardwareReport, RamInfo> 
{
    public void MapInto(HardwareReport hardwareReport, ProvideResult<RamInfo> analysisRamData)
    {
        if (hardwareReport.TryExtractData(analysisRamData, out var data))
        {
            hardwareReport.RamType = data.Type;
            hardwareReport.RamCapacity = data.Capacity;
            hardwareReport.RamSpeed = data.Speed;
        }
    }
}