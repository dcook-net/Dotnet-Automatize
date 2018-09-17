using System.Runtime.InteropServices;

namespace AutoUpgrade
{
    public static class OS
    {
        public static string Slash => RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? @"\" : @"/";
    }
}