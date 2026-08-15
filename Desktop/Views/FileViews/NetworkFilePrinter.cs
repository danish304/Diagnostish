using Desktop.Views.FileViews.Common;

namespace Desktop.Views.FileViews;

public class NetworkFilePrinter : BaseFilePrinter<NetworkReport>
{
    public NetworkFilePrinter(CommonReportFile reportFile) : base(reportFile)
    {
    }

    protected override void PrintReport(NetworkReport report)
    {
        Writer.WriteLine("\nКОНФИГУРАЦИЯ СЕТИ:");

        Writer.Write("\n1) Сетевые адаптеры: ");
        if (report.NetworkAdapters.Count > 0)
        {
            Writer.WriteLine();
            foreach (var networkAdapter in report.NetworkAdapters)
            {
                Writer.WriteLine(
                    $"    - {networkAdapter.Description} ({networkAdapter.MacAddress}), " +
                    $"cтатус DHCP: {networkAdapter.DhcpEnabled}");
            }
        }
        else
        {
            Writer.WriteLine("Данные не получены.");
        }

        Writer.Write("2) IP-адреса: ");
        if (report.IpAddresses.Count > 0)
        {
            Writer.WriteLine();
            foreach (var ipAddress in report.IpAddresses)
            {
                Writer.WriteLine(
                    $"    - {ipAddress.Address} ({ipAddress.Interface}), " +
                    $"маска: {ipAddress.Subnet}");
            }
        }
        else
        {
            Writer.WriteLine("Данные не получены.");
        }

        Writer.Write("3) Шлюзы: ");
        if (report.Gateways.Count > 0)
        {
            Writer.WriteLine();
            foreach (var gateway in report.Gateways)
            {
                Writer.WriteLine($"    - {gateway.Address} ({gateway.Interface})");
            }
        }
        else
        {
            Writer.WriteLine("Данные не получены.");
        }

        Writer.Write("4) DNS: ");
        if (report.DnsAddresses.Count > 0)
        {
            Writer.WriteLine();
            foreach (var dns in report.DnsAddresses)
            {
                Writer.WriteLine($"    - {dns.Address} ({dns.Interface})");
            }
        }
        else
        {
            Writer.WriteLine("Данные не получены.");
        }

        PrintIssues(report.Warnings, report.CriticalErrors);
    }
}