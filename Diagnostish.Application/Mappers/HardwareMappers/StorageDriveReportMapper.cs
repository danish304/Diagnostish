using Diagnostish.Application.Mappers.Common;
using Diagnostish.Domain.Common;
using Diagnostish.Domain.Interfaces;
using Diagnostish.Domain.Models.Entities.Hardware;
using Diagnostish.Domain.Models.Reports;

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