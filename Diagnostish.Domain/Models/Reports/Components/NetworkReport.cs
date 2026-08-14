using Diagnostish.Domain.Models.Entities.Network;
using Diagnostish.Domain.Models.Reports.Common;

namespace Diagnostish.Domain.Models.Reports.Components;

public class NetworkReport : BaseIssuesReport
{
    // Компоненты коллекции
    public IReadOnlyList<NetworkAdapter> NetworkAdapters { get; set; } = [];
    public IReadOnlyList<IpAddress> IpAddresses { get; set; } = [];
    public IReadOnlyList<Gateway> Gateways { get; set; } = [];
    public IReadOnlyList<Dns> DnsAddresses { get; set; } = [];
}