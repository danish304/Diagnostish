using Diagnostish.Infrastructure.Providers.Common.RawModels.Hardware;
using Diagnostish.Infrastructure.Providers.Registry.Common;
using Diagnostish.Infrastructure.Providers.Wmi.Common;

namespace Diagnostish.Infrastructure.Providers;

public class GpuFallBackProvider(
    IWmiSource<RawGpuModel> wmiProvider, 
    IRegistrySource<RawGpuModel> registryProvider) 
    : IProvider<RawGpuModel>
{
    private const double UINT32_OVERFLOW = 4_290_000_000;

    public async Task<ProvideResult<IReadOnlyList<RawGpuModel>>> ProvideAsync(
        CancellationToken cancellationToken = default)
    {
        var wmiResult = await wmiProvider.ProvideAsync(cancellationToken);

        bool needsFallback = wmiResult.Data is null || wmiResult.Data.Any(gpu => 
            gpu.AdapterRam is null or <= 0 or >= UINT32_OVERFLOW);

        if (!needsFallback)
        {
            return wmiResult;
        }

        var registryResult = await registryProvider.ProvideAsync(cancellationToken);

        if (registryResult.Data is not { Count: > 0 })
        {
            return HandleRegistryFallbackFail(wmiResult, registryResult);
        }

        return MergeMemoryFromRegistry(wmiResult, registryResult);
    }

    private static ProvideResult<IReadOnlyList<RawGpuModel>> HandleRegistryFallbackFail(
        ProvideResult<IReadOnlyList<RawGpuModel>> wmiResult,
        ProvideResult<IReadOnlyList<RawGpuModel>> registryResult)
    {
        if (wmiResult.Data is not null)
        {
            var warnings = new List<string>(wmiResult.Warnings);
            warnings.AddRange(registryResult.Warnings);
            warnings.AddRange(registryResult.CriticalErrors);

            return ProvideResult<IReadOnlyList<RawGpuModel>>.Ok(wmiResult.Data, warnings);
        }

        var combinedWarnings = new List<string>(wmiResult.Warnings);
        combinedWarnings.AddRange(registryResult.Warnings);

        var combinedCriticalErrors = new List<string>(wmiResult.CriticalErrors);
        combinedCriticalErrors.AddRange(registryResult.CriticalErrors);

        return ProvideResult<IReadOnlyList<RawGpuModel>>.Fail(combinedWarnings, combinedCriticalErrors);
    }

    private static ProvideResult<IReadOnlyList<RawGpuModel>> MergeMemoryFromRegistry(
        ProvideResult<IReadOnlyList<RawGpuModel>> wmiResult, 
        ProvideResult<IReadOnlyList<RawGpuModel>> registryResult)
    {
        var wmiGpus = wmiResult.Data ?? [];
        var registryGpus = registryResult.Data!;

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

        return ProvideResult<IReadOnlyList<RawGpuModel>>.Ok(merged, warnings);
    }
}