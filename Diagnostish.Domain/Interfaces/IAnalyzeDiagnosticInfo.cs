using Diagnostish.Domain.Common;

namespace Diagnostish.Domain.Interfaces;

public interface IAnalyzeDiagnosticInfo<TRawInfo, TInfo>
{
    ProvideResult<TInfo> AnalyzeInfo(ProvideResult<IReadOnlyList<TRawInfo>> provided);
}