namespace Desktop.Views.Common.Printers;

public class NetworkPrinter(
    TextWriter writer,
    ILineWriter? lineWriter = null)
    : BasePrinter<NetworkReport>(writer, lineWriter)
{
    protected override void PrintReport(NetworkReport report)
    {
        WriteHeader("\nКОНФИГУРАЦИЯ СЕТИ:\n", ConsoleColor.Cyan);

        reportComponentIndex = 1;

        PrintNetworkAdapters(report);
        PrintIpAddresses(report);
        PrintGateways(report);
        PrintDnsAddresses(report);

        PrintIssues(report.Warnings, report.CriticalErrors);
    }

    private void PrintNetworkAdapters(NetworkReport report)
    {
        writer.Write($"{reportComponentIndex++}) Сетевые адаптеры: ");
        if (report.NetworkAdapters.Count > 0)
        {
            writer.WriteLine();
            foreach (var networkAdapter in report.NetworkAdapters)
            {
                writer.WriteLine(
                    $"    - {networkAdapter.Description} ({networkAdapter.MacAddress}), " +
                    $"cтатус DHCP: {networkAdapter.DhcpEnabled}");
            }
        }
        else
        {
            writer.WriteLine("Данные не получены.");
        }
    }

    private void PrintIpAddresses(NetworkReport report)
    {
        writer.Write($"{reportComponentIndex++}) IP-адреса: ");
        if (report.IpAddresses.Count > 0)
        {
            writer.WriteLine();
            foreach (var ipAddress in report.IpAddresses)
            {
                writer.WriteLine(
                    $"    - {ipAddress.Address} ({ipAddress.Interface}), " +
                    $"маска: {ipAddress.Subnet}");
            }
        }
        else
        {
            writer.WriteLine("Данные не получены.");
        }
    }

    private void PrintGateways(NetworkReport report)
    {
        writer.Write($"{reportComponentIndex++}) Шлюзы: ");
        if (report.Gateways.Count > 0)
        {
            writer.WriteLine();
            foreach (var gateway in report.Gateways)
            {
                writer.WriteLine($"    - {gateway.Address} ({gateway.Interface})");
            }
        }
        else
        {
            writer.WriteLine("Данные не получены.");
        }
    }

    private void PrintDnsAddresses(NetworkReport report)
    {
        writer.Write($"{reportComponentIndex++}) DNS: ");
        if (report.DnsAddresses.Count > 0)
        {
            writer.WriteLine();
            foreach (var dns in report.DnsAddresses)
            {
                writer.WriteLine($"    - {dns.Address} ({dns.Interface})");
            }
        }
        else
        {
            writer.WriteLine("Данные не получены.");
        }
    }
}