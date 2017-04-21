using System;


namespace Public.Common.Lib.Code
{
    /// <summary>
    /// .NET Framework versions.
    /// </summary>
    public enum NetFrameworkVersion
    {
        NetFramework10,
        NetFramework11,
        NetFramework20,
        NetFramework30,
        NetFramework35,
        NetFramework40,
        NetFramework45,
        NetFramework451,
        NetFramework452,
        NetFramework46,
        NetFramework461,
        NetFramework462,
        NetFramework47,
    }


    /// <summary>
    /// Non-machine human readable string representations.
    /// </summary>
    public static class NetFrameworkVersionExtensions
    {
        public const string NetFramework10 = @"NetFramework10";
        public const string NetFramework11 = @"NetFramework11";
        public const string NetFramework20 = @"NetFramework20";
        public const string NetFramework30 = @"NetFramework30";
        public const string NetFramework35 = @"NetFramework35";
        public const string NetFramework40 = @"NetFramework40";
        public const string NetFramework45 = @"NetFramework45";
        public const string NetFramework451 = @"NetFramework451";
        public const string NetFramework452 = @"NetFramework452";
        public const string NetFramework46 = @"NetFramework46";
        public const string NetFramework461 = @"NetFramework461";
        public const string NetFramework462 = @"NetFramework462";
        public const string NetFramework47 = @"NetFramework47";


        public static string ToDefaultString(this NetFrameworkVersion netFrameworkVersion)
        {
            string output;
            switch (netFrameworkVersion)
            {
                case NetFrameworkVersion.NetFramework10:
                    output = NetFrameworkVersionExtensions.NetFramework10;
                    break;

                case NetFrameworkVersion.NetFramework11:
                    output = NetFrameworkVersionExtensions.NetFramework11;
                    break;

                case NetFrameworkVersion.NetFramework20:
                    output = NetFrameworkVersionExtensions.NetFramework20;
                    break;

                case NetFrameworkVersion.NetFramework30:
                    output = NetFrameworkVersionExtensions.NetFramework30;
                    break;

                case NetFrameworkVersion.NetFramework35:
                    output = NetFrameworkVersionExtensions.NetFramework35;
                    break;

                case NetFrameworkVersion.NetFramework40:
                    output = NetFrameworkVersionExtensions.NetFramework40;
                    break;

                case NetFrameworkVersion.NetFramework45:
                    output = NetFrameworkVersionExtensions.NetFramework45;
                    break;

                case NetFrameworkVersion.NetFramework451:
                    output = NetFrameworkVersionExtensions.NetFramework451;
                    break;

                case NetFrameworkVersion.NetFramework452:
                    output = NetFrameworkVersionExtensions.NetFramework452;
                    break;

                case NetFrameworkVersion.NetFramework46:
                    output = NetFrameworkVersionExtensions.NetFramework46;
                    break;

                case NetFrameworkVersion.NetFramework461:
                    output = NetFrameworkVersionExtensions.NetFramework461;
                    break;

                case NetFrameworkVersion.NetFramework462:
                    output = NetFrameworkVersionExtensions.NetFramework462;
                    break;

                case NetFrameworkVersion.NetFramework47:
                    output = NetFrameworkVersionExtensions.NetFramework47;
                    break;

                default:
                    throw new UnexpectedEnumerationValueException<NetFrameworkVersion>(netFrameworkVersion);
            }

            return output;
        }

        public static NetFrameworkVersion FromDefault(string netFrameworkVersion)
        {
            NetFrameworkVersion output;
            if (!NetFrameworkVersionExtensions.TryFromDefault(netFrameworkVersion, out output))
            {
                string message = String.Format(@"Unrecognized .NET framework version string: {0}.", netFrameworkVersion);
                throw new ArgumentException(message);
            }

            return output;
        }

        public static bool TryFromDefault(string netFrameworkVersion, out NetFrameworkVersion value)
        {
            bool output = true;
            value = NetFrameworkVersion.NetFramework10;

            switch (netFrameworkVersion)
            {
                case NetFrameworkVersionExtensions.NetFramework10:
                    value = NetFrameworkVersion.NetFramework10;
                    break;

                case NetFrameworkVersionExtensions.NetFramework11:
                    value = NetFrameworkVersion.NetFramework11;
                    break;

                case NetFrameworkVersionExtensions.NetFramework20:
                    value = NetFrameworkVersion.NetFramework20;
                    break;

                case NetFrameworkVersionExtensions.NetFramework30:
                    value = NetFrameworkVersion.NetFramework30;
                    break;

                case NetFrameworkVersionExtensions.NetFramework35:
                    value = NetFrameworkVersion.NetFramework35;
                    break;

                case NetFrameworkVersionExtensions.NetFramework40:
                    value = NetFrameworkVersion.NetFramework40;
                    break;

                case NetFrameworkVersionExtensions.NetFramework45:
                    value = NetFrameworkVersion.NetFramework45;
                    break;

                case NetFrameworkVersionExtensions.NetFramework451:
                    value = NetFrameworkVersion.NetFramework451;
                    break;

                case NetFrameworkVersionExtensions.NetFramework452:
                    value = NetFrameworkVersion.NetFramework452;
                    break;

                case NetFrameworkVersionExtensions.NetFramework46:
                    value = NetFrameworkVersion.NetFramework46;
                    break;

                case NetFrameworkVersionExtensions.NetFramework461:
                    value = NetFrameworkVersion.NetFramework461;
                    break;

                case NetFrameworkVersionExtensions.NetFramework462:
                    value = NetFrameworkVersion.NetFramework462;
                    break;

                case NetFrameworkVersionExtensions.NetFramework47:
                    value = NetFrameworkVersion.NetFramework47;
                    break;

                default:
                    output = false;
                    break;
            }

            return output;
        }
    }


