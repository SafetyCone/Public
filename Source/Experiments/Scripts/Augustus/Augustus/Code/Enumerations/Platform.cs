using System;
using Public.Common.Lib;


namespace Augustus
{
    /// <summary>
    /// The build platform for the build file.
    /// </summary>
    /// <remarks>
    /// Visual Studio solution files must be labeled with 'Windows', while make files must be labeled with 'Cygwin'.
    /// </remarks>
    public enum Platform
    {
        /// <summary>
        /// For Visual Studio solution files.
        /// </summary>
        Windows,
        /// <summary>
        /// For make files.
        /// </summary>
        Cygwin
    }
}


namespace Augustus.Extensions
{
    public static class PlatformExtensions
    {
        public const string Windows = @"Windows";
        public const string Cygwin = @"Cygwin";


        public static string ToDefaultString(this Platform platform)
        {
            string output;
            switch(platform)
            {
                case Platform.Cygwin:
                    output = PlatformExtensions.Cygwin;
                    break;

                case Platform.Windows:
                    output = PlatformExtensions.Windows;
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
                throw new ArgumentException(@"Unrecognized build platform string.", nameof(platform));
            }

            return output;
        }

        public static bool TryFromDefault(string platform, out Platform value)
        {
            bool output = true;
            switch(platform)
            {
                case PlatformExtensions.Cygwin:
                    value = Platform.Cygwin;
                    break;

                case PlatformExtensions.Windows:
                    value = Platform.Windows;
                    break;
                
                default:
                    output = false;
                    value = Platform.Windows;
                    break;
            }

            return output;
        }
    }
}
