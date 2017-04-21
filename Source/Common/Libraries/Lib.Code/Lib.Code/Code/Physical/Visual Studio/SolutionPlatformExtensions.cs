using System;


namespace Public.Common.Lib.Code.Physical
{
    /// <summary>
    /// Spaced platform string representations.
    /// </summary>
    public static class SolutionPlatformExtensions
    {
        public const string x86 = @"x86";
        public const string x64 = @"x64";
        public const string AnyCPU = @"Any CPU";
        public const string MixedPlatforms = @"Mixed Platforms";


        public static string ToDefaultString(this Platform platform)
        {
            string output;
            switch (platform)
            {
                case Platform.x86:
                    output = SolutionPlatformExtensions.x86;
                    break;

                case Platform.x64:
                    output = SolutionPlatformExtensions.x64;
                    break;

                case Platform.AnyCPU:
                    output = SolutionPlatformExtensions.AnyCPU;
                    break;

                case Platform.MixedPlatforms:
                    output = SolutionPlatformExtensions.MixedPlatforms;
                    break;

                default:
                    throw new UnexpectedEnumerationValueException<Platform>(platform);
            }

            return output;
        }

        public static Platform FromDefault(string platform)
        {
            Platform output;
            if (!SolutionPlatformExtensions.TryFromDefault(platform, out output))
            {
                throw new ArgumentException(@"Unrecognized solution platform string.");
            }

            return output;
        }

        public static bool TryFromDefault(string platform, out Platform value)
        {
            bool output = true;
            value = Platform.AnyCPU;

            switch (platform)
            {
                case SolutionPlatformExtensions.x86:
                    value = Platform.x86;
                    break;

                case SolutionPlatformExtensions.x64:
                    value = Platform.x64;
                    break;

                case SolutionPlatformExtensions.AnyCPU:
                    value = Platform.AnyCPU;
                    break;

                case SolutionPlatformExtensions.MixedPlatforms:
                    value = Platform.MixedPlatforms;
                    break;

                default:
                    output = false;
                    break;
            }

            return output;
        }
    }
}
