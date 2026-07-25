using Diagnostish.Domain.Common;

namespace Diagnostish.Domain.Interfaces;

public interface IReportMapper<TReport, TInfo> 
{
    void MapInto(TReport report, ProvideResult<TInfo> data);
}