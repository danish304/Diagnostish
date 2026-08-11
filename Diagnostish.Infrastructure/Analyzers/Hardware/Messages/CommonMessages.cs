namespace Diagnostish.Infrastructure.Analyzers.Hardware.Messages;

internal static class CommonMessages
{
    internal static string CountOfTotal(
        int count,
        int total)
    {
        return $"(у {count} из {total})";
    }
}