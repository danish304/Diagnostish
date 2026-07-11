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

        public List<string> VideoCards { get; } = [];                           // Название видеокарты
        public List<double> AdaptersRAM { get; } = [];                          // Объем видеопамяти

        public List<string> ModelsDrives { get; } = [];                         // Модель накопителя
        public List<double> DrivesSize { get; } = [];                           // Объем накопителя

        public string BaseBoardModel { get; set; } = "Unknown";                 // Модель основной платы
        public string BaseBoardManufacturer { get; set; } = "Unknown";          // Производитель основной платы
        public string BaseBoardVersion { get; set; } = "Unknown";               // Версия основной платы
        public string BaseBoardStatus { get; set; } = "Unknown";                // Статус состояния основной платы

        public string BIOSVersion { get; set; } = "Unknown";                    // Версия BIOS
        public DateTime? BIOSReleaseDate { get; set; }                          // Дата релиза BIOS      
        public string BIOSManufacturer { get; set; } = "Unknown";               // Производитель BIOS
    }
}
