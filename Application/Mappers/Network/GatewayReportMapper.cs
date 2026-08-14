using Application.Mappers.Common;
using Domain.Models.Entities.Network;
using Domain.Models.Reports.Components;

namespace Application.Mappers.Network;

public class GatewayReportMapper
    : IReportMapper<NetworkReport, IReadOnlyList<Gateway>>
{
    public void MapInto(
        NetworkReport report,
        ProvideResult<IReadOnlyList<Gateway>> result)
    {
        if (report.TryExtractData(result, out var data))
        {
            report.Gateways = [.. data];
        }
    }
}