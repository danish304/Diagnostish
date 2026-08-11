using Diagnostish.Domain.Models.Entities.Hardware;
using Diagnostish.Infrastructure.Providers.Common.RawModels.Hardware;
using Diagnostish.Infrastructure.Shared.Common.Utils;
using Serilog;

using static Diagnostish.Infrastructure.Analyzers.Hardware.Messages.CommonMessages;
using static Diagnostish.Infrastructure.Analyzers.Hardware.Messages.RamAnalyzerMessages;

namespace Diagnostish.Infrastructure.Analyzers.Hardware;

public class RamAnalyzer(ILogger logger)
    : IAnalyzer<RawRamModel, Ram>
{
    private static readonly Dictionary<string, string> RamTypes = new()
    {
        { "20", "DDR" },
        { "21", "DDR2" },
        { "24", "DDR3" },
        { "26", "DDR4" },
        { "34", "DDR5" }
    };

    public ProvideResult<Ram> Analyze(
        ProvideResult<IReadOnlyList<RawRamModel>> result)
    {
        var warnings = new List<string>(result.Warnings);

        if (result.Data is not { Count: > 0 } rawData)
        {
            return ProvideResult<Ram>.Fail(
                warnings,
                result.CriticalErrors);
        }

        var types = new List<string>();
        string type = "Неизвестно";

        double totalCapacityInBytes = 0;
        double totalCapacityInGB = 0;

        var speeds = new List<int>();
        int speed = 0;

        int unknownTypeCount = 0;
        int unknownCapacityCount = 0;
        int unknownSpeedCount = 0;

        foreach (var model in rawData)
        {
            if (model.Type is not null && RamTypes.TryGetValue(model.Type, out var humanReadableType))
            {
                types.Add(humanReadableType);
            }
            else
            {
                unknownTypeCount++;
            }

            if (model.Capacity is { } capacity && capacity > 0)
            {
                totalCapacityInBytes += capacity;
            }
            else
            {
                unknownCapacityCount++;
            }

            if (model.Speed is { } spd && spd > 0)
            {
                speeds.Add(spd);
            }
            else
            {
                unknownSpeedCount++;
            }
        }

        if (unknownTypeCount > 0)
        {
            warnings.Add($"{UNKNOWN_TYPE} {CountOfTotal(unknownTypeCount, rawData.Count)}");

            logger.Warning(
                UNKNOWN_TYPE + " Затронуто {Count} из {Total} модулей.",
                unknownTypeCount, rawData.Count);
        }
        if (unknownCapacityCount > 0)
        {
            warnings.Add($"{UNKNOWN_CAPACITY} {CountOfTotal(unknownCapacityCount, rawData.Count)}");

            logger.Warning(
                UNKNOWN_CAPACITY + " Затронуто {Count} из {Total} модулей.",
                unknownCapacityCount, rawData.Count);
        }
        if (unknownSpeedCount > 0)
        {
            warnings.Add($"{UNKNOWN_SPEED} {CountOfTotal(unknownSpeedCount, rawData.Count)}");

            logger.Warning(
                UNKNOWN_SPEED + " Затронуто {Count} из {Total} модулей.",
                unknownSpeedCount, rawData.Count);
        }

        if (types.Count > 0)
        {
            type = types[0];

            if (types.Any(s => s != type))
            {
                type = "Неизвестно";
                warnings.Add(TYPE_CONFLICT);

                logger.Warning(
                    TYPE_CONFLICT + " ({Types})",
                    string.Join(", ", types));
            }
        }

        if (totalCapacityInBytes > 0)
        {
            totalCapacityInGB = ByteConverter.ToGigabytes(totalCapacityInBytes, 2);
        }

        if (speeds.Count > 0)
        {
            speed = speeds.Min();

            if (speeds.Any(s => s != speed))
            {
                warnings.Add(SPEED_CONFLICT);

                logger.Warning(
                    SPEED_CONFLICT + " ({Speeds}) Выбрана минимальная: {Min} MHz",
                    string.Join(", ", speeds), speed);
            }
        }

        return ProvideResult<Ram>.Ok(
            new Ram(type, totalCapacityInGB, speed),
            warnings);
    }
}