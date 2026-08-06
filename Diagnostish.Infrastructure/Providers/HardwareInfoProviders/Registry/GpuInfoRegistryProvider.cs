using Diagnostish.Infrastructure.Providers.Common;
using Diagnostish.Infrastructure.Providers.HardwareInfoProviders.RawHardwareInfo;
using Diagnostish.Infrastructure.Shared.Registry.Executor;
using Diagnostish.Infrastructure.Shared.Utils;
using Microsoft.Win32;

namespace Diagnostish.Infrastructure.Providers.HardwareInfoProviders.Registry;

public class GpuInfoRegistryProvider(IExecutorRegistry executor) : BaseRegistryProvider<RawGpuInfo>(executor)
{
    private const string GPUNAME = "DriverDesc";
    private const string GPURAM = "HardwareInformation.qwMemorySize";

    protected override string RootPath => @"SYSTEM\CurrentControlSet\Control\Class\{4d36e968-e325-11ce-bfc1-08002be10318}";

    protected override string ContextName => "о видеокарте";

    protected override bool IsRelevantSubKey(string subKeyName) => subKeyName.All(char.IsDigit);

    protected override RawGpuInfo? Map(RegistryKey subKey)
    {
        object? nameValue = subKey.GetValue(GPUNAME);
        object? memoryValue = subKey.GetValue(GPURAM);

        if (nameValue is null && memoryValue is null) return null;   

        return new RawGpuInfo(Parser.ToSafeString(nameValue),
                              Parser.ToSafeDouble(memoryValue));
    }
}