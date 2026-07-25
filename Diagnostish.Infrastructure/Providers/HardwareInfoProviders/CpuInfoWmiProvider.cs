using Diagnostish.Infrastructure.Providers.Common;
using Diagnostish.Infrastructure.Providers.HardwareInfoProviders.RawHardwareInfo;
using Diagnostish.Infrastructure.Shared.Utils;
using Diagnostish.Infrastructure.Shared.Wmi.Executor;
using System.Management;

namespace Diagnostish.Infrastructure.Providers.HardwareInfoProviders;

public class CpuInfoWmiProvider(IExecutorWmi executor) : BaseWmiProvider<RawCpuInfo>(executor)
{
    private const string CPUNAME = "Name";
    private const string COUNTCORES = "NumberOfCores";
    private const string SPEED = "CurrentClockSpeed";

    protected override string BuildQuery() => $"SELECT {CPUNAME}, {COUNTCORES}, {SPEED} FROM Win32_Processor";

    protected override string ContextName => "о процессоре";

    protected override RawCpuInfo Map(ManagementBaseObject item)
    {
        return new RawCpuInfo(Parser.ToSafeString(item[CPUNAME]),
                              Parser.ToSafeInt(item[COUNTCORES]),
                              Parser.ToSafeInt(item[SPEED]));
    }
}