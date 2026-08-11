using Diagnostish.Application.Mappers.Common;
using Diagnostish.Domain.Models.Entities.Hardware;
using Diagnostish.Domain.Models.Reports.Components;

namespace Diagnostish.Application.Mappers.Hardware;

public class CpuReportMapper 
    : IReportMapper<HardwareReport, Cpu>
{
    public void MapInto(
        HardwareReport report, 
        ProvideResult<Cpu> result)
    {
        if (report.TryExtractData(result, out var data))
        {
            report.CpuName = data.Name;
            report.CpuCores = data.Cores;
            report.CpuClockSpeed = data.ClockSpeed;
        }
    }
}