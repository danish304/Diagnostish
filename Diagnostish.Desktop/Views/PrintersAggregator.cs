namespace Diagnostish.Desktop.Views;

public class PrintersAggregator
{
    private readonly IEnumerable<IReportPrinter<HardwareReport>> _hardwarePrinters;
    private readonly IEnumerable<IReportPrinter<OperatingSystemReport>> _operatingSystemPrinters;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0290:Use primary constructor", Justification = "<Pending>")]
    public PrintersAggregator(IEnumerable<IReportPrinter<HardwareReport>> hardwarePrinters,
                              IEnumerable<IReportPrinter<OperatingSystemReport>> operatingSystemPrinters)
    {
        _hardwarePrinters = hardwarePrinters;
        _operatingSystemPrinters = operatingSystemPrinters;
    }

    public void PrintAllReports(FinalReport finalReport)
    {
        foreach (var printer in _hardwarePrinters) printer.Print(finalReport.HardwareReport);

        foreach (var printer in _operatingSystemPrinters) printer.Print(finalReport.OperatingSystemReport);
    }
}