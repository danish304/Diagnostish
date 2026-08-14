namespace Infrastructure.Shared.Common.Utils;

public static class ByteConverter
{
    private const double BYTES_IN_GIGABYTES = 1024d * 1024 * 1024;

    public static double ToGigabytes(ulong bytes, int decimals = 2) =>
        Math.Round(bytes / BYTES_IN_GIGABYTES, decimals);

    public static double ToGigabytes(double bytes, int decimals = 2) =>
        Math.Round(bytes / BYTES_IN_GIGABYTES, decimals);
}