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

        public DiagnosticController(IHWCheck hwCheck, IOSCheck osCheck, IPrintHW printHw, IPrintOS printOs)
        {
            _hwCheck = hwCheck;
            _osCheck = osCheck;
            _printHw = printHw;
            _printOs = printOs;
        }

        public void StartDiagnostic()
        {
            Preview();

            HWReport hwReport = _hwCheck.CheckPCCFG();
            _printHw.PrintHardware(hwReport);

            OSReport osReport = _osCheck.CheckOSCFG();
            _printOs.PrintOperationSystem(osReport);

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\nДля завершения нажмите любую клавишу . . .");
            Console.ResetColor();
            Console.ReadKey();
        }

        private void Preview()
        {
            Console.Title = "Diagnostish";
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("ЗАПУСК ДИАГНОСТИКИ . . .");
            Console.ResetColor();
        }
    }
}
