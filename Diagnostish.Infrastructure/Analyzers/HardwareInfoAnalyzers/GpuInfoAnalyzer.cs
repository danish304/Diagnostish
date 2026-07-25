using Diagnostish.Domain.Common;
using Diagnostish.Domain.Interfaces;
using Diagnostish.Domain.Models.Entities.Hardware;
using Diagnostish.Infrastructure.Analyzers.HardwareInfoAnalyzers.Messages;
using Diagnostish.Infrastructure.Providers.HardwareInfoProviders.RawHardwareInfo;
using Diagnostish.Infrastructure.Shared.Utils;

namespace Diagnostish.Infrastructure.Analyzers.HardwareInfoAnalyzers;

public class GpuInfoAnalyzer(Serilog.ILogger logger) : IAnalyzeDiagnosticInfo<RawGpuInfo, IReadOnlyList<GpuInfo>>
{
    public ProvideResult<IReadOnlyList<GpuInfo>> AnalyzeInfo(ProvideResult<IReadOnlyList<RawGpuInfo>> providedGpuData)
    {
        var warnings = new List<string>(providedGpuData.Warnings);

        if (providedGpuData.Data is not { Count: > 0 } gpuInfo)
            return ProvideResult<IReadOnlyList<GpuInfo>>.Fail(warnings, providedGpuData.CriticalErrors);

        var videoCards = new List<GpuInfo>();

        int unknownNameCount = 0;
        int unknownAdapterRamCount = 0;

        foreach (var item in gpuInfo)
        {
            string gpuName = "Unknown";
            double adapterRam = 0;

            if (item.Name is not null) gpuName = item.Name;
            else unknownNameCount++;

            if (item.AdapterRam is { } ram && ram > 0) adapterRam = ByteConverter.ToGigabytes(ram);
            else unknownAdapterRamCount++;

            videoCards.Add(new GpuInfo(gpuName, adapterRam));
        }

        if (unknownNameCount > 0)
        {
            warnings.Add($"{GpuAnalyzerMessages.UnknownName} {CommonMessages.CountOfTotal(unknownNameCount, gpuInfo.Count)}");
            logger.Warning("{UnknownNameMessage} Затронуто {Count} из {Total} видеокарт.", GpuAnalyzerMessages.UnknownName, unknownNameCount, gpuInfo.Count);
        }
        if (unknownAdapterRamCount > 0)
        {
            warnings.Add($"{GpuAnalyzerMessages.UnknownAdapterRam} {CommonMessages.CountOfTotal(unknownAdapterRamCount, gpuInfo.Count)}");
            logger.Warning("{UnknownAdapterRamMessage} Затронуто {Count} из {Total} видеокарт.", GpuAnalyzerMessages.UnknownAdapterRam, unknownAdapterRamCount, gpuInfo.Count);
        }

        return ProvideResult<IReadOnlyList<GpuInfo>>.Ok(videoCards, warnings);
    }
}