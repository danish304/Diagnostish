using Application.Services;
using Desktop.Views;

namespace Desktop.Controllers;

public class DiagnosticController
{
    private readonly FinalReportComposer _composer;
    private readonly FinalReportPrintDispatcher _printDispatcher;
    private readonly UserInterfaceDispatcher _uiDispatcher;

    public DiagnosticController(
        FinalReportComposer composer,
        FinalReportPrintDispatcher printDispatcher,
        UserInterfaceDispatcher uiDispatcher)
    {
        _composer = composer;
        _printDispatcher = printDispatcher;
        _uiDispatcher = uiDispatcher;
    }

    public async Task StartDiagnosticAsync(CancellationToken cancellationToken = default)
    {
        _uiDispatcher.ShowWelcomes();

        var finalReport = await _composer.GetFinalReportAsync(cancellationToken);

        _printDispatcher.PrintAllReports(finalReport);

        _uiDispatcher.ShowCompletions();
    }
}