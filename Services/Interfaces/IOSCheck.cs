using Diagnostish.Models;

namespace Diagnostish.Services
{
    public interface IOSCheck
    {
        OSReport CheckOSCFG();
    }
}
