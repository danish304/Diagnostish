using Diagnostish.Domain.Common;

namespace Diagnostish.Domain.Interfaces;

public interface IProvider<TRawModel>
{
    Task<ProvideResult<IReadOnlyList<TRawModel>>> ProvideAsync(
        CancellationToken cancellationToken = default);
}