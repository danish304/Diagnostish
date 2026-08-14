using Diagnostish.Application.Mappers.Common;
using Diagnostish.Domain.Models.Entities.Network;
using Diagnostish.Domain.Models.Reports.Components;

namespace Diagnostish.Application.Mappers.Network;

public class NetworkAdapterReportMapper
    : IReportMapper<NetworkReport, IReadOnlyList<NetworkAdapter>>
{
    public void MapInto(
        NetworkReport report,
        ProvideResult<IReadOnlyList<NetworkAdapter>> result)
    {
        if (report.TryExtractData(result, out var data))
        {
            report.NetworkAdapters = [.. data];
        }
    }
}