using System.Management;

namespace Diagnostish.Infrastructure.Shared.Wmi.Executor;

public interface IExecutorWmi
{
    Task ExecuteSafeQueryAsync(string query, string context, 
                          List<string> warnings, List<string> criticalErrors, 
                          Action<ManagementObjectCollection> wmiAction,
                          CancellationToken cancellationToken = default);
}