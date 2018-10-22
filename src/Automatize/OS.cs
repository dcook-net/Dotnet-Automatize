using System.Runtime.InteropServices;

namespace Automatize
{
    public static class OS
    {
        public static string Slash => RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? @"\" : @"/";
    }
}