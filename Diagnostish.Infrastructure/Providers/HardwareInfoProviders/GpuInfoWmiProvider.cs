using Diagnostish.Infrastructure.Providers.Common;
using Diagnostish.Infrastructure.Providers.HardwareInfoProviders.RawHardwareInfo;
using Diagnostish.Infrastructure.Shared.Utils;
using Diagnostish.Infrastructure.Shared.Wmi.Executor;
using System.Management;

namespace Diagnostish.Infrastructure.Providers.HardwareInfoProviders;

public class GpuInfoWmiProvider(IExecutorWmi executor) : BaseWmiProvider<RawGpuInfo>(executor)
{
    private const string GPUNAME = "Name";
    private const string GPURAM = "AdapterRAM";

    protected override string BuildQuery() => $"SELECT {GPUNAME}, {GPURAM} FROM Win32_VideoController";

    protected override string ContextName => "о видеокартах";

    protected override RawGpuInfo Map(ManagementBaseObject item)
    {
        return new RawGpuInfo(Parser.ToSafeString(item[GPUNAME]),
                              Parser.ToSafeDouble(item[GPURAM]));
    }
}