using Diagnostish.Desktop.Views.Common;
using Diagnostish.Domain.Models.Reports;

namespace Diagnostish.Desktop.Views.HardwareInfoPrinters;

public class HardwareInfoPrintToConsole : BaseReportPrinter<HardwareReport>
{
    protected override void PrintReport(HardwareReport hardwareReport)
    {
        ColorPrinter.WriteLineColored("\nКОНФИГУРАЦИЯ ПК:", ConsoleColor.Cyan);

        Console.WriteLine($"\n1) Процессор: {hardwareReport.CpuName} ({hardwareReport.CpuCores} ядер), частота - {hardwareReport.CpuClockSpeed} MHz");

        Console.WriteLine($"2) ОЗУ: {hardwareReport.RamType} {hardwareReport.RamCapacity} GB, {hardwareReport.RamSpeed} MHz");

        Console.WriteLine("3) Видеокарты:");
        if (hardwareReport.VideoCards.Count > 0) 
            foreach (var gpu in hardwareReport.VideoCards) Console.WriteLine($"    - {gpu.Name} ({gpu.AdapterRam} GB)");
        else Console.WriteLine("    - Данные не получены.");

        Console.WriteLine("4) Накопители:");
        if (hardwareReport.StorageDrives.Count > 0) 
            foreach (var storageDrive in hardwareReport.StorageDrives) Console.WriteLine($"    - {storageDrive.Model} ({storageDrive.Size} GB)");
        else Console.WriteLine("    - Данные не получены.");

        Console.WriteLine($"5) Материнская плата: {hardwareReport.BaseBoardModel} ({hardwareReport.BaseBoardManufacturer}), версия {hardwareReport.BaseBoardVersion}");
        Console.WriteLine($"   Статус платы: {hardwareReport.BaseBoardStatus}");

        Console.WriteLine($"6) BIOS: {hardwareReport.BiosVersion} ({hardwareReport.BiosManufacturer}), дата релиза - {FormattingData.FormatDate(hardwareReport.BiosReleaseDate)}");

        PrintIssues(hardwareReport.Warnings, hardwareReport.CriticalErrors);
    }
}