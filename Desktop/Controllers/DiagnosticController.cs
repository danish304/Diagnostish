using Application.Services;
using Desktop.Views;

namespace Desktop.Controllers;

public class DiagnosticController
{
    private readonly FinalReportComposer _composer;
    private readonly FinalReportPrintDispatcher _printDispatcher;
    private readonly IUserInterface _userInterface;

    public DiagnosticController(
        FinalReportComposer composer,
        FinalReportPrintDispatcher printDispatcher,
        IUserInterface userInterface)
    {
        _composer = composer;
        _printDispatcher = printDispatcher;
        _userInterface = userInterface;
    }

    public async Task StartDiagnosticAsync(CancellationToken cancellationToken = default)
    {
        _userInterface.ShowWelcome();

        var finalReport = await _composer.GetFinalReportAsync(cancellationToken);

        _printDispatcher.PrintAllReports(finalReport);

        _userInterface.WaitForExit();
    }
}