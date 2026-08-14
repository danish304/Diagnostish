using Diagnostish.Domain.Models.Entities.Hardware;
using Diagnostish.Infrastructure.Providers.Common.RawModels.Hardware;
using Diagnostish.Infrastructure.Shared.Common.Utils;
using Serilog;

using static Diagnostish.Infrastructure.Analyzers.Common.CommonMessages;
using static Diagnostish.Infrastructure.Analyzers.Hardware.Messages.GpuAnalyzerMessages;

namespace Diagnostish.Infrastructure.Analyzers.Hardware;

public class GpuAnalyzer(ILogger logger)
    : IAnalyzer<GpuRawModel, IReadOnlyList<Gpu>>
{
    public ProvideResult<IReadOnlyList<Gpu>> Analyze(
        ProvideResult<IReadOnlyList<GpuRawModel>> result)
    {
        var warnings = new List<string>(result.Warnings);

        if (result.RawData is not { Count: > 0 } rawData)
        {
            return ProvideResult<IReadOnlyList<Gpu>>.Fail(
                warnings,
                result.CriticalErrors);
        }

        var videoCards = new List<Gpu>();
        int unknownNameCount = 0;
        int unknownAdapterRamCount = 0;

        foreach (var model in rawData)
        {
            string gpuName = model.Name ?? "Неизвестно";
            double adapterRam = 0;

            if (model.Name is null)
            {
                unknownNameCount++;
            }

            if (model.AdapterRam is { } ram && ram > 0)
            {
                adapterRam = ByteConverter.ToGigabytes(ram);
            }
            else
            {
                unknownAdapterRamCount++;
            }

            videoCards.Add(new Gpu(gpuName, adapterRam));
        }

        if (unknownNameCount > 0)
        {
            warnings.Add($"{UNKNOWN_NAME} {CountOfTotal(unknownNameCount, rawData.Count)}");

            logger.Warning(
                UNKNOWN_NAME + " Затронуто {Count} из {Total} видеокарт.",
                unknownNameCount, rawData.Count);
        }
        if (unknownAdapterRamCount > 0)
        {
            warnings.Add($"{UNKNOWN_ADAPTER_RAM} {CountOfTotal(unknownAdapterRamCount, rawData.Count)}");

            logger.Warning(
                UNKNOWN_ADAPTER_RAM + " Затронуто {Count} из {Total} видеокарт.",
                unknownAdapterRamCount, rawData.Count);
        }

        return ProvideResult<IReadOnlyList<Gpu>>.Ok(videoCards, warnings);
    }
}