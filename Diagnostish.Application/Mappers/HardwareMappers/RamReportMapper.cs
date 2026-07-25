using Diagnostish.Application.Mappers.Common;
using Diagnostish.Domain.Common;
using Diagnostish.Domain.Interfaces;
using Diagnostish.Domain.Models.Entities.Hardware;
using Diagnostish.Domain.Models.Reports;

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