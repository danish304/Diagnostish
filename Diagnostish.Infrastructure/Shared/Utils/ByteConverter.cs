namespace Diagnostish.Infrastructure.Shared.Utils;

public static class ByteConverter
{
    private const double BytesInGigabyte = 1024d * 1024 * 1024;

    public static double ToGigabytes(ulong bytes, int decimals = 2) => Math.Round(bytes / BytesInGigabyte, decimals);

    public static double ToGigabytes(double bytes, int decimals = 2) => Math.Round(bytes / BytesInGigabyte, decimals);
}