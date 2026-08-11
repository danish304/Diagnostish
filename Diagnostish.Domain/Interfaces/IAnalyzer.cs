using Diagnostish.Domain.Common;

namespace Diagnostish.Domain.Interfaces;

public interface IAnalyzer<TRawData, TData>
{
    ProvideResult<TData> Analyze(
        ProvideResult<IReadOnlyList<TRawData>> result);
}