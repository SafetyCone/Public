using System;
using Public.Common.Lib;


namespace Public.Common.Augustus
{
    /// <summary>
    /// The operating system environmnet for the build file.
    /// </summary>
    /// <remarks>
    /// Visual Studio solution files must be labeled with 'Windows', while make files must be labeled with 'Cygwin'.
    /// This code does not currently work for non-Windows operating system environments.
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


namespace Public.Common.Augustus.Extensions
{
    public static class OsEnvironmentExtensions
    {
        public const string Windows = @"Windows";
        public const string Cygwin = @"Cygwin";


        public static string ToDefaultString(this OsEnvironment osEnvironment)
        {
            string output;
            switch(osEnvironment)
            {
                case OsEnvironment.Cygwin:
                    output = OsEnvironmentExtensions.Cygwin;
                    break;

                case OsEnvironment.Windows:
                    output = OsEnvironmentExtensions.Windows;
                    break;

                default:
                    throw new UnexpectedEnumerationValueException<OsEnvironment>(osEnvironment);
            }

            return output;
        }

        public static OsEnvironment FromDefault(string osEnvironment)
        {
            OsEnvironment output;
            if (!OsEnvironmentExtensions.TryFromDefault(osEnvironment, out output))
            {
                throw new ArgumentException(@"Unrecognized OS environment string.", "osEnvironment");
            }

            return output;
        }

        public static bool TryFromDefault(string osEnvironment, out OsEnvironment value)
        {
            bool output = true;
            switch(osEnvironment)
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
