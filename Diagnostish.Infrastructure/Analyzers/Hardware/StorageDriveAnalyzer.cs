using Diagnostish.Domain.Models.Entities.Hardware;
using Diagnostish.Infrastructure.Providers.Common.RawModels.Hardware;
using Diagnostish.Infrastructure.Shared.Common.Utils;
using Serilog;

using static Diagnostish.Infrastructure.Analyzers.Hardware.Messages.StorageDriveAnalyzerMessages;
using static Diagnostish.Infrastructure.Analyzers.Hardware.Messages.CommonMessages;

namespace Diagnostish.Infrastructure.Analyzers.Hardware;

public class StorageDriveAnalyzer(ILogger logger) 
    : IAnalyzer<RawStorageDriveModel, IReadOnlyList<StorageDrive>>
{
    public ProvideResult<IReadOnlyList<StorageDrive>> Analyze(
        ProvideResult<IReadOnlyList<RawStorageDriveModel>> result)
    {
        var warnings = new List<string>(result.Warnings);

        if (result.Data is not { Count: > 0 } rawData)
        {
            return ProvideResult<IReadOnlyList<StorageDrive>>.Fail(
                warnings, 
                result.CriticalErrors);
        }

        var storageDrives = new List<StorageDrive>();
        int unknownModelCount = 0;
        int unknownSizeCount = 0;

        foreach (var model in rawData)
        {
            string storageDriveModel = model.Model ?? "Неизвестно";
            double storageDriveSize = 0;

            if (model.Model is null)
            {
                unknownModelCount++;
            }

            if (model.Size is { } size && size > 0)
            {
                storageDriveSize = ByteConverter.ToGigabytes(size);
            }
            else 
            { 
                unknownSizeCount++; 
            }

            storageDrives.Add(new StorageDrive(storageDriveModel, storageDriveSize));
        }

        if (unknownModelCount > 0)
        {
            warnings.Add($"{UNKNOWN_MODEL} {CountOfTotal(unknownModelCount, rawData.Count)}");

            logger.Warning(
                UNKNOWN_MODEL + " Затронуто {Count} из {Total} накопителей.", 
                unknownModelCount, rawData.Count);
        }
        if (unknownSizeCount > 0)
        {
            warnings.Add($"{UNKNOWN_SIZE} {CountOfTotal(unknownSizeCount, rawData.Count)}");

            logger.Warning(
                UNKNOWN_SIZE + " Затронуто {Count} из {Total} накопителей.", 
                unknownSizeCount, rawData.Count);
        }

        return ProvideResult<IReadOnlyList<StorageDrive>>.Ok(storageDrives, warnings);
    }
}