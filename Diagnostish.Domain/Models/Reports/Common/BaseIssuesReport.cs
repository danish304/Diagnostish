namespace Diagnostish.Domain.Models.Reports.Common;

public abstract class BaseIssuesReport
{
    private readonly List<string> _warnings = [];
    private readonly List<string> _criticalErrors = [];

    public IReadOnlyList<string> Warnings => _warnings;
    public IReadOnlyList<string> CriticalErrors => _criticalErrors;

    public void AddWarnings(IEnumerable<string> warnings) =>
        _warnings.AddRange(warnings);

    public void AddCriticalErrors(IEnumerable<string> criticalErrors) =>
        _criticalErrors.AddRange(criticalErrors);
}