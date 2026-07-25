namespace Diagnostish.Domain.Interfaces;

public interface IReportPrinter<TReport>
{
    void Print(TReport report);
}