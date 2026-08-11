using Diagnostish.Domain.Common;

namespace Diagnostish.Domain.Interfaces;

public interface IReportMapper<TReport, TData> 
{
    void MapInto(
        TReport report, 
        ProvideResult<TData> result);
}