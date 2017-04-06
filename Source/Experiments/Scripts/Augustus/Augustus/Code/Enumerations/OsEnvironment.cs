using System;
using Public.Common.Lib;


namespace Augustus
{
    /// <summary>
    /// The operating system environmnet for the build file.
    /// </summary>
    /// <remarks>
    /// Visual Studio solution files must be labeled with 'Windows', while make files must be labeled with 'Cygwin'.
    /// This code does not currently work for non-Windows environments.
    /// </remarks>
    public enum OsEnvironment
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
    public static class OsEnvironmentExtensions
    {
        public const string Windows = @"Windows";
        public const string Cygwin = @"Cygwin";


        public static string ToDefaultString(this OsEnvironment platform)
        {
            string output;
            switch(platform)
            {
                case OsEnvironment.Cygwin:
                    output = OsEnvironmentExtensions.Cygwin;
                    break;

                case OsEnvironment.Windows:
                    output = OsEnvironmentExtensions.Windows;
                    break;

                default:
                    throw new UnexpectedEnumerationValueException<OsEnvironment>(platform);
            }

            return output;
        }

        public static OsEnvironment FromDefault(string platform)
        {
            OsEnvironment output;
            if (!OsEnvironmentExtensions.TryFromDefault(platform, out output))
            {
                throw new ArgumentException(@"Unrecognized build platform string.", nameof(platform));
            }

            return output;
        }

        public static bool TryFromDefault(string platform, out OsEnvironment value)
        {
            bool output = true;
            switch(platform)
            {
                case OsEnvironmentExtensions.Cygwin:
                    value = OsEnvironment.Cygwin;
                    break;

                case OsEnvironmentExtensions.Windows:
                    value = OsEnvironment.Windows;
                    break;
                
                default:
                    output = false;
                    value = OsEnvironment.Windows;
                    break;
            }

            return output;
        }
    }
}
