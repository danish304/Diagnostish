using Diagnostish.Application.Mappers.Common;
using Diagnostish.Domain.Common;
using Diagnostish.Domain.Interfaces;
using Diagnostish.Domain.Models.Entities.Hardware;
using Diagnostish.Domain.Models.Reports;

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