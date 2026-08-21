using Domain.Models.Entities.Network;
using Infrastructure.Providers.Common.RawModels.Network;
using Serilog;

using static Infrastructure.Analyzers.Common.CommonMessages;
using static Infrastructure.Analyzers.Network.Messages.IpAddressAnalyzerMessages;

namespace Infrastructure.Analyzers.Network;

public class IpAddressAnalyzer(ILogger logger)
    : IAnalyzer<IpAddressRawModel, IReadOnlyList<IpAddress>>
{
    public ProvideResult<IReadOnlyList<IpAddress>> Analyze(
        ProvideResult<IReadOnlyList<IpAddressRawModel>> result)
    {
        var warnings = new List<string>(result.Warnings);

        if (result.Data is not { Count: > 0 } rawData)
        {
            return ProvideResult<IReadOnlyList<IpAddress>>.Fail(
                warnings,
                result.CriticalErrors);
        }

        var (
            ipAddresses,
            unknownAddressCount, unknownSubnetCount, unknownInterfaceCount) = BuildIpList(rawData);

        AppendCountWarnings(
            warnings,
            logger,
            unknownAddressCount,
            unknownSubnetCount,
            unknownInterfaceCount,
            rawData.Count);

        return ProvideResult<IReadOnlyList<IpAddress>>.Ok(ipAddresses, warnings);
    }

    private static (
        List<IpAddress> IpAddresses,
        int UnknownAddressCount, int UnknownSubnetCount, int UnknownInterfaceCount)
        BuildIpList(IReadOnlyList<IpAddressRawModel> rawData)
    {
        var ipAddresses = new List<IpAddress>();
        int unknownAddressCount = 0;
        int unknownSubnetCount = 0;
        int unknownInterfaceCount = 0;

        foreach (var model in rawData)
        {
            string ipAddress = model.Address ?? "Неизвестно";
            string ipSubnet = model.Subnet ?? "Неизвестно";
            string ipInterface = model.Interface ?? "Неизвестно";

            if (model.Address is null)
            {
                unknownAddressCount++;
            }

            if (model.Subnet is null)
            {
                unknownSubnetCount++;
            }

            if (model.Interface is null)
            {
                unknownInterfaceCount++;
            }

            ipAddresses.Add(new IpAddress(ipAddress, ipSubnet, ipInterface));
        }

        return (ipAddresses, unknownAddressCount, unknownSubnetCount, unknownInterfaceCount);
    }

    private static void AppendCountWarnings(
        List<string> warnings,
        ILogger logger,
        int unknownAddressCount,
        int unknownSubnetCount,
        int unknownInterfaceCount,
        int total)
    {
        if (unknownAddressCount > 0)
        {
            warnings.Add($"{UNKNOWN_ADDRESS} {CountOfTotal(unknownAddressCount, total)}");

            logger.Warning(
                UNKNOWN_ADDRESS + " Затронуто {Count} из {Total} IP-адресов.",
                unknownAddressCount, total);
        }
        if (unknownSubnetCount > 0)
        {
            warnings.Add($"{UNKNOWN_SUBNET} {CountOfTotal(unknownSubnetCount, total)}");

            logger.Warning(
                UNKNOWN_SUBNET + " Затронуто {Count} из {Total} IP-адресов.",
                unknownSubnetCount, total);
        }
        if (unknownInterfaceCount > 0)
        {
            warnings.Add($"{UNKNOWN_INTERFACE} {CountOfTotal(unknownInterfaceCount, total)}");

            logger.Warning(
                UNKNOWN_INTERFACE + " Затронуто {Count} из {Total} IP-адресов.",
                unknownInterfaceCount, total);
        }
    }
}