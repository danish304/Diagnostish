using Application.Mappers.Common;
using Domain.Models.Entities.Network;
using Domain.Models.Reports.Components;

namespace Application.Mappers.Network;

public class IpAddressReportMapper
    : IReportMapper<NetworkReport, IReadOnlyList<IpAddress>>
{
    public void MapInto(
        NetworkReport report,
        ProvideResult<IReadOnlyList<IpAddress>> result)
    {
        if (report.TryExtractData(result, out var data))
        {
            report.IpAddresses = [.. data];
        }
    }
}