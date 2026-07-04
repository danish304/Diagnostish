using Diagnostish.Controllers;
using Diagnostish.Services;
using Diagnostish.Views;

static class Program
{
    static void Main(string[] args)
    {
        CheckPCConfigurationWMI wmiServiceHW = new CheckPCConfigurationWMI();
        IHWCheck hwService = wmiServiceHW;

        CheckOSConfigurationWMI wmiServiceOS = new CheckOSConfigurationWMI();
        IOSCheck osService = wmiServiceOS;

        PrintToConsole consoleView = new PrintToConsole();
        IPrintHW hwView = consoleView;
        IPrintOS osView = consoleView;
        IUserInterface UI = consoleView;

        DiagnosticController controller = new DiagnosticController(hwService, osService, hwView, osView, UI);

        controller.StartDiagnostic();
    }
}