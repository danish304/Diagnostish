using Domain.Common;

namespace Domain.Interfaces;

public interface IAnalyzer<TRawModel, TData>
{
    ProvideResult<TData> Analyze(
        ProvideResult<IReadOnlyList<TRawModel>> result);
}