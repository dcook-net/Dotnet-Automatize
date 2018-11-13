using System;

namespace Automatize.Version
{
    public class UnsupportedVersionException : Exception
    {
        public UnsupportedVersionException(int major, int minor) 
            : base ($"Unsupported Version: {major}.{minor}")
        {}
    }
}