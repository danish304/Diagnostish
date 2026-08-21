using Domain.Models.Entities.Hardware;
using Infrastructure.Providers.Common.RawModels.Hardware;
using Infrastructure.Shared.Common.Utils;
using Serilog;

using static Infrastructure.Analyzers.Common.CommonMessages;
using static Infrastructure.Analyzers.Hardware.Messages.GpuAnalyzerMessages;

namespace Infrastructure.Analyzers.Hardware;

public class GpuAnalyzer(ILogger logger)
    : IAnalyzer<GpuRawModel, IReadOnlyList<Gpu>>
{
    public ProvideResult<IReadOnlyList<Gpu>> Analyze(
        ProvideResult<IReadOnlyList<GpuRawModel>> result)
    {
        var warnings = new List<string>(result.Warnings);

        if (result.Data is not { Count: > 0 } rawData)
        {
            return ProvideResult<IReadOnlyList<Gpu>>.Fail(
                warnings,
                result.CriticalErrors);
        }

        var (videoCards, unknownNameCount, unknownAdapterRamCount) = BuildGpuList(rawData);

        AppendCountWarnings(
            warnings,
            logger,
            unknownNameCount,
            unknownAdapterRamCount,
            rawData.Count);

        return ProvideResult<IReadOnlyList<Gpu>>.Ok(videoCards, warnings);
    }

    private static (List<Gpu> Gpus, int UnknownNameCount, int UnknownAdapterRamCount) BuildGpuList(
        IReadOnlyList<GpuRawModel> rawData)
    {
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

        return (videoCards, unknownNameCount, unknownAdapterRamCount);
    }

    private static void AppendCountWarnings(
        List<string> warnings,
        ILogger logger,
        int unknownNameCount,
        int unknownAdapterRamCount,
        int total)
    {
        if (unknownNameCount > 0)
        {
            warnings.Add($"{UNKNOWN_NAME} {CountOfTotal(unknownNameCount, total)}");

            logger.Warning(
                UNKNOWN_NAME + " Затронуто {Count} из {Total} видеокарт.",
                unknownNameCount, total);
        }
        if (unknownAdapterRamCount > 0)
        {
            warnings.Add($"{UNKNOWN_ADAPTER_RAM} {CountOfTotal(unknownAdapterRamCount, total)}");

            logger.Warning(
                UNKNOWN_ADAPTER_RAM + " Затронуто {Count} из {Total} видеокарт.",
                unknownAdapterRamCount, total);
        }
    }
}