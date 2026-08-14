using Application.Mappers.Common;
using Domain.Models.Entities.Network;
using Domain.Models.Reports.Components;

namespace Application.Mappers.Network;

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