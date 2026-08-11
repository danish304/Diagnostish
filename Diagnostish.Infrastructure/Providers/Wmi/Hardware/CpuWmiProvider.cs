using Diagnostish.Infrastructure.Providers.Common.RawModels.Hardware;
using Diagnostish.Infrastructure.Providers.Wmi.Common;
using Diagnostish.Infrastructure.Shared.Common.Utils;
using Diagnostish.Infrastructure.Shared.Wmi.Executor;
using System.Management;

namespace Diagnostish.Infrastructure.Providers.Wmi.Hardware;

public class CpuWmiProvider(IWmiExecutor executor) 
    : BaseWmiProvider<RawCpuModel>(executor)
{
    private const string CPUNAME = "Name";
    private const string COUNTCORES = "NumberOfCores";
    private const string SPEED = "CurrentClockSpeed";

    protected override string BuildQuery() => 
        $"SELECT {CPUNAME}, {COUNTCORES}, {SPEED} FROM Win32_Processor";

    protected override string ContextName => "о процессоре";

    protected override RawCpuModel Map(ManagementBaseObject item)
    {
        return new RawCpuModel(
            Parser.ToSafeString(item[CPUNAME]),
            Parser.ToSafeInt(item[COUNTCORES]),
            Parser.ToSafeInt(item[SPEED])
        );
    }
}