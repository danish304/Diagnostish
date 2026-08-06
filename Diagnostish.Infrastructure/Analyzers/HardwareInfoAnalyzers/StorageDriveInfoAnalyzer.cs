using Diagnostish.Domain.Models.Entities.Hardware;
using Diagnostish.Infrastructure.Analyzers.HardwareInfoAnalyzers.Messages;
using Diagnostish.Infrastructure.Providers.HardwareInfoProviders.RawHardwareInfo;
using Diagnostish.Infrastructure.Shared.Utils;

namespace Diagnostish.Infrastructure.Analyzers.HardwareInfoAnalyzers;

public class StorageDriveInfoAnalyzer(Serilog.ILogger logger) : IAnalyzeDiagnosticInfo<RawStorageDriveInfo, IReadOnlyList<StorageDriveInfo>>
{
    public ProvideResult<IReadOnlyList<StorageDriveInfo>> AnalyzeInfo(ProvideResult<IReadOnlyList<RawStorageDriveInfo>> providedStorageDriveData)
    {
        var warnings = new List<string>(providedStorageDriveData.Warnings);

        if (providedStorageDriveData.Data is not { Count: > 0 } storageDriveInfo)
            return ProvideResult<IReadOnlyList<StorageDriveInfo>>.Fail(warnings, providedStorageDriveData.CriticalErrors);

        var storageDrives = new List<StorageDriveInfo>();

        int unknownModelCount = 0;
        int unknownSizeCount = 0;

        foreach (var item in storageDriveInfo)
        {
            string storageDriveModel = "Неизвестно";
            double storageDriveSize = 0;

            if (item.Model is not null) storageDriveModel = item.Model;
            else unknownModelCount++;

            if (item.Size is { } size && size > 0) storageDriveSize = ByteConverter.ToGigabytes(size);
            else unknownSizeCount++;

            storageDrives.Add(new StorageDriveInfo(storageDriveModel, storageDriveSize));
        }

        if (unknownModelCount > 0)
        {
            warnings.Add($"{StorageDriveAnalyzerMessages.UnknownModel} {CommonMessages.CountOfTotal(unknownModelCount, storageDriveInfo.Count)}");
            logger.Warning("{UnknownModelMessage} Затронуто {Count} из {Total} накопителей.", StorageDriveAnalyzerMessages.UnknownModel, 
                                                                                              unknownModelCount, storageDriveInfo.Count);
        }
        if (unknownSizeCount > 0)
        {
            warnings.Add($"{StorageDriveAnalyzerMessages.UnknownSize} {CommonMessages.CountOfTotal(unknownSizeCount, storageDriveInfo.Count)}");
            logger.Warning("{UnknownSizeMessage} Затронуто {Count} из {Total} накопителей.", StorageDriveAnalyzerMessages.UnknownSize, 
                                                                                             unknownSizeCount, storageDriveInfo.Count);
        }

        return ProvideResult<IReadOnlyList<StorageDriveInfo>>.Ok(storageDrives, warnings);
    }
}