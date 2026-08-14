using Domain.Common;

namespace Domain.Interfaces;

public interface IProvider<TRawModel>
{
    Task<ProvideResult<IReadOnlyList<TRawModel>>> ProvideAsync(
        CancellationToken cancellationToken = default);
}