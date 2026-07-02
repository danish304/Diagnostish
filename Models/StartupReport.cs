namespace Diagnostish.Models
{
    public class StartupReport
    {
        public string Name { get; set; } = "Unknown";       // Название программы
        public string Command { get; set; } = "Unknown";    // Путь к файлу / команда запуска
    }
}
