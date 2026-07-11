namespace Diagnostish.Models
{
    public class OSReport : IssuesReport
    {
        public string OpSystemName { get; set; } = "Unknown";                           // Имя ОС (сборка)
        public string OpSystemManufacturer { get; set; } = "Unknown";                   // Производитель ОС
        public string OpSystemVersion { get; set; } = "Unknown";                        // Версия ОС
        public DateTime? OpSystemInstallDate { get; set; }                              // Дата установки ОС
        public string OpSystemRegisteredUser { get; set; } = "Unknown";                 // Зарегистрированный пользователь ОС
        public DateTime? OpSystemLastBootUpTime { get; set; }                           // Время последнего включения ОС
    }
}
