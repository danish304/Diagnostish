using Application.Mappers.Common;
using Domain.Models.Entities.Hardware;
using Domain.Models.Reports.Components;

namespace Application.Mappers.Hardware;

public class GpuReportMapper
    : IReportMapper<HardwareReport, IReadOnlyList<Gpu>>
{
    public void MapInto(
        HardwareReport report,
        ProvideResult<IReadOnlyList<Gpu>> result)
    {
        if (report.TryExtractData(result, out var data))
        {
            report.VideoCards = [.. data];
        }
    }
}