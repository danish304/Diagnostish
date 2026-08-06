using Diagnostish.Application.Mappers.Common;
using Diagnostish.Domain.Models.Entities.Hardware;

namespace Diagnostish.Application.Mappers.HardwareMappers;

public class StorageDriveReportMapper : IReportMapper<HardwareReport, IReadOnlyList<StorageDriveInfo>>
{
    public void MapInto(HardwareReport hardwareReport, ProvideResult<IReadOnlyList<StorageDriveInfo>> analysisStorageDriveData)
    {
        if (hardwareReport.TryExtractData(analysisStorageDriveData, out var data))
        {
            hardwareReport.StorageDrives = [..data];
        }
    }
}