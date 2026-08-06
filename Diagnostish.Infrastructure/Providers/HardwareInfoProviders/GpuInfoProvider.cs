using Diagnostish.Domain.Common;
using Diagnostish.Domain.Interfaces;
using Diagnostish.Infrastructure.Providers.HardwareInfoProviders.RawHardwareInfo;
using Diagnostish.Infrastructure.Providers.HardwareInfoProviders.Registry;
using Diagnostish.Infrastructure.Providers.HardwareInfoProviders.Wmi;

namespace Diagnostish.Infrastructure.Providers.HardwareInfoProviders;

public class GpuInfoProvider(GpuInfoWmiProvider wmiProvider, 
                             GpuInfoRegistryProvider registryProvider) : IProvideDiagnosticInfo<RawGpuInfo>
{
    public async Task<ProvideResult<IReadOnlyList<RawGpuInfo>>> ProvideInfoAsync(CancellationToken cancellationToken = default)
    {
        var wmiResult = await wmiProvider.ProvideInfoAsync(cancellationToken);

        bool needsFallback = wmiResult.Data is null || wmiResult.Data.Any(gpu => gpu.AdapterRam is null or <= 0);

        if (!needsFallback) return wmiResult;

        var registryResult = await registryProvider.ProvideInfoAsync(cancellationToken);

        if (registryResult.Data is not { Count: > 0 }) return wmiResult;   

        return MergeMemoryFromRegistry(wmiResult, registryResult);
    }

    private static ProvideResult<IReadOnlyList<RawGpuInfo>> MergeMemoryFromRegistry(ProvideResult<IReadOnlyList<RawGpuInfo>> wmiResult, 
                                                                                    ProvideResult<IReadOnlyList<RawGpuInfo>> registryResult)
    {
        var wmiGpus = wmiResult.Data ?? [];
        var registryGpus = registryResult.Data!;

        var merged = wmiGpus.Select(wmiGpu =>
        {
            var match = registryGpus.FirstOrDefault(r =>
                r.Name is not null && wmiGpu.Name is not null && r.Name.Contains(wmiGpu.Name, StringComparison.OrdinalIgnoreCase));

            bool wmiMemoryInvalid = wmiGpu.AdapterRam is null or <= 0;

            return wmiMemoryInvalid && match?.AdapterRam is > 0
                ? wmiGpu with { AdapterRam = match.AdapterRam }
                : wmiGpu;
        }).ToList();

        var warnings = new List<string>(wmiResult.Warnings);
        warnings.AddRange(registryResult.Warnings);
        warnings.Add("Объём видеопамяти для части адаптеров получен из реестра — WMI вернул некорректное значение.");

        return ProvideResult<IReadOnlyList<RawGpuInfo>>.Ok(merged, warnings);
    }
}