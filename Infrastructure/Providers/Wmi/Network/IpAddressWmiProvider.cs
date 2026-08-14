using Infrastructure.Providers.Common.RawModels.Network;
using Infrastructure.Providers.Wmi.Common;
using Infrastructure.Shared.Common.Utils;
using Infrastructure.Shared.Wmi.Executor;
using System.Management;

namespace Infrastructure.Providers.Wmi.Network;

public class IpAddressWmiProvider(IWmiExecutor executor)
    : BaseWmiProvider<IpAddressRawModel>(executor)
{
    private const string IP_ADDRESS = "IPAddress";
    private const string IP_SUBNET = "IPSubnet";
    private const string IP_INTERFACE = "Description";

    protected override string BuildQuery() =>
        $"SELECT {IP_INTERFACE}, {IP_ADDRESS}, {IP_SUBNET} " +
        $"FROM Win32_NetworkAdapterConfiguration " +
        $"WHERE IPEnabled = True";

    protected override string ContextName => "об IP-адресах";

    protected override IpAddressRawModel Map(ManagementBaseObject item)
    {
        return new IpAddressRawModel(
            Parser.ToSafeArrayString(item[IP_ADDRESS]),
            Parser.ToSafeArrayString(item[IP_SUBNET]),
            Parser.ToSafeString(item[IP_INTERFACE])
        );
    }
}