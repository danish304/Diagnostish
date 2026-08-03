using Diagnostish.Infrastructure.Providers.Common;
using Diagnostish.Infrastructure.Providers.HardwareInfoProviders.RawHardwareInfo;
using Diagnostish.Infrastructure.Shared.Utils;
using Diagnostish.Infrastructure.Shared.Wmi.Executor;
using System.Management;

namespace Diagnostish.Infrastructure.Providers.HardwareInfoProviders.Wmi;

public class BiosInfoWmiProvider(IExecutorWmi executor) : BaseWmiProvider<RawBiosInfo>(executor)
{
    private const string VERSION = "Version";
    private const string RELEASE = "ReleaseDate";
    private const string MANUFACTURER = "Manufacturer";

    protected override string BuildQuery() => $"SELECT {VERSION}, {RELEASE}, {MANUFACTURER} FROM Win32_BIOS";

    protected override string ContextName => "о биосе";

    protected override RawBiosInfo Map(ManagementBaseObject item)
    {
        return new RawBiosInfo(Parser.ToSafeString(item[VERSION]),
                               Parser.ToSafeDateTime(item[RELEASE]),
                               Parser.ToSafeString(item[MANUFACTURER]));
    }
}