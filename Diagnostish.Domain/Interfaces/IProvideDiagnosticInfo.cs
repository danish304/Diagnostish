using Diagnostish.Domain.Common;

namespace Diagnostish.Domain.Interfaces;

public interface IProvideDiagnosticInfo<TRawInfo>
{
    ProvideResult <IReadOnlyList<TRawInfo>> ProvideInfo();
}