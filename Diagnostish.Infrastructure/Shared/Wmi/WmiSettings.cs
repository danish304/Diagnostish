namespace Diagnostish.Infrastructure.Shared.Wmi;

public class WmiSettings
{
    public int WmiQueryTimeoutSeconds { get; set; } = 5;
    public TimeSpan WmiQueryTimeout => 
        TimeSpan.FromSeconds(WmiQueryTimeoutSeconds);
}