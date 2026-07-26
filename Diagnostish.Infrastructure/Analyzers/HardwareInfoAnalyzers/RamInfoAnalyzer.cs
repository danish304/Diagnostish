using Diagnostish.Domain.Common;
using Diagnostish.Domain.Interfaces;
using Diagnostish.Domain.Models.Entities.Hardware;
using Diagnostish.Infrastructure.Analyzers.HardwareInfoAnalyzers.Messages;
using Diagnostish.Infrastructure.Providers.HardwareInfoProviders.RawHardwareInfo;
using Diagnostish.Infrastructure.Shared.Utils;

namespace Diagnostish.Infrastructure.Analyzers.HardwareInfoAnalyzers;

public class RamInfoAnalyzer(Serilog.ILogger logger) : IAnalyzeDiagnosticInfo<RawRamInfo, RamInfo>
{
    private static readonly Dictionary<string, string> RamTypes = new()
    {
        { "20", "DDR" }, 
        { "21", "DDR2" }, 
        { "24", "DDR3" }, 
        { "26", "DDR4" }, 
        { "34", "DDR5" }
    };

    public ProvideResult<RamInfo> AnalyzeInfo(ProvideResult<IReadOnlyList<RawRamInfo>> providedRamData)
    {
        var warnings = new List<string>(providedRamData.Warnings);

        if (providedRamData.Data is not { Count: > 0 } ramInfo) 
            return ProvideResult<RamInfo>.Fail(warnings, providedRamData.CriticalErrors);

        var types = new List<string>();
        string type = "Неизвестно";

        double totalCapacityInBytes = 0;
        double totalCapacityInGB = 0;

        var speeds = new List<int>();
        int speed = 0;

        int unknownTypeCount = 0;
        int unknownCapacityCount = 0;
        int unknownSpeedCount = 0;

        foreach (var item in ramInfo)
        {
            if (item.Type is not null && RamTypes.TryGetValue(item.Type, out var humanReadableType)) types.Add(humanReadableType);
            else unknownTypeCount++;

            if (item.Capacity is { } capacity && capacity > 0) totalCapacityInBytes += capacity;
            else unknownCapacityCount++;

            if (item.Speed is { } spd && spd > 0) speeds.Add(spd);
            else unknownSpeedCount++;
        }

        if (unknownTypeCount > 0)
        {
            warnings.Add($"{RamAnalyzerMessages.UnknownType} {CommonMessages.CountOfTotal(unknownTypeCount, ramInfo.Count)}");
            logger.Warning("{UnknownTypeMessage} Затронуто {Count} из {Total} модулей.", RamAnalyzerMessages.UnknownType, 
                                                                                         unknownTypeCount, ramInfo.Count);
        }
        if (unknownCapacityCount > 0)
        {
            warnings.Add($"{RamAnalyzerMessages.UnknownCapacity} {CommonMessages.CountOfTotal(unknownCapacityCount, ramInfo.Count)}");
            logger.Warning("{UnknownCapacityMessage} Затронуто {Count} из {Total} модулей.", RamAnalyzerMessages.UnknownCapacity, 
                                                                                             unknownCapacityCount, ramInfo.Count);
        }
        if (unknownSpeedCount > 0)
        {
            warnings.Add($"{RamAnalyzerMessages.UnknownSpeed} {CommonMessages.CountOfTotal(unknownSpeedCount, ramInfo.Count)}");
            logger.Warning("{UnknownSpeedMessage} Затронуто {Count} из {Total} модулей.", RamAnalyzerMessages.UnknownSpeed, 
                                                                                          unknownSpeedCount, ramInfo.Count);
        }

        if (types.Count > 0)
        {
            type = types[0];
            if (types.Any(s => s != type))
            {
                type = "Неизвестно";
                warnings.Add(RamAnalyzerMessages.TypeConflict);
                logger.Warning(RamAnalyzerMessages.TypeConflict + "({Types})", string.Join(", ", types));
            }
        }

        if (totalCapacityInBytes > 0) totalCapacityInGB = ByteConverter.ToGigabytes(totalCapacityInBytes, 2);

        if (speeds.Count > 0)
        {
            speed = speeds.Min();
            if (speeds.Any(s => s != speed))
            {
                warnings.Add(RamAnalyzerMessages.SpeedConflict);
                logger.Warning(RamAnalyzerMessages.SpeedConflict + "({Speeds}) Выбрана минимальная: {Min} MHz", string.Join(", ", speeds), speed);
            }
        }

        return ProvideResult<RamInfo>.Ok(new RamInfo(type, totalCapacityInGB, speed), warnings);
    }
}