using Diagnostish.Models;
using Diagnostish.Services;
using Diagnostish.Views;

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

            foreach (var check in _hwCheck)
            {
                HWReport hwReport = check.CheckPCCFG();
                foreach (var printer in _printHw)
                {
                    printer.PrintHardware(hwReport);
                }
            }

            foreach (var check in _osCheck)
            {
                OSReport osReport = check.CheckOSCFG();

                foreach (var printer in _printOs)
                {
                    printer.PrintOperationSystem(osReport);
                }
            }

            _ui.WaitForExit();
        }
    }
}
