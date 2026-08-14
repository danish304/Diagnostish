using Diagnostish.Infrastructure.Providers.Common.RawModels.Network;
using Diagnostish.Infrastructure.Providers.Wmi.Common;
using Diagnostish.Infrastructure.Shared.Common.Utils;
using Diagnostish.Infrastructure.Shared.Wmi.Executor;
using System.Management;

namespace Diagnostish.Infrastructure.Providers.Wmi.Network;

public class NetworkAdatperWmiProvider(IWmiExecutor executor)
    : BaseWmiProvider<NetworkAdapterRawModel>(executor)
{
    private const string DESCRIPRION = "Description";
    private const string MAC_ADDRESS = "MACAddress";
    private const string DHCP_ENABLED = "DHCPEnabled";

    protected override string BuildQuery() =>
        $"SELECT {DESCRIPRION}, {MAC_ADDRESS}, {DHCP_ENABLED} " +
        $"FROM Win32_NetworkAdapterConfiguration " +
        $"WHERE IPEnabled = True";

    protected override string ContextName => "о сетевых адаптерах";

    protected override NetworkAdapterRawModel Map(ManagementBaseObject item)
    {
        return new NetworkAdapterRawModel(
            Parser.ToSafeString(item[DESCRIPRION]),
            Parser.ToSafeString(item[MAC_ADDRESS]),
            Parser.ToSafeString(item[DHCP_ENABLED])
        );
    }
}