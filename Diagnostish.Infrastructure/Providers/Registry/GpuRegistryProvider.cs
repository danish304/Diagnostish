using Diagnostish.Infrastructure.Providers.Common.RawModels.Hardware;
using Diagnostish.Infrastructure.Providers.Registry.Common;
using Diagnostish.Infrastructure.Shared.Common.Utils;
using Diagnostish.Infrastructure.Shared.Registry.Executor;
using Microsoft.Win32;

namespace Diagnostish.Infrastructure.Providers.Registry;

public class GpuRegistryProvider(IRegistryExecutor executor)
    : BaseRegistryProvider<GpuRawModel>(executor)
{
    private const string GPU_NAME = "DriverDesc";
    private const string GPU_ADAPTER_RAM = "HardwareInformation.qwMemorySize";

    protected override string RootPath =>
        @"SYSTEM\CurrentControlSet\Control\Class\{4d36e968-e325-11ce-bfc1-08002be10318}";

    protected override string ContextName => "о видеокарте";

    protected override bool IsRelevantSubKey(string subKeyName) =>
        subKeyName.All(char.IsDigit);

    protected override GpuRawModel? Map(RegistryKey subKey)
    {
        object? nameValue = subKey.GetValue(GPU_NAME);
        object? memoryValue = subKey.GetValue(GPU_ADAPTER_RAM);

        if (nameValue is null && memoryValue is null)
        {
            return null;
        }

        return new GpuRawModel(
            Parser.ToSafeString(nameValue),
            Parser.ToSafeDouble(memoryValue)
        );
    }
}