using Diagnostish.Models;
using Diagnostish.Services;
using Diagnostish.Views;

namespace Diagnostish.Controllers
{
    public class DiagnosticController
    {
        private readonly IHWCheck _hwCheck;
        private readonly IOSCheck _osCheck;
        private readonly IPrintHW _printHw;
        private readonly IPrintOS _printOs;
        private readonly IUserInterface _ui;

        public DiagnosticController(IHWCheck hwCheck, IOSCheck osCheck, IPrintHW printHw, IPrintOS printOs, IUserInterface ui)
        {
            _hwCheck = hwCheck;
            _osCheck = osCheck;
            _printHw = printHw;
            _printOs = printOs;
            _ui = ui;
        }

        public void StartDiagnostic()
        {
            _ui.Preview();

            HWReport hwReport = _hwCheck.CheckPCCFG();
            _printHw.PrintHardware(hwReport);

            OSReport osReport = _osCheck.CheckOSCFG();
            _printOs.PrintOperationSystem(osReport);

            _ui.WaitForExit();
        }
    }
}
