using Diagnostish.Models;

namespace Diagnostish.Services.Interfaces
{
    public interface IOSCheck
    {
        OSReport CheckOSCFG();
    }
}
