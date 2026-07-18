using Diagnostish.Application.Services;
using Diagnostish.Desktop.Views;
using Diagnostish.Domain.Interfaces;
using Diagnostish.Domain.Models.Reports;

namespace Diagnostish.Desktop.Controllers
{
    public class DiagnosticController(ServicesAggregator servicesAggregator,
                                      IReportPrinter<HardwareReport> hardwarePrinter,
                                      IReportPrinter<OperatingSystemReport> operatingSystemPrinter,
                                      IUserInterface userInterface)
    {
        public void StartDiagnostic()
        {
            userInterface.ShowWelcome();

            var finalReport = servicesAggregator.GetFinalReport();

            hardwarePrinter.Print(finalReport.HardwareReport);
            operatingSystemPrinter.Print(finalReport.OperatingSystemReport);

            userInterface.WaitForExit();
        }
    }
}
