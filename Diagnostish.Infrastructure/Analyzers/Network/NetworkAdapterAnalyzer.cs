using Diagnostish.Domain.Models.Entities.Network;
using Diagnostish.Infrastructure.Providers.Common.RawModels.Network;
using Serilog;

using static Diagnostish.Infrastructure.Analyzers.Common.CommonMessages;
using static Diagnostish.Infrastructure.Analyzers.Network.Messages.NetworkAdapterAnalyzerMessages;

namespace Diagnostish.Infrastructure.Analyzers.Network;

public class NetworkAdapterAnalyzer(ILogger logger)
    : IAnalyzer<NetworkAdapterRawModel, IReadOnlyList<NetworkAdapter>>
{
    public ProvideResult<IReadOnlyList<NetworkAdapter>> Analyze(
        ProvideResult<IReadOnlyList<NetworkAdapterRawModel>> result)
    {
        var warnings = new List<string>(result.Warnings);

        if (result.RawData is not { Count: > 0 } rawData)
        {
            return ProvideResult<IReadOnlyList<NetworkAdapter>>.Fail(
                warnings,
                result.CriticalErrors);
        }

        var networkAdapters = new List<NetworkAdapter>();
        int unknownDescriptionCount = 0;
        int unknownMacAddressCount = 0;
        int unknownDhcpEnabledCount = 0;

        foreach (var model in rawData)
        {
            string networkAdapterDescription = model.Description ?? "Неизвестно";
            string networkAdapterMacAddress = model.MacAddress ?? "Неизвестно";
            string networkAdapterDhcpEnabled = model.DhcpEnabled ?? "Неизвестно";

            if (model.Description is null)
            {
                unknownDescriptionCount++;
            }

            if (model.MacAddress is null)
            {
                unknownMacAddressCount++;
            }

            if (model.DhcpEnabled is null)
            {
                unknownDhcpEnabledCount++;
            }

            networkAdapters.Add(new NetworkAdapter(
                networkAdapterDescription,
                networkAdapterMacAddress,
                networkAdapterDhcpEnabled));
        }

        if (unknownDescriptionCount > 0)
        {
            warnings.Add($"{UNKNOWN_DESCRIPTION} {CountOfTotal(unknownDescriptionCount, rawData.Count)}");

            logger.Warning(
                UNKNOWN_DESCRIPTION + " Затронуто {Count} из {Total} сетевых адаптеров.",
                unknownDescriptionCount, rawData.Count);
        }
        if (unknownMacAddressCount > 0)
        {
            warnings.Add($"{UNKNOWN_MAC_ADDRESS} {CountOfTotal(unknownMacAddressCount, rawData.Count)}");

            logger.Warning(
                UNKNOWN_MAC_ADDRESS + " Затронуто {Count} из {Total} сетевых адаптеров.",
                unknownMacAddressCount, rawData.Count);
        }
        if (unknownDhcpEnabledCount > 0)
        {
            warnings.Add($"{UNKNOWN_DHCP_ENABLED} {CountOfTotal(unknownDhcpEnabledCount, rawData.Count)}");

            logger.Warning(
                UNKNOWN_DHCP_ENABLED + " Затронуто {Count} из {Total} сетевых адаптеров.",
                unknownDhcpEnabledCount, rawData.Count);
        }

        return ProvideResult<IReadOnlyList<NetworkAdapter>>.Ok(networkAdapters, warnings);
    }
}