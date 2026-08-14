using Infrastructure.Providers.Common.RawModels.Network;
using Infrastructure.Providers.Wmi.Common;
using Infrastructure.Shared.Common.Utils;
using Infrastructure.Shared.Wmi.Executor;
using System.Management;

namespace Infrastructure.Providers.Wmi.Network;

public class GatewayWmiProvider(IWmiExecutor executor)
    : BaseWmiProvider<GatewayRawModel>(executor)
{
    private const string GATEWAY_INTERFACE = "Description";
    private const string GATEWAY_ADDRESS = "DefaultIPGateway";

    protected override string BuildQuery() =>
        $"SELECT {GATEWAY_INTERFACE}, {GATEWAY_ADDRESS} " +
        $"FROM Win32_NetworkAdapterConfiguration " +
        $"WHERE IPEnabled = True";

    protected override string ContextName => "о шлюзах сети";

    protected override GatewayRawModel Map(ManagementBaseObject item)
    {
        return new GatewayRawModel(
            Parser.ToSafeArrayString(item[GATEWAY_ADDRESS]),
            Parser.ToSafeString(item[GATEWAY_INTERFACE])
        );
    }
}