namespace Diagnostish.Models
{
    public class OSReport : IssuesReport
    {
        public string Name { get; set; } = "Unknown";                           // Имя ОС (сборка)
        public string Version { get; set; } = "Unknown";                        // Номер версии
        public string Manufacturer { get; set; } = "Unknown";                   // Производитель (Microsoft Corporation)
        public string RegisteredUser { get; set; } = "Unknown";                 // Зарегистрированный пользователь
        public DateTime? InstallDate { get; set; }                              // Дата установки
        public DateTime? LastBootUpTime { get; set; }                           // Время последнего включения
    }
}
