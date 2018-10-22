using SemVersion;

namespace Automatize.Extensions
{
    public static class StringExtentions
    {
        public static SemanticVersion ToSemanticVersion(this string version)
        {
            if (version is null)
                return null;

            var versionParts = version.Split(".");

            return new SemanticVersion(
                int.Parse(versionParts[0]),
                int.Parse(versionParts[1]),
                int.Parse(versionParts[2]));
        }
    }
}