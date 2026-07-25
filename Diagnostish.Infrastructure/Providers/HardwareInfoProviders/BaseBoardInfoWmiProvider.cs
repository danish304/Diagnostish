using Diagnostish.Infrastructure.Providers.Common;
using Diagnostish.Infrastructure.Providers.HardwareInfoProviders.RawHardwareInfo;
using Diagnostish.Infrastructure.Shared.Utils;
using Diagnostish.Infrastructure.Shared.Wmi.Executor;
using System.Management;

namespace Diagnostish.Infrastructure.Providers.HardwareInfoProviders;

public class BaseBoardInfoWmiProvider(IExecutorWmi executor) : BaseWmiProvider<RawBaseBoardInfo>(executor)
{
    private const string MODEL = "Product";
    private const string MANUFACTURER = "Manufacturer";
    private const string VERSION = "Version";
    private const string STATUS = "Status";

    protected override string BuildQuery() => $"SELECT {MODEL}, {MANUFACTURER}, {VERSION}, {STATUS} FROM Win32_BaseBoard";

    protected override string ContextName => "о материнской плате";

    protected override RawBaseBoardInfo Map(ManagementBaseObject item)
    {
        return new RawBaseBoardInfo(Parser.ToSafeString(item[MODEL]),
                                    Parser.ToSafeString(item[MANUFACTURER]),
                                    Parser.ToSafeString(item[VERSION]),
                                    Parser.ToSafeString(item[STATUS])
        );
    }
}