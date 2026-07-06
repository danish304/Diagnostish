namespace Diagnostish.Helpers
{
    public static class WMISettings
    {
        public static readonly TimeSpan RequestTimeout = TimeSpan.FromSeconds(5);
        public const long BytesInGigabyte = 1024L * 1024L * 1024L;
    }
}
