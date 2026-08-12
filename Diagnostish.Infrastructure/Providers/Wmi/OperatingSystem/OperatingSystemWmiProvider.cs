using Diagnostish.Infrastructure.Providers.Common.RawModels.OperatingSystem;
using Diagnostish.Infrastructure.Providers.Wmi.Common;
using Diagnostish.Infrastructure.Shared.Common.Utils;
using Diagnostish.Infrastructure.Shared.Wmi.Executor;
using System.Management;

namespace Diagnostish.Infrastructure.Providers.Wmi.OperatingSystem;

public class OperatingSystemWmiProvider(IWmiExecutor executor)
    : BaseWmiProvider<OperatingSystemRawModel>(executor)
{
    private const string CAPTION = "Caption";
    private const string VERSION = "Version";
    private const string MANUFACTURER = "Manufacturer";
    private const string USER = "RegisteredUser";
    private const string INSTALL = "InstallDate";
    private const string LAST_BOOT = "LastBootUpTime";

    protected override string BuildQuery() =>
        $"SELECT {CAPTION}, {VERSION}, {MANUFACTURER}, {USER}, {INSTALL}, {LAST_BOOT} " +
        $"FROM Win32_OperatingSystem";

    protected override string ContextName => "об операционной системе";

    protected override OperatingSystemRawModel Map(ManagementBaseObject item)
    {
        return new OperatingSystemRawModel(
            Parser.ToSafeString(item[CAPTION]),
            Parser.ToSafeString(item[MANUFACTURER]),
            Parser.ToSafeString(item[VERSION]),
            Parser.ToSafeDateTime(item[INSTALL]),
            Parser.ToSafeString(item[USER]),
            Parser.ToSafeDateTime(item[LAST_BOOT])
        );
    }
}