using Diagnostish.Controllers;
using Diagnostish.Services;
using Diagnostish.Views;

static class Program
{
    static void Main(string[] args)
    {
        CheckConfigurationWMI wmiService = new CheckConfigurationWMI();
        IHWCheck hwService = wmiService;
        IOSCheck osService = wmiService;

        PrintToConsole consoleView = new PrintToConsole();
        IPrintHW hwView = consoleView;
        IPrintOS osView = consoleView;

        DiagnosticController controller = new DiagnosticController(hwService, osService, hwView, osView);

        controller.StartDiagnostic();
    }
}