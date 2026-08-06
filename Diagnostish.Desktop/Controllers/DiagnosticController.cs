using Diagnostish.Application.Services;
using Diagnostish.Desktop.Views;

namespace Diagnostish.Desktop.Controllers;

public class DiagnosticController
{
    private readonly ServicesAggregator _servicesAggregator;
    private readonly PrintersAggregator _printersAggregator;
    private readonly IUserInterface _userInterface;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0290:Use primary constructor", Justification = "<Pending>")]
    public DiagnosticController(ServicesAggregator servicesAggregator,
                                PrintersAggregator printersAggregator,
                                IUserInterface userInterface)
    {
        _servicesAggregator = servicesAggregator;
        _printersAggregator = printersAggregator;
        _userInterface = userInterface;
    }

    public async Task StartDiagnosticAsync(CancellationToken cancellationToken = default)
    {
        _userInterface.ShowWelcome();

        _servicesAggregator.ComponentsCollected += name => Console.Write(".");

        var finalReport = await _servicesAggregator.GetFinalReportAsync(cancellationToken);
        _printersAggregator.PrintAllReports(finalReport);

        _userInterface.WaitForExit();
    }
}