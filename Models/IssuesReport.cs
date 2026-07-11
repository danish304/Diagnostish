namespace Diagnostish.Models
{
    public abstract class IssuesReport
    {
        public List<string> Errors { get; } = [];               // Некритичные ошибки/предупреждения 
        public List<string> CriticalErrors { get; } = [];       // Критические ошибки 
    }
}
