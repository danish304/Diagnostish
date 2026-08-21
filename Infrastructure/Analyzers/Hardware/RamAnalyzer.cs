using Domain.Models.Entities.Hardware;
using Infrastructure.Providers.Common.RawModels.Hardware;
using Infrastructure.Shared.Common.Utils;
using Serilog;

using static Infrastructure.Analyzers.Common.CommonMessages;
using static Infrastructure.Analyzers.Hardware.Messages.RamAnalyzerMessages;

namespace Infrastructure.Analyzers.Hardware;

public class RamAnalyzer(ILogger logger)
    : IAnalyzer<RamRawModel, Ram>
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
        ProvideResult<IReadOnlyList<RamRawModel>> result)
    {
        var warnings = new List<string>(result.Warnings);

        if (result.Data is not { Count: > 0 } rawData)
        {
            return ProvideResult<Ram>.Fail(
                warnings,
                result.CriticalErrors);
        }

        var (
            types, capacities, speeds,
            unknownTypeCount, unknownCapacityCount, unknownSpeedCount) = CollectFields(result.Data);

        AppendCountWarnings(
            warnings,
            logger,
            unknownTypeCount,
            unknownCapacityCount,
            unknownSpeedCount,
            rawData.Count);

        string type = ResolveType(types, warnings, logger);
        double capacity = ByteConverter.ToGigabytes(capacities.Sum());
        int speed = ResolveSpeed(speeds, warnings, logger);

        return ProvideResult<Ram>.Ok(
            new Ram(type, capacity, speed),
            warnings);
    }

    private static (
        List<string> Types, List<double> Capacities, List<int> Speeds,
        int UnknownTypeCount, int UnknownCapacityCount, int UnknownSpeedCount)
        CollectFields(IReadOnlyList<RamRawModel> rawData)
    {
        var types = new List<string>();
        var capacities = new List<double>();
        var speeds = new List<int>();

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
                capacities.Add(capacity);
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

        return (
            types, capacities, speeds,
            unknownTypeCount, unknownCapacityCount, unknownSpeedCount);
    }

    private static void AppendCountWarnings(
        List<string> warnings,
        ILogger logger,
        int unknownTypeCount,
        int unknownCapacityCount,
        int unknownSpeedCount,
        int total)
    {
        if (unknownTypeCount > 0)
        {
            warnings.Add($"{UNKNOWN_TYPE} {CountOfTotal(unknownTypeCount, total)}");

            logger.Warning(
                UNKNOWN_TYPE + " Затронуто {Count} из {Total} модулей.",
                unknownTypeCount, total);
        }
        if (unknownCapacityCount > 0)
        {
            warnings.Add($"{UNKNOWN_CAPACITY} {CountOfTotal(unknownCapacityCount, total)}");

            logger.Warning(
                UNKNOWN_CAPACITY + " Затронуто {Count} из {Total} модулей.",
                unknownCapacityCount, total);
        }
        if (unknownSpeedCount > 0)
        {
            warnings.Add($"{UNKNOWN_SPEED} {CountOfTotal(unknownSpeedCount, total)}");

            logger.Warning(
                UNKNOWN_SPEED + " Затронуто {Count} из {Total} модулей.",
                unknownSpeedCount, total);
        }
    }

    private static string ResolveType(List<string> types, List<string> warnings, ILogger logger)
    {
        string type = "Неизвестно";

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

        return type;
    }

    private static int ResolveSpeed(List<int> speeds, List<string> warnings, ILogger logger)
    {
        int speed = 0;

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

        return speed;
    }
}