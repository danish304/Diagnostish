using Infrastructure.Providers.Common.RawModels.Hardware;
using Infrastructure.Providers.Registry.Common;
using Infrastructure.Providers.Wmi.Common;

namespace Infrastructure.Providers;

public class GpuFallBackProvider(
    IWmiSource<GpuRawModel> wmiProvider,
    IRegistrySource<GpuRawModel> registryProvider)
    : IProvider<GpuRawModel>
{
    private const double UINT32_OVERFLOW = 4_290_000_000;

    public async Task<ProvideResult<IReadOnlyList<GpuRawModel>>> ProvideAsync(
        CancellationToken cancellationToken = default)
    {
        var wmiResult = await wmiProvider.ProvideAsync(cancellationToken);

        bool needsFallback = wmiResult.RawData is null || wmiResult.RawData.Any(gpu =>
            gpu.AdapterRam is null or <= 0 or >= UINT32_OVERFLOW);

        if (!needsFallback)
        {
            return wmiResult;
        }

        var registryResult = await registryProvider.ProvideAsync(cancellationToken);

        if (registryResult.RawData is not { Count: > 0 })
        {
            return HandleRegistryFallbackFail(wmiResult, registryResult);
        }

        return MergeMemoryFromRegistry(wmiResult, registryResult);
    }

    private static ProvideResult<IReadOnlyList<GpuRawModel>> HandleRegistryFallbackFail(
        ProvideResult<IReadOnlyList<GpuRawModel>> wmiResult,
        ProvideResult<IReadOnlyList<GpuRawModel>> registryResult)
    {
        if (wmiResult.RawData is not null)
        {
            var warnings = new List<string>(wmiResult.Warnings);
            warnings.AddRange(registryResult.Warnings);
            warnings.AddRange(registryResult.CriticalErrors);

            return ProvideResult<IReadOnlyList<GpuRawModel>>.Ok(wmiResult.RawData, warnings);
        }

        var combinedWarnings = new List<string>(wmiResult.Warnings);
        combinedWarnings.AddRange(registryResult.Warnings);

        var combinedCriticalErrors = new List<string>(wmiResult.CriticalErrors);
        combinedCriticalErrors.AddRange(registryResult.CriticalErrors);

        return ProvideResult<IReadOnlyList<GpuRawModel>>.Fail(combinedWarnings, combinedCriticalErrors);
    }

    private static ProvideResult<IReadOnlyList<GpuRawModel>> MergeMemoryFromRegistry(
        ProvideResult<IReadOnlyList<GpuRawModel>> wmiResult,
        ProvideResult<IReadOnlyList<GpuRawModel>> registryResult)
    {
        var wmiGpus = wmiResult.RawData ?? [];
        var registryGpus = registryResult.RawData!;

        bool anyReplaced = false;
        bool anyStillInvalid = false;

        var merged = wmiGpus.Select(wmiGpu =>
        {
            var match = registryGpus.FirstOrDefault(r =>
                r.Name is not null
                && wmiGpu.Name is not null
                && r.Name.Contains(wmiGpu.Name, StringComparison.OrdinalIgnoreCase));

            bool wmiMemoryInvalid = wmiGpu.AdapterRam is null or <= 0 or >= UINT32_OVERFLOW;

            if (wmiMemoryInvalid && match?.AdapterRam is > 0)
            {
                anyReplaced = true;

                return wmiGpu with
                {
                    AdapterRam = match.AdapterRam
                };
            }

            if (wmiMemoryInvalid)
            {
                anyStillInvalid = true;
            }

            return wmiGpu;
        })
        .ToList();

        var warnings = new List<string>(wmiResult.Warnings);
        warnings.AddRange(registryResult.Warnings);

        if (anyReplaced)
        {
            warnings.Add("Объём видеопамяти для части адаптеров получен из реестра — WMI вернул некорректное значение.");
        }

        if (anyStillInvalid)
        {
            warnings.Add("Не удалось определить точный объём видеопамяти для одного из адаптеров ни через WMI, ни через реестр.");
        }

        return ProvideResult<IReadOnlyList<GpuRawModel>>.Ok(merged, warnings);
    }
}