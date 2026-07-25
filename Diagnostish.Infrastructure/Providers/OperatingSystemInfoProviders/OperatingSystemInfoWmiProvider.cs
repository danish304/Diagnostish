using Diagnostish.Infrastructure.Providers.Common;
using Diagnostish.Infrastructure.Shared.Utils;
using Diagnostish.Infrastructure.Shared.Wmi.Executor;
using System.Management;

namespace Diagnostish.Infrastructure.Providers.OperatingSystemInfoProviders;

public class OperatingSystemInfoWmiProvider(IExecutorWmi executor) : BaseWmiProvider<RawOperatingSystemInfo>(executor)
{
    private const string CAPTION = "Caption";
    private const string VERSION = "Version";
    private const string MANUFACTURER = "Manufacturer";
    private const string USER = "RegisteredUser";
    private const string INSTALL = "InstallDate";
    private const string LASTBOOT = "LastBootUpTime";

    protected override string BuildQuery() => $"SELECT {CAPTION}, {VERSION}, {MANUFACTURER}, {USER}, {INSTALL}, {LASTBOOT} FROM Win32_OperatingSystem";

    protected override string ContextName => "об операционной системе";

    protected override RawOperatingSystemInfo Map(ManagementBaseObject item)
    {
        return new RawOperatingSystemInfo(Parser.ToSafeString(item[CAPTION]),
                                          Parser.ToSafeString(item[MANUFACTURER]),
                                          Parser.ToSafeString(item[VERSION]),
                                          Parser.ToSafeDateTime(item[INSTALL]),
                                          Parser.ToSafeString(item[USER]),
                                          Parser.ToSafeDateTime(item[LASTBOOT]));
    }
}