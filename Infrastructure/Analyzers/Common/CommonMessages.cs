namespace Infrastructure.Analyzers.Common;

internal static class CommonMessages
{
    internal static string CountOfTotal(int count, int total)
    {
        return $"({count} из {total})";
    }
}