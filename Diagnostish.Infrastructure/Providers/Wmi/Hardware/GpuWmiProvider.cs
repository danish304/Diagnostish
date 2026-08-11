using Diagnostish.Infrastructure.Providers.Common.RawModels.Hardware;
using Diagnostish.Infrastructure.Providers.Wmi.Common;
using Diagnostish.Infrastructure.Shared.Common.Utils;
using Diagnostish.Infrastructure.Shared.Wmi.Executor;
using System.Management;

namespace Diagnostish.Infrastructure.Providers.Wmi.Hardware;

public class GpuWmiProvider(IWmiExecutor executor) 
    : BaseWmiProvider<RawGpuModel>(executor)
{
    private const string GPUNAME = "Name";
    private const string GPURAM = "AdapterRAM";

    protected override string BuildQuery() => 
        $"SELECT {GPUNAME}, {GPURAM} FROM Win32_VideoController";

    protected override string ContextName => "о видеокартах";

    protected override RawGpuModel Map(ManagementBaseObject item)
    {
        return new RawGpuModel(
            Parser.ToSafeString(item[GPUNAME]),
            Parser.ToSafeDouble(item[GPURAM])
        );
    }
}