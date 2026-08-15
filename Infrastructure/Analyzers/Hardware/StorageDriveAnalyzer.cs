using Domain.Models.Entities.Hardware;
using Infrastructure.Providers.Common.RawModels.Hardware;
using Infrastructure.Shared.Common.Utils;
using Serilog;

using static Infrastructure.Analyzers.Common.CommonMessages;
using static Infrastructure.Analyzers.Hardware.Messages.StorageDriveAnalyzerMessages;

namespace Infrastructure.Analyzers.Hardware;

public class StorageDriveAnalyzer(ILogger logger)
    : IAnalyzer<StorageDriveRawModel, IReadOnlyList<StorageDrive>>
{
    public ProvideResult<IReadOnlyList<StorageDrive>> Analyze(
        ProvideResult<IReadOnlyList<StorageDriveRawModel>> result)
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
        int unknownStatusCount = 0;
        int badStatusCount = 0;

        foreach (var model in rawData)
        {
            string storageDriveModel = model.Model ?? "Неизвестно";
            double storageDriveSize = 0;
            string storageDriveStatus = model.Status ?? "Неизвестно";

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

            if (model.Status is null)
            {
                unknownStatusCount++;
            }
            else if (model.Status is not null && storageDriveStatus != "OK")
            {
                badStatusCount++;
            }

            storageDrives.Add(new StorageDrive(
                storageDriveModel,
                storageDriveSize,
                storageDriveStatus));
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
        if (unknownStatusCount > 0)
        {
            warnings.Add($"{UNKNOWN_STATUS} {CountOfTotal(unknownStatusCount, rawData.Count)}");

            logger.Warning(
                UNKNOWN_STATUS + " Затронуто {Count} из {Total} накопителей.",
                unknownStatusCount, rawData.Count);
        }
        if (badStatusCount > 0)
        {
            warnings.Add($"{BAD_STATUS} {CountOfTotal(badStatusCount, rawData.Count)}");

            logger.Warning(
                BAD_STATUS + " Затронуто {Count} из {Total} накопителей.",
                badStatusCount, rawData.Count);
        }

        return ProvideResult<IReadOnlyList<StorageDrive>>.Ok(storageDrives, warnings);
    }
}