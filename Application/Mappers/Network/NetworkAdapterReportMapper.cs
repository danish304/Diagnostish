using Application.Mappers.Common;
using Domain.Models.Entities.Network;
using Domain.Models.Reports.Components;

namespace Application.Mappers.Network;

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