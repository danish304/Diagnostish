using Diagnostish.Infrastructure.Providers.Common;
using Diagnostish.Infrastructure.Providers.HardwareInfoProviders.RawHardwareInfo;
using Diagnostish.Infrastructure.Shared.Utils;
using Diagnostish.Infrastructure.Shared.Wmi.Executor;
using System.Management;

namespace Diagnostish.Infrastructure.Providers.HardwareInfoProviders;

public class RamInfoWmiProvider(IExecutorWmi executor) : BaseWmiProvider<RawRamInfo>(executor)
{
    private const string TYPE = "SMBIOSMemoryType";
    private const string CAPACITY = "Capacity";
    private const string SPEED = "Speed";

    protected override string BuildQuery() => $"SELECT {TYPE}, {CAPACITY}, {SPEED} FROM Win32_PhysicalMemory";

    protected override string ContextName => "об ОЗУ";

    protected override RawRamInfo Map(ManagementBaseObject item)
    {
        return new RawRamInfo(Parser.ToSafeString(item[TYPE]),
                              Parser.ToSafeDouble(item[CAPACITY]),
                              Parser.ToSafeInt(item[SPEED]));
    }
}