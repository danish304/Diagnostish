namespace Diagnostish.Models
{
    public abstract class IssuesReport
    {
        public List<string> Errors { get; } = new List<string>();               // Некритичные ошибки/предупреждения 
        public List<string> CriticalErrors { get; } = new List<string>();       // Критические ошибки 
    }
}
