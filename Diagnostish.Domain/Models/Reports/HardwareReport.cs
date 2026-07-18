namespace Diagnostish.Domain.Models.Reports
{
    public class HardwareReport : IssuesReport
    {
        public string CpuName { get; set; } = "Unknown";                        // Название процессора
        public int CpuCores { get; set; }                                       // Количество ядер процессора
        public int CpuClockspeed { get; set; }                                  // Базовая частота процессора   

        public string RamType { get; set; } = "Unknown";                        // Тип ОЗУ
        public double RamSize { get; set; }                                     // Объем ОЗУ
        public int RamSpeed { get; set; }                                       // Частота ОЗУ

        public record ComponentInfo(string Name, double SizeGB);
        public List<ComponentInfo> Gpus { get; init; } = [];                    // Название видеокарты и объем видеопамяти
        public List<ComponentInfo> Drives { get; init; } = [];                  // Модель накопителя и объем накопителя

        public string BaseboardModel { get; set; } = "Unknown";                 // Модель основной платы
        public string BaseboardManufacturer { get; set; } = "Unknown";          // Производитель основной платы
        public string BaseboardVersion { get; set; } = "Unknown";               // Версия основной платы
        public string BaseboardStatus { get; set; } = "Unknown";                // Статус состояния основной платы

        public string BiosVersion { get; set; } = "Unknown";                    // Версия BIOS
        public DateTime? BiosReleaseDate { get; set; }                          // Дата релиза BIOS      
        public string BiosManufacturer { get; set; } = "Unknown";               // Производитель BIOS
    }
}
