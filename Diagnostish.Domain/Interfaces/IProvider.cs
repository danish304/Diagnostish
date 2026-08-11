using Diagnostish.Domain.Common;

namespace Diagnostish.Domain.Interfaces;

public interface IProvider<TRawData>
{
    Task<ProvideResult<IReadOnlyList<TRawData>>> ProvideAsync(
        CancellationToken cancellationToken = default);
}