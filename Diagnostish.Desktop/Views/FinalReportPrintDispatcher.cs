namespace Diagnostish.Desktop.Views;

public class FinalReportPrintDispatcher
{
    private readonly IEnumerable<IReportPrinter<HardwareReport>> _hardwarePrinters;
    private readonly IEnumerable<IReportPrinter<OperatingSystemReport>> _operatingSystemPrinters;

    public FinalReportPrintDispatcher(
        IEnumerable<IReportPrinter<HardwareReport>> hardwarePrinters,
        IEnumerable<IReportPrinter<OperatingSystemReport>> operatingSystemPrinters)
    {
        _hardwarePrinters = hardwarePrinters;
        _operatingSystemPrinters = operatingSystemPrinters;
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
    }
}