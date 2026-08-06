using Diagnostish.Application.Mappers.Common;
using Diagnostish.Domain.Models.Entities.Hardware;

namespace Diagnostish.Application.Mappers.HardwareMappers;

public class CpuReportMapper : IReportMapper<HardwareReport, CpuInfo>
{
    public void MapInto(HardwareReport hardwareReport, ProvideResult<CpuInfo> analysisCpuData)
    {
        if (hardwareReport.TryExtractData(analysisCpuData, out var data))
        {
            hardwareReport.CpuName = data.Name;
            hardwareReport.CpuCores = data.Cores;
            hardwareReport.CpuClockSpeed = data.ClockSpeed;
        }
    }
}