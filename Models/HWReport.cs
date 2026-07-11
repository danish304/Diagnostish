namespace Diagnostish.Models
{
    public class HWReport : IssuesReport
    {
        public string ProcessorName { get; set; } = "Unknown";                  // Название процессора
        public int CoresCount { get; set; }                                     // Количество ядер процессора
        public int CurrentClockSpeed { get; set; }                              // Базовая частота процессора   

        public double RAMSize { get; set; }                                     // Общее количество ОЗУ
        public int RAMSpeed { get; set; }                                       // Частота ОЗУ
        public string RAMType { get; set; } = "Unknown";                        // Тип ОЗУ

        public List<string> VideoCards { get; } = [];                           // Названия видеокарт

        public List<string> ModelsDrives { get; } = [];                         // Модели накопителей
        public List<double> DrivesSize { get; } = [];                           // Объемы накопителей
    }
}
