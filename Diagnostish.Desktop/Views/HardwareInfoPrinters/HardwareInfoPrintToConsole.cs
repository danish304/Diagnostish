using Diagnostish.Desktop.Views.Helpers;
using Diagnostish.Domain.Models.Reports;

namespace Diagnostish.Desktop.Views.HardwareInfoPrinters
{
    public class HardwareInfoPrintToConsole : ReportPrinter<HardwareReport>
    {
        protected override void PrintReport(HardwareReport hardwareReport)
        {
            ColorPrinter.WriteLineColored("\nКОНФИГУРАЦИЯ ПК:", ConsoleColor.Cyan);
            Console.WriteLine($"\n1) Процессор: {hardwareReport.CpuName} ({hardwareReport.CpuCores} ядер), частота - {hardwareReport.CpuClockspeed} MHz");
            Console.WriteLine($"2) ОЗУ: {hardwareReport.RamType} {hardwareReport.RamSize} GB, {hardwareReport.RamSpeed} MHz");

            Console.WriteLine("3) Видеокарты:");
            foreach (var gpu in hardwareReport.Gpus)
            {
                Console.WriteLine($"    - {gpu.Name} ({gpu.SizeGB} GB)");
            }

            Console.WriteLine("4) Накопители:");
            foreach (var drive in hardwareReport.Drives)
            {
                Console.WriteLine($"    - {drive.Name} ({drive.SizeGB} GB)");
            }

            Console.WriteLine($"5) Материнская плата: {hardwareReport.BaseboardModel} ({hardwareReport.BaseboardManufacturer}), версия {hardwareReport.BaseboardVersion}");
            Console.WriteLine($"   Статус платы: {hardwareReport.BaseboardStatus}");

            Console.WriteLine($"6) BIOS: {hardwareReport.BiosVersion} ({hardwareReport.BiosManufacturer}), дата релиза - {hardwareReport.BiosReleaseDate}");

            PrintIssues(hardwareReport.Errors, hardwareReport.CriticalErrors);
        }
    }
}
