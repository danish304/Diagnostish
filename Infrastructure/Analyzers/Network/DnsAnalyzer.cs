using Domain.Models.Entities.Network;
using Infrastructure.Providers.Common.RawModels.Network;
using Serilog;

using static Infrastructure.Analyzers.Common.CommonMessages;
using static Infrastructure.Analyzers.Network.Messages.DnsAnalyzerMessages;

namespace Infrastructure.Analyzers.Network;

public class DnsAnalyzer(ILogger logger)
    : IAnalyzer<DnsRawModel, IReadOnlyList<Dns>>
{
    public ProvideResult<IReadOnlyList<Dns>> Analyze(
        ProvideResult<IReadOnlyList<DnsRawModel>> result)
    {
        var warnings = new List<string>(result.Warnings);

        if (result.Data is not { Count: > 0 } rawData)
        {
            return ProvideResult<IReadOnlyList<Dns>>.Fail(
                warnings,
                result.CriticalErrors);
        }

        var (dnsAddresses, unknownAddressCount, unknownInterfaceCount) = BuildDnsList(rawData);

        AppendCountWarnings(
            warnings,
            logger,
            unknownAddressCount,
            unknownInterfaceCount,
            rawData.Count);

        return ProvideResult<IReadOnlyList<Dns>>.Ok(dnsAddresses, warnings);
    }

    private static (List<Dns> DnsAddresses, int UnknownAddressCount, int UnknownInterfaceCount) BuildDnsList(
        IReadOnlyList<DnsRawModel> rawData)
    {
        var dnsAddresses = new List<Dns>();
        int unknownAddressCount = 0;
        int unknownInterfaceCount = 0;

        foreach (var model in rawData)
        {
            string dnsAddress = model.Address ?? "Неизвестно";
            string dnsInterface = model.Interface ?? "Неизвестно";

            if (model.Address is null)
            {
                unknownAddressCount++;
            }

            if (model.Interface is null)
            {
                unknownInterfaceCount++;
            }

            dnsAddresses.Add(new Dns(dnsAddress, dnsInterface));
        }

        return (dnsAddresses, unknownAddressCount, unknownInterfaceCount);
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
                UNKNOWN_ADDRESS + " Затронуто {Count} из {Total} DNS-адресов.",
                unknownAddressCount, total);
        }
        if (unknownInterfaceCount > 0)
        {
            warnings.Add($"{UNKNOWN_INTERFACE} {CountOfTotal(unknownInterfaceCount, total)}");

            logger.Warning(
                UNKNOWN_INTERFACE + " Затронуто {Count} из {Total} DNS-адресов.",
                unknownInterfaceCount, total);
        }
    }
}