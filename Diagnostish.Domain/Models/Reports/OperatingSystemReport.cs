namespace Diagnostish.Domain.Models.Reports
{
    public class OperatingSystemReport : IssuesReport
    {
        public string OperatingSystemName { get; set; } = "Unknown";                           // Имя ОС (сборка)
        public string OperatingSystemManufacturer { get; set; } = "Unknown";                   // Производитель ОС
        public string OperatingSystemVersion { get; set; } = "Unknown";                        // Версия ОС
        public DateTime? OperatingSystemInstallDate { get; set; }                              // Дата установки ОС
        public string OperatingSystemRegisteredUser { get; set; } = "Unknown";                 // Зарегистрированный пользователь ОС
        public DateTime? OperatingSystemLastBootUpTime { get; set; }                           // Время последнего включения ОС
    }
}
