using System;


namespace Public.Common.Lib.Code
{
    /// <summary>
    /// Hardware platform for build configurations.
    /// </summary>
    public enum Platform
    {
        x86,
        x64,
        AnyCPU,
        MixedPlatforms,
    }


    /// <summary>
    /// Non-space basic string representations.
    /// </summary>
    public static class PlatformExtensions
    {
        public const string x86 = @"x86";
        public const string x64 = @"x64";
        public const string AnyCPU = @"AnyCPU";
        public const string MixedPlatforms = @"MixedPlatforms";


        public static string ToDefaultString(this Platform platform)
        {
            string output;
            switch (platform)
            {
                case Platform.x86:
                    output = PlatformExtensions.x86;
                    break;

                case Platform.x64:
                    output = PlatformExtensions.x64;
                    break;

                case Platform.AnyCPU:
                    output = PlatformExtensions.AnyCPU;
                    break;

                case Platform.MixedPlatforms:
                    output = PlatformExtensions.MixedPlatforms;
                    break;

                default:
                    throw new UnexpectedEnumerationValueException<Platform>(platform);
            }

            return output;
        }

        public static Platform FromDefault(string platform)
        {
            Platform output;
            if (!PlatformExtensions.TryFromDefault(platform, out output))
            {
#if (CSharp_6)
                throw new ArgumentException(@"Unrecognized Visual Studio version string.", nameof(output));
#else
                throw new ArgumentException(@"Unrecognized platform string.", "output");
#endif  
            }

            return output;
        }

        public static bool TryFromDefault(string platform, out Platform value)
        {
            bool output = true;
            value = Platform.AnyCPU;

            switch (platform)
            {
                case PlatformExtensions.x86:
                    value = Platform.x86;
                    break;

                case PlatformExtensions.x64:
                    value = Platform.x64;
                    break;

                case PlatformExtensions.AnyCPU:
                    value = Platform.AnyCPU;
                    break;

                case PlatformExtensions.MixedPlatforms:
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
