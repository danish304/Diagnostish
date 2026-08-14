using Infrastructure.Providers.Common.RawModels.Network;
using Infrastructure.Providers.Wmi.Common;
using Infrastructure.Shared.Common.Utils;
using Infrastructure.Shared.Wmi.Executor;
using System.Management;

namespace Infrastructure.Providers.Wmi.Network;

public class DnsWmiProvider(IWmiExecutor executor)
    : BaseWmiProvider<DnsRawModel>(executor)
{
    private const string DNS_INTERFACE = "Description";
    private const string DNS_ADDRESS = "DNSServerSearchOrder";

    protected override string BuildQuery() =>
        $"SELECT {DNS_INTERFACE}, {DNS_ADDRESS} " +
        $"FROM Win32_NetworkAdapterConfiguration " +
        $"WHERE IPEnabled = True";

    protected override string ContextName => "о DNS серверах";

    protected override DnsRawModel Map(ManagementBaseObject item)
    {
        return new DnsRawModel(
            Parser.ToSafeArrayString(item[DNS_ADDRESS]),
            Parser.ToSafeString(item[DNS_INTERFACE])
        );
    }
}