namespace Diagnostish.Domain.Models.Reports;

public abstract class IssuesReport
{
    public List<string> Warnings { get; init; } = [];            
    public List<string> CriticalErrors { get; init; } = [];       
}