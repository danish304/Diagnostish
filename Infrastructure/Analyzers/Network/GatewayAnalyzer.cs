using Domain.Models.Entities.Network;
using Infrastructure.Providers.Common.RawModels.Network;
using Serilog;

using static Infrastructure.Analyzers.Common.CommonMessages;
using static Infrastructure.Analyzers.Network.Messages.GatewayAnalyzerMessages;

namespace Infrastructure.Analyzers.Network;

public class GatewayAnalyzer(ILogger logger)
    : IAnalyzer<GatewayRawModel, IReadOnlyList<Gateway>>
{
    public ProvideResult<IReadOnlyList<Gateway>> Analyze(
        ProvideResult<IReadOnlyList<GatewayRawModel>> result)
    {
        var warnings = new List<string>(result.Warnings);

        if (result.Data is not { Count: > 0 } rawData)
        {
            return ProvideResult<IReadOnlyList<Gateway>>.Fail(
                warnings,
                result.CriticalErrors);
        }

        var (gateways, unknownAddressCount, unknownInterfaceCount) = BuildGatewayList(rawData);

        AppendCountWarnings(
            warnings,
            logger,
            unknownAddressCount,
            unknownInterfaceCount,
            rawData.Count);

        return ProvideResult<IReadOnlyList<Gateway>>.Ok(gateways, warnings);
    }

    private static (List<Gateway> Gateways, int UnknownAddressCount, int UnknownInterfaceCount) BuildGatewayList(
        IReadOnlyList<GatewayRawModel> rawData)
    {
        var gateways = new List<Gateway>();
        int unknownAddressCount = 0;
        int unknownInterfaceCount = 0;

        foreach (var model in rawData)
        {
            string gatewayAddress = model.Address ?? "Неизвестно";
            string gatewayInterface = model.Interface ?? "Неизвестно";

            if (model.Address is null)
            {
                unknownAddressCount++;
            }

            if (model.Interface is null)
            {
                unknownInterfaceCount++;
            }

            gateways.Add(new Gateway(gatewayAddress, gatewayInterface));
        }

        return (gateways, unknownAddressCount, unknownInterfaceCount);
    }

    private static void AppendCountWarnings(
        List<string> warnings,
        ILogger logger,
        int unknownAddressCount,
        int unknownInterfaceCount,
        int total)
    {
        if (unknownAddressCount > 0)
        {
            warnings.Add($"{UNKNOWN_ADDRESS} {CountOfTotal(unknownAddressCount, total)}");

            logger.Warning(
                UNKNOWN_ADDRESS + " Затронуто {Count} из {Total} шлюзов.",
                unknownAddressCount, total);
        }
        if (unknownInterfaceCount > 0)
        {
            warnings.Add($"{UNKNOWN_INTERFACE} {CountOfTotal(unknownInterfaceCount, total)}");

            logger.Warning(
                UNKNOWN_INTERFACE + " Затронуто {Count} из {Total} шлюзов.",
                unknownInterfaceCount, total);
        }
    }
}