using Domain.Common;

namespace Domain.Interfaces;

public interface IReportMapper<TReport, TData>
{
    void MapInto(
        TReport report,
        ProvideResult<TData> result);
}