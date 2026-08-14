using Diagnostish.Application.Mappers.Common;
using Diagnostish.Domain.Models.Entities.Network;
using Diagnostish.Domain.Models.Reports.Components;

namespace Diagnostish.Application.Mappers.Network;

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