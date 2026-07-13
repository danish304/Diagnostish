namespace Diagnostish.Models
{
    public class HWReport : IssuesReport
    {
        public string ProcessorName { get; set; } = "Unknown";                  // Название процессора
        public int CoresCount { get; set; }                                     // Количество ядер процессора
        public int CurrentClockSpeed { get; set; }                              // Базовая частота процессора   

        public string RAMType { get; set; } = "Unknown";                        // Тип ОЗУ
        public double RAMSize { get; set; }                                     // Объем ОЗУ
        public int RAMSpeed { get; set; }                                       // Частота ОЗУ

        public record ComponentInfo(string Name, double SizeGB);
        public List<ComponentInfo> VideoCards { get; } = [];                    // Название видеокарты и объем видеопамяти
        public List<ComponentInfo> Drives { get; } = [];                        // Модель накопителя и объем накопителя

        public string BaseBoardModel { get; set; } = "Unknown";                 // Модель основной платы
        public string BaseBoardManufacturer { get; set; } = "Unknown";          // Производитель основной платы
        public string BaseBoardVersion { get; set; } = "Unknown";               // Версия основной платы
        public string BaseBoardStatus { get; set; } = "Unknown";                // Статус состояния основной платы

        public string BIOSVersion { get; set; } = "Unknown";                    // Версия BIOS
        public DateTime? BIOSReleaseDate { get; set; }                          // Дата релиза BIOS      
        public string BIOSManufacturer { get; set; } = "Unknown";               // Производитель BIOS
    }
}
