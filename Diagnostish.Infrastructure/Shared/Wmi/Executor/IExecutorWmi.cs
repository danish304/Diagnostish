using System.Management;

namespace Diagnostish.Infrastructure.Shared.Wmi.Executor;

public interface IExecutorWmi
{
    void ExecuteSafeQuery(string query, string context, 
                          List<string> warnings, List<string> criticalErrors, 
                          Action<ManagementObjectCollection> wmiAction);
}