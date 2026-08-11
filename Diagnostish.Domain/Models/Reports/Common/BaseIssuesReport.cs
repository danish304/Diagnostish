namespace Diagnostish.Domain.Models.Reports.Common;

public abstract class BaseIssuesReport
{
    private readonly List<string> _warnings = [];
    private readonly List<string> _criticalErrors = [];

    public List<string> Warnings => _warnings;
    public List<string> CriticalErrors => _criticalErrors;
}