using Application.Mappers.Common;
using Domain.Models.Entities.Hardware;
using Domain.Models.Reports.Components;

namespace Application.Mappers.Hardware;

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