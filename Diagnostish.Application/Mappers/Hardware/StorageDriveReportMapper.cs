using Diagnostish.Application.Mappers.Common;
using Diagnostish.Domain.Models.Entities.Hardware;
using Diagnostish.Domain.Models.Reports.Components;

namespace Diagnostish.Application.Mappers.Hardware;

public class StorageDriveReportMapper
    : IReportMapper<HardwareReport, IReadOnlyList<StorageDrive>>
{
    public void MapInto(
        HardwareReport report,
        ProvideResult<IReadOnlyList<StorageDrive>> result)
    {
        if (report.TryExtractData(result, out var data))
        {
            report.StorageDrives = [.. data];
        }
    }
}