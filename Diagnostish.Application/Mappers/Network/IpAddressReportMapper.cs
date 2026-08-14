using Diagnostish.Application.Mappers.Common;
using Diagnostish.Domain.Models.Entities.Network;
using Diagnostish.Domain.Models.Reports.Components;

namespace Diagnostish.Application.Mappers.Network;

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