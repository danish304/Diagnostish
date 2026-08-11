using Diagnostish.Infrastructure.Providers.Common.RawModels.Hardware;
using Diagnostish.Infrastructure.Providers.Wmi.Common;
using Diagnostish.Infrastructure.Shared.Common.Utils;
using Diagnostish.Infrastructure.Shared.Wmi.Executor;
using System.Management;

namespace Diagnostish.Infrastructure.Providers.Wmi.Hardware;

public class RamWmiProvider(IWmiExecutor executor)
    : BaseWmiProvider<RawRamModel>(executor)
{
    private const string TYPE = "SMBIOSMemoryType";
    private const string CAPACITY = "Capacity";
    private const string SPEED = "Speed";

    protected override string BuildQuery() =>
        $"SELECT {TYPE}, {CAPACITY}, {SPEED} FROM Win32_PhysicalMemory";

    protected override string ContextName => "об ОЗУ";

    protected override RawRamModel Map(ManagementBaseObject item)
    {
        return new RawRamModel(
            Parser.ToSafeString(item[TYPE]),
            Parser.ToSafeDouble(item[CAPACITY]),
            Parser.ToSafeInt(item[SPEED])
        );
    }
}