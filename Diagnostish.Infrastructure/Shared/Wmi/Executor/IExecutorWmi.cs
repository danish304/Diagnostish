using System.Management;

namespace Diagnostish.Infrastructure.Shared.Wmi.Executor;

public interface IExecutorWmi
{
    Task ExecuteSafeQuery(string query, string context, 
                          List<string> warnings, List<string> criticalErrors, 
                          Action<ManagementObjectCollection> wmiAction,
                          CancellationToken cancellationToken = default);
}