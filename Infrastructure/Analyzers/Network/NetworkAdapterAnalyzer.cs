using Domain.Models.Entities.Network;
using Infrastructure.Providers.Common.RawModels.Network;
using Serilog;

using static Infrastructure.Analyzers.Common.CommonMessages;
using static Infrastructure.Analyzers.Network.Messages.NetworkAdapterAnalyzerMessages;

namespace Infrastructure.Analyzers.Network;

public class NetworkAdapterAnalyzer(ILogger logger)
    : IAnalyzer<NetworkAdapterRawModel, IReadOnlyList<NetworkAdapter>>
{
    public ProvideResult<IReadOnlyList<NetworkAdapter>> Analyze(
        ProvideResult<IReadOnlyList<NetworkAdapterRawModel>> result)
    {
        var warnings = new List<string>(result.Warnings);

        if (result.Data is not { Count: > 0 } rawData)
        {
            return ProvideResult<IReadOnlyList<NetworkAdapter>>.Fail(
                warnings,
                result.CriticalErrors);
        }

        var (
            networkAdapters,
            unknownDescriptionCount, unknownMacAddressCount, unknownDhcpEnabledCount) = BuildNetworkAdapterList(rawData);

        AppendCountWarnings(
            warnings,
            logger,
            unknownDescriptionCount,
            unknownMacAddressCount,
            unknownDhcpEnabledCount,
            rawData.Count);

        return ProvideResult<IReadOnlyList<NetworkAdapter>>.Ok(networkAdapters, warnings);
    }

    private static (
        List<NetworkAdapter> Adapters,
        int UnknownDescriprionCount, int UnknownMacAddressCount, int UnknownDhcpEnabledCount)
        BuildNetworkAdapterList(IReadOnlyList<NetworkAdapterRawModel> rawData)
    {
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

        return (networkAdapters, unknownDescriptionCount, unknownMacAddressCount, unknownDhcpEnabledCount);
    }

    private static void AppendCountWarnings(
        List<string> warnings,
        ILogger logger,
        int unknownDescriptionCount,
        int unknownMacAddressCount,
        int unknownDhcpEnabledCount,
        int total)
    {
        if (unknownDescriptionCount > 0)
        {
            warnings.Add($"{UNKNOWN_DESCRIPTION} {CountOfTotal(unknownDescriptionCount, total)}");

            logger.Warning(
                UNKNOWN_DESCRIPTION + " Затронуто {Count} из {Total} сетевых адаптеров.",
                unknownDescriptionCount, total);
        }
        if (unknownMacAddressCount > 0)
        {
            warnings.Add($"{UNKNOWN_MAC_ADDRESS} {CountOfTotal(unknownMacAddressCount, total)}");

            logger.Warning(
                UNKNOWN_MAC_ADDRESS + " Затронуто {Count} из {Total} сетевых адаптеров.",
                unknownMacAddressCount, total);
        }
        if (unknownDhcpEnabledCount > 0)
        {
            warnings.Add($"{UNKNOWN_DHCP_ENABLED} {CountOfTotal(unknownDhcpEnabledCount, total)}");

            logger.Warning(
                UNKNOWN_DHCP_ENABLED + " Затронуто {Count} из {Total} сетевых адаптеров.",
                unknownDhcpEnabledCount, total);
        }
    }
}