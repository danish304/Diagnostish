namespace Desktop.Views;

public class FinalReportPrintDispatcher
{
    private readonly IEnumerable<IReportPrinter<HardwareReport>> _hardwarePrinters;
    private readonly IEnumerable<IReportPrinter<OperatingSystemReport>> _operatingSystemPrinters;
    private readonly IEnumerable<IReportPrinter<NetworkReport>> _networkPrinters;

    public FinalReportPrintDispatcher(
        IEnumerable<IReportPrinter<HardwareReport>> hardwarePrinters,
        IEnumerable<IReportPrinter<OperatingSystemReport>> operatingSystemPrinters,
        IEnumerable<IReportPrinter<NetworkReport>> networkPrinters)
    {
        _hardwarePrinters = hardwarePrinters;
        _operatingSystemPrinters = operatingSystemPrinters;
        _networkPrinters = networkPrinters;
    }

    public void PrintAllReports(FinalReport report)
    {
        foreach (var printer in _hardwarePrinters)
        {
            printer.Print(report.HardwareReport);
        }

        foreach (var printer in _operatingSystemPrinters)
        {
            printer.Print(report.OperatingSystemReport);
        }

        foreach (var printer in _networkPrinters)
        {
            printer.Print(report.NetworkReport);
        }
    }
}