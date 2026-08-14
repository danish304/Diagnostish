using Desktop.Views.ConsoleViews.Common;

namespace Desktop.Views.ConsoleViews;

public class NetworkConsolePrinter : BaseConsolePrinter<NetworkReport>
{
    protected override void PrintReport(NetworkReport report)
    {
        ColorPrinter.WriteLineColored("\nКОНФИГУРАЦИЯ СЕТИ:", ConsoleColor.Cyan);

        Console.WriteLine("\n1) Сетевые адаптеры: ");
        if (report.NetworkAdapters.Count > 0)
        {
            foreach (var networkAdapter in report.NetworkAdapters)
            {
                Console.WriteLine(
                    $"    - {networkAdapter.Description} ({networkAdapter.MacAddress}), " +
                    $"cтатус DHCP: {networkAdapter.DhcpEnabled}");
            }
        }
        else
        {
            Console.Write("Данные не получены.");
        }

        Console.WriteLine("2) IP-адреса: ");
        if (report.IpAddresses.Count > 0)
        {
            foreach (var ipAddress in report.IpAddresses)
            {
                Console.WriteLine(
                    $"    - {ipAddress.Address} ({ipAddress.Interface}), " +
                    $"маска: {ipAddress.Subnet}");
            }
        }
        else
        {
            Console.Write("Данные не получены.");
        }

        Console.WriteLine("3) Шлюзы: ");
        if (report.Gateways.Count > 0)
        {
            foreach (var gateway in report.Gateways)
            {
                Console.WriteLine($"    - {gateway.Address} ({gateway.Interface})");
            }
        }
        else
        {
            Console.Write("Данные не получены.");
        }

        Console.WriteLine("4) DNS: ");
        if (report.DnsAddresses.Count > 0)
        {
            foreach (var dns in report.DnsAddresses)
            {
                Console.WriteLine($"    - {dns.Address} ({dns.Interface})");
            }
        }
        else
        {
            Console.Write("Данные не получены.");
        }

        PrintIssues(report.Warnings, report.CriticalErrors);
    }
}