using Application.Mappers.Common;
using Domain.Models.Entities.Hardware;
using Domain.Models.Reports.Components;

namespace Application.Mappers.Hardware;

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