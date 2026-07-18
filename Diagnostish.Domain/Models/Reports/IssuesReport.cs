namespace Diagnostish.Domain.Models.Reports
{
    public abstract class IssuesReport
    {
        public List<string> Errors { get; init; } = [];               // Некритичные ошибки/предупреждения 
        public List<string> CriticalErrors { get; init; } = [];       // Критические ошибки 
    }
}
