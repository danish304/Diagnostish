using Infrastructure.Providers.Common.RawModels.Hardware;
using Infrastructure.Providers.Wmi.Common;
using Infrastructure.Shared.Common.Utils;
using Infrastructure.Shared.Wmi.Executor;
using System.Management;

namespace Infrastructure.Providers.Wmi.Hardware;

public class BiosWmiProvider(IWmiExecutor executor)
    : BaseWmiProvider<BiosRawModel>(executor)
{
    private const string VERSION = "Version";
    private const string RELEASE = "ReleaseDate";
    private const string MANUFACTURER = "Manufacturer";

    protected override string BuildQuery() =>
        $"SELECT {VERSION}, {RELEASE}, {MANUFACTURER} FROM Win32_BIOS";

    protected override string ContextName => "о биосе";

    protected override BiosRawModel Map(ManagementBaseObject item)
    {
        return new BiosRawModel(
            Parser.ToSafeString(item[VERSION]),
            Parser.ToSafeDateTime(item[RELEASE]),
            Parser.ToSafeString(item[MANUFACTURER])
        );
    }
}