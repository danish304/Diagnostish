using Diagnostish.Domain.Common;

namespace Diagnostish.Domain.Interfaces;

public interface IAnalyzer<TRawModel, TData>
{
    ProvideResult<TData> Analyze(
        ProvideResult<IReadOnlyList<TRawModel>> result);
}