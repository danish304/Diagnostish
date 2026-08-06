using Microsoft.Win32;

namespace Diagnostish.Infrastructure.Shared.Registry.Executor;

public interface IExecutorRegistry
{
    Task ExecuteSafeReadAsync(string rootPath, string context,
                              List<string> warnings, List<string> criticalErrors,
                              Action<RegistryKey> registryAction,
                              CancellationToken cancellationToken = default);
}