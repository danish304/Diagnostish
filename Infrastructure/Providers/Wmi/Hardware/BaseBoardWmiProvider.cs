using Infrastructure.Providers.Common.RawModels.Hardware;
using Infrastructure.Providers.Wmi.Common;
using Infrastructure.Shared.Common.Utils;
using Infrastructure.Shared.Wmi.Executor;
using System.Management;

namespace Infrastructure.Providers.Wmi.Hardware;

public class BaseBoardWmiProvider(IWmiExecutor executor)
    : BaseWmiProvider<BaseBoardRawModel>(executor)
{
    private const string MODEL = "Product";
    private const string MANUFACTURER = "Manufacturer";
    private const string VERSION = "Version";
    private const string STATUS = "Status";

    protected override string BuildQuery() =>
        $"SELECT {MODEL}, {MANUFACTURER}, {VERSION}, {STATUS} FROM Win32_BaseBoard";

    protected override string ContextName => "о материнской плате";

    protected override BaseBoardRawModel Map(ManagementBaseObject item)
    {
        return new BaseBoardRawModel(
            Parser.ToSafeString(item[MODEL]),
            Parser.ToSafeString(item[MANUFACTURER]),
            Parser.ToSafeString(item[VERSION]),
            Parser.ToSafeString(item[STATUS])
        );
    }
}