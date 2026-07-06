namespace Diagnostish.Models
{
    public class HWReport
    {
        public string ProcessorName { get; set; } = "Unknown";                  // Название процессора
        public int CoresCount { get; set; }                                     // Количество ядер процессора
        public int CurrentClockSpeed { get; set; }                              // Базовая частота процессора   
        public double RAMSize { get; set; }                                     // Общее количество ОЗУ
        public int RAMSpeed { get; set; }                                       // Частота ОЗУ
        public List<string> VideoCards { get; } = new List<string>();           // Названия видеокарт
        public List<string> Drives { get; } = new List<string>();               // Накопители (название, объем)
        public List<string> Errors { get; } = new List<string>();               // Ошибки при получении данных
        public List<string> CriticalErrors { get; } = new List<string>();       // Критические ошибки при получении данных
    }
}
