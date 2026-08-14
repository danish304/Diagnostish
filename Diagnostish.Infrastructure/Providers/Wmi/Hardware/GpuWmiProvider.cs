using Diagnostish.Infrastructure.Providers.Common.RawModels.Hardware;
using Diagnostish.Infrastructure.Providers.Wmi.Common;
using Diagnostish.Infrastructure.Shared.Common.Utils;
using Diagnostish.Infrastructure.Shared.Wmi.Executor;
using System.Management;

namespace Diagnostish.Infrastructure.Providers.Wmi.Hardware;

public class GpuWmiProvider(IWmiExecutor executor)
    : BaseWmiProvider<GpuRawModel>(executor)
{
    private const string GPU_NAME = "Name";
    private const string GPU_ADAPTER_RAM = "AdapterRAM";

    protected override string BuildQuery() =>
        $"SELECT {GPU_NAME}, {GPU_ADAPTER_RAM} FROM Win32_VideoController";

    protected override string ContextName => "о видеокартах";

    protected override GpuRawModel Map(ManagementBaseObject item)
    {
        return new GpuRawModel(
            Parser.ToSafeString(item[GPU_NAME]),
            Parser.ToSafeDouble(item[GPU_ADAPTER_RAM])
        );
    }
}