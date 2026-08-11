using Diagnostish.Infrastructure.Providers.Common.RawModels.Hardware;
using Diagnostish.Infrastructure.Providers.Wmi.Common;
using Diagnostish.Infrastructure.Shared.Common.Utils;
using Diagnostish.Infrastructure.Shared.Wmi.Executor;
using System.Management;

namespace Diagnostish.Infrastructure.Providers.Wmi.Hardware;

public class BaseBoardWmiProvider(IWmiExecutor executor) 
    : BaseWmiProvider<RawBaseBoardModel>(executor)
{
    private const string MODEL = "Product";
    private const string MANUFACTURER = "Manufacturer";
    private const string VERSION = "Version";
    private const string STATUS = "Status";

    protected override string BuildQuery() => 
        $"SELECT {MODEL}, {MANUFACTURER}, {VERSION}, {STATUS} FROM Win32_BaseBoard";

    protected override string ContextName => "о материнской плате";

    protected override RawBaseBoardModel Map(ManagementBaseObject item)
    {
        return new RawBaseBoardModel(
            Parser.ToSafeString(item[MODEL]),
            Parser.ToSafeString(item[MANUFACTURER]),
            Parser.ToSafeString(item[VERSION]),
            Parser.ToSafeString(item[STATUS])
        );
    }
}