    /// <summary>
    /// Machine readable version strings in the 'v' format (example: v4.0).
    /// </summary>
    public static class NetFrameworkVersionVFormatExtensions
    {
        public const string NetFramework10 = @"v1.0";
        public const string NetFramework11 = @"v1.1";
        public const string NetFramework20 = @"v2.0";
        public const string NetFramework30 = @"v3.0";
        public const string NetFramework35 = @"v3.5";
        public const string NetFramework40 = @"v4.0";
        public const string NetFramework45 = @"v4.5";
        public const string NetFramework451 = @"v4.5.1";
        public const string NetFramework452 = @"v4.5.2";
        public const string NetFramework46 = @"v4.6";
        public const string NetFramework461 = @"v4.6.1";
        public const string NetFramework462 = @"v4.6.2";
        public const string NetFramework47 = @"v4.7";


        public static string ToDefaultString(this NetFrameworkVersion netFrameworkVersion)
        {
            string output;
            switch (netFrameworkVersion)
            {
                case NetFrameworkVersion.NetFramework10:
                    output = NetFrameworkVersionVFormatExtensions.NetFramework10;
                    break;

                case NetFrameworkVersion.NetFramework11:
                    output = NetFrameworkVersionVFormatExtensions.NetFramework11;
                    break;

                case NetFrameworkVersion.NetFramework20:
                    output = NetFrameworkVersionVFormatExtensions.NetFramework20;
                    break;

                case NetFrameworkVersion.NetFramework30:
                    output = NetFrameworkVersionVFormatExtensions.NetFramework30;
                    break;

                case NetFrameworkVersion.NetFramework35:
                    output = NetFrameworkVersionVFormatExtensions.NetFramework35;
                    break;

                case NetFrameworkVersion.NetFramework40:
                    output = NetFrameworkVersionVFormatExtensions.NetFramework40;
                    break;

                case NetFrameworkVersion.NetFramework45:
                    output = NetFrameworkVersionVFormatExtensions.NetFramework45;
                    break;

                case NetFrameworkVersion.NetFramework451:
                    output = NetFrameworkVersionVFormatExtensions.NetFramework451;
                    break;

                case NetFrameworkVersion.NetFramework452:
                    output = NetFrameworkVersionVFormatExtensions.NetFramework452;
                    break;

                case NetFrameworkVersion.NetFramework46:
                    output = NetFrameworkVersionVFormatExtensions.NetFramework46;
                    break;

                case NetFrameworkVersion.NetFramework461:
                    output = NetFrameworkVersionVFormatExtensions.NetFramework461;
                    break;

                case NetFrameworkVersion.NetFramework462:
                    output = NetFrameworkVersionVFormatExtensions.NetFramework462;
                    break;

                case NetFrameworkVersion.NetFramework47:
                    output = NetFrameworkVersionVFormatExtensions.NetFramework47;
                    break;

                default:
                    throw new UnexpectedEnumerationValueException<NetFrameworkVersion>(netFrameworkVersion);
            }

            return output;
        }

        public static NetFrameworkVersion FromDefault(string netFrameworkVersion)
        {
            NetFrameworkVersion output;
            if (!NetFrameworkVersionVFormatExtensions.TryFromDefault(netFrameworkVersion, out output))
            {
                string message = String.Format(@"Unrecognized .NET framework version string: {0}.", netFrameworkVersion);
                throw new ArgumentException(message);
            }

            return output;
        }

        public static bool TryFromDefault(string netFrameworkVersion, out NetFrameworkVersion value)
        {
            bool output = true;
            value = NetFrameworkVersion.NetFramework10;

            switch (netFrameworkVersion)
            {
                case NetFrameworkVersionVFormatExtensions.NetFramework10:
                    value = NetFrameworkVersion.NetFramework10;
                    break;

                case NetFrameworkVersionVFormatExtensions.NetFramework11:
                    value = NetFrameworkVersion.NetFramework11;
                    break;

                case NetFrameworkVersionVFormatExtensions.NetFramework20:
                    value = NetFrameworkVersion.NetFramework20;
                    break;

                case NetFrameworkVersionVFormatExtensions.NetFramework30:
                    value = NetFrameworkVersion.NetFramework30;
                    break;

                case NetFrameworkVersionVFormatExtensions.NetFramework35:
                    value = NetFrameworkVersion.NetFramework35;
                    break;

                case NetFrameworkVersionVFormatExtensions.NetFramework40:
                    value = NetFrameworkVersion.NetFramework40;
                    break;

                case NetFrameworkVersionVFormatExtensions.NetFramework45:
                    value = NetFrameworkVersion.NetFramework45;
                    break;

                case NetFrameworkVersionVFormatExtensions.NetFramework451:
                    value = NetFrameworkVersion.NetFramework451;
                    break;

                case NetFrameworkVersionVFormatExtensions.NetFramework452:
                    value = NetFrameworkVersion.NetFramework452;
                    break;

                case NetFrameworkVersionVFormatExtensions.NetFramework46:
                    value = NetFrameworkVersion.NetFramework46;
                    break;

                case NetFrameworkVersionVFormatExtensions.NetFramework461:
                    value = NetFrameworkVersion.NetFramework461;
                    break;

                case NetFrameworkVersionVFormatExtensions.NetFramework462:
                    value = NetFrameworkVersion.NetFramework462;
                    break;

                case NetFrameworkVersionVFormatExtensions.NetFramework47:
                    value = NetFrameworkVersion.NetFramework47;
                    break;

                default:
                    output = false;
                    break;
            }

            return output;
        }
    }
}
