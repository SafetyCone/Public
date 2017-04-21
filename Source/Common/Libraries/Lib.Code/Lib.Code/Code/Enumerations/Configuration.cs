using System;


namespace Public.Common.Lib.Code
{
    // Ok.
    public enum Configuration
    {
        Debug,
        Release
    }


    public static class ConfigurationExtensions
    {
        public const string Debug = @"Debug";
        public const string Release = @"Release";


        public static string ToDefaultString(this Configuration configuration)
        {
            string output;
            switch (configuration)
            {
                case Configuration.Debug:
                    output = ConfigurationExtensions.Debug;
                    break;

                case Configuration.Release:
                    output = ConfigurationExtensions.Release;
                    break;

                default:
                    throw new UnexpectedEnumerationValueException<Configuration>(configuration);
            }

            return output;
        }

        public static Configuration FromDefault(string configuration)
        {
            Configuration output;
            if (!ConfigurationExtensions.TryFromDefault(configuration, out output))
            {
#if (CSharp_6)
                throw new ArgumentException(@"Unrecognized Visual Studio version string.", nameof(output));
#else
                throw new ArgumentException(@"Unrecognized configuration string.", "output");
#endif  
            }

            return output;
        }

        public static bool TryFromDefault(string configuration, out Configuration value)
        {
            bool output = true;
            value = Configuration.Debug;

            switch (configuration)
            {
                case ConfigurationExtensions.Debug:
                    value = Configuration.Debug;
                    break;

                case ConfigurationExtensions.Release:
                    value = Configuration.Release;
                    break;

                default:
                    output = false;
                    break;
            }

            return output;
        }
    }
}
