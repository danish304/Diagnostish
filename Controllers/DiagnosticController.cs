using Diagnostish.Services.Interfaces;
using Diagnostish.Views.Interfaces;

namespace Diagnostish.Controllers
{
    public class DiagnosticController
    {
        private readonly IEnumerable<IHWCheck> _hwCheck;
        private readonly IEnumerable<IOSCheck> _osCheck;
        private readonly IEnumerable<IPrintHW> _printHw;
        private readonly IEnumerable<IPrintOS> _printOs;
        private readonly IUserInterface _ui;

        public DiagnosticController(IEnumerable<IHWCheck> hwCheck,
                                    IEnumerable<IOSCheck> osCheck, 
                                    IEnumerable<IPrintHW> printHw,
                                    IEnumerable<IPrintOS> printOs,
                                    IUserInterface ui)
        {
            _hwCheck = hwCheck;
            _osCheck = osCheck;
            _printHw = printHw;
            _printOs = printOs;
            _ui = ui;
        }

        public void StartDiagnostic()
        {
            _ui.ShowWelcome();

            var hwReports = _hwCheck.Select(c => c.CheckPCCFG()).ToList();
            var osReports = _osCheck.Select(c => c.CheckOSCFG()).ToList();

            foreach (var report in hwReports)
            {
                foreach (var printer in _printHw)
                {
                    printer.PrintHardware(report);
                }
            }

            foreach (var report in osReports)
            {
                foreach (var printer in _printOs)
                {
                    printer.PrintOperationSystem(report);
                }
            }

            _ui.WaitForExit();
        }
    }
}
