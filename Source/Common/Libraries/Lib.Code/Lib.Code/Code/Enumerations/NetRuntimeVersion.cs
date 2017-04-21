using System;


namespace Public.Common.Lib.Code
{
    /// <summary>
    /// .NET runtime versions.
    /// </summary>
    /// <remarks>
    /// This is different from the .NET Framework version. In fact, the runtime version has not changed since version 4.0.
    /// </remarks>
    public enum NetRuntimeVersion
    {
        Runtime40,
    }


    /// <summary>
    /// Non-machine, human readable runtime versions.
    /// </summary>
    public static class NetRuntimeVersionExtensions
    {
        public const string Runtime40 = @"Runtime40";


        public static string ToDefaultString(this NetRuntimeVersion netRuntimeVersion)
        {
            string output;
            switch (netRuntimeVersion)
            {
                case NetRuntimeVersion.Runtime40:
                    output = NetRuntimeVersionExtensions.Runtime40;
                    break;

                default:
                    throw new UnexpectedEnumerationValueException<NetRuntimeVersion>(netRuntimeVersion);
            }

            return output;
        }

        public static NetRuntimeVersion FromDefault(string netRuntimeVersion)
        {
            NetRuntimeVersion output;
            if (!NetRuntimeVersionExtensions.TryFromDefault(netRuntimeVersion, out output))
            {
                string message = String.Format(@"Unrecognized .NET runtime version string: {0}.", netRuntimeVersion);
                throw new ArgumentException(message);
            }

            return output;
        }

        public static bool TryFromDefault(string netRuntimeVersion, out NetRuntimeVersion value)
        {
            bool output = true;
            value = NetRuntimeVersion.Runtime40;

            switch (netRuntimeVersion)
            {
                case NetRuntimeVersionExtensions.Runtime40:
                    value = NetRuntimeVersion.Runtime40;
                    break;

                default:
                    output = false;
                    break;
            }

            return output;
        }
    }
}
