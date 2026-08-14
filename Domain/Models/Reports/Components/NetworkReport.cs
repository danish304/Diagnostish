using Domain.Models.Entities.Network;
using Domain.Models.Reports.Common;

namespace Domain.Models.Reports.Components;

public class NetworkReport : BaseIssuesReport
{
    // Компоненты коллекции
    public IReadOnlyList<NetworkAdapter> NetworkAdapters { get; set; } = [];
    public IReadOnlyList<IpAddress> IpAddresses { get; set; } = [];
    public IReadOnlyList<Gateway> Gateways { get; set; } = [];
    public IReadOnlyList<Dns> DnsAddresses { get; set; } = [];
}