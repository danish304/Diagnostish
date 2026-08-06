using Diagnostish.Application.Mappers.Common;
using Diagnostish.Domain.Models.Entities.Hardware;

namespace Diagnostish.Application.Mappers.HardwareMappers;

public class GpuReportMapper : IReportMapper<HardwareReport, IReadOnlyList<GpuInfo>>
{
    public void MapInto(HardwareReport hardwareReport, ProvideResult<IReadOnlyList<GpuInfo>> analysisGpuData)
    {
        if (hardwareReport.TryExtractData(analysisGpuData, out var data))
        {
            hardwareReport.VideoCards = [..data];
        }
    }
}