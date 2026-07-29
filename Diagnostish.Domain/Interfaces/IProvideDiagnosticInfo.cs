using Diagnostish.Domain.Common;

namespace Diagnostish.Domain.Interfaces;

public interface IProvideDiagnosticInfo<TRawInfo>
{
    Task<ProvideResult<IReadOnlyList<TRawInfo>>> ProvideInfoAsync(CancellationToken cancellationToken = default);
}