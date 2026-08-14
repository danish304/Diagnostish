using Diagnostish.Application.Mappers.Common;
using Diagnostish.Domain.Models.Entities.Network;
using Diagnostish.Domain.Models.Reports.Components;

namespace Diagnostish.Application.Mappers.Network;

public class DnsReportMapper
    : IReportMapper<NetworkReport, IReadOnlyList<Dns>>
{
    public void MapInto(
        NetworkReport report,
        ProvideResult<IReadOnlyList<Dns>> result)
    {
        if (report.TryExtractData(result, out var data))
        {
            report.DnsAddresses = [.. data];
        }
    }
}