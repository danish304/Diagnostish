using Domain.Models.Entities.Hardware;
using Domain.Models.Reports.Common;

namespace Domain.Models.Reports.Components;

public class HardwareReport : BaseIssuesReport
{
    // Процессор
    public string CpuName { get; set; } = "Неизвестно";
    public int CpuCores { get; set; }
    public int CpuClockSpeed { get; set; }

    // Оперативная память
    public string RamType { get; set; } = "Неизвестно";
    public double RamCapacity { get; set; }
    public int RamSpeed { get; set; }

    // Компоненты коллекции
    public IReadOnlyList<Gpu> VideoCards { get; set; } = [];
    public IReadOnlyList<StorageDrive> StorageDrives { get; set; } = [];

    // Материнская плата
    public string BaseBoardModel { get; set; } = "Неизвестно";
    public string BaseBoardManufacturer { get; set; } = "Неизвестно";
    public string BaseBoardVersion { get; set; } = "Неизвестно";
    public string BaseBoardStatus { get; set; } = "Неизвестно";

    // BIOS
    public string BiosVersion { get; set; } = "Неизвестно";
    public DateTime BiosReleaseDate { get; set; }
    public string BiosManufacturer { get; set; } = "Неизвестно";
}