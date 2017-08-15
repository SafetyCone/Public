using System;
using System.Collections.Generic;

using Public.Common.Lib;


namespace Public.Common.Lib.Code.Physical
{
    /// <summary>
    /// Specifies the Visual Studio year version.
    /// </summary>
    public enum VisualStudioVersion
    {
        VS_ALL,
        VS_UNKNOWN,
        VS_LATEST,
        VS2010,
        VS2013,
        VS2015,
        VS2017,
    }


    /// <summary>
    /// Basic human and machine readable string representations.
    /// </summary>
    /// <remarks>
    /// This format is used in solution and project file tokens.
    /// </remarks>
    public static class VisualStudioVersionExtensions
    {
        public const string VS_ALL = @"VS_ALL";
        public const string VS_UNKNOWN = @"VS_UNKNOWN";
        public const string VS_LATEST = @"VS_LATEST";
        public const string VS2010 = @"VS2010";
        public const string VS2013 = @"VS2013";
        public const string VS2015 = @"VS2015";
        public const string VS2017 = @"VS2017";


        public static VisualStudioVersion[] GetAllVisualStudioVersions()
        {
            VisualStudioVersion[] output = new VisualStudioVersion[]
            {
                VisualStudioVersion.VS2010,
                VisualStudioVersion.VS2013,
                VisualStudioVersion.VS2015,
                VisualStudioVersion.VS2017,
            };
            return output;
        }

        public static string[] GetAllVisualStudioVersionStrings()
        {
            string[] output = new string[]
            {
                VisualStudioVersionExtensions.VS2010,
                VisualStudioVersionExtensions.VS2013,
                VisualStudioVersionExtensions.VS2015,
                VisualStudioVersionExtensions.VS2017,
            };

            return output;
        }

        public static string ToDefaultString(this VisualStudioVersion visualStudioVersion)
        {
            string output;
            switch (visualStudioVersion)
            {
                case VisualStudioVersion.VS_ALL:
                    output = VisualStudioVersionExtensions.VS_ALL;
                    break;

                case VisualStudioVersion.VS_UNKNOWN:
                    output = VisualStudioVersionExtensions.VS_UNKNOWN;
                    break;

                case VisualStudioVersion.VS_LATEST:
                    output = VisualStudioVersionExtensions.VS_LATEST;
                    break;

                case VisualStudioVersion.VS2010:
                    output = VisualStudioVersionExtensions.VS2010;
                    break;

                case VisualStudioVersion.VS2013:
                    output = VisualStudioVersionExtensions.VS2013;
                    break;

                case VisualStudioVersion.VS2015:
                    output = VisualStudioVersionExtensions.VS2015;
                    break;

                case VisualStudioVersion.VS2017:
                    output = VisualStudioVersionExtensions.VS2017;
                    break;

                default:
                    throw new UnexpectedEnumerationValueException<VisualStudioVersion>(visualStudioVersion);
            }

            return output;
        }

        public static VisualStudioVersion FromDefault(string visualStudioVersion)
        {
            VisualStudioVersion output;
            if (!VisualStudioVersionExtensions.TryFromDefault(visualStudioVersion, out output))
            {
                string message = String.Format(@"Unrecognized Visual Studio version string: {0}.", visualStudioVersion);
                throw new ArgumentException(message);
            }

            return output;
        }

        public static bool TryFromDefault(string visualStudioVersion, out VisualStudioVersion value)
        {
            bool output = true;
            value = VisualStudioVersion.VS2015;

            switch (visualStudioVersion)
            {
                case VisualStudioVersionExtensions.VS_ALL:
                    value = VisualStudioVersion.VS_ALL;
                    break;

                case VisualStudioVersionExtensions.VS_UNKNOWN:
                    value = VisualStudioVersion.VS_UNKNOWN;
                    break;

                case VisualStudioVersionExtensions.VS_LATEST:
                    value = VisualStudioVersion.VS_LATEST;
                    break;

                case VisualStudioVersionExtensions.VS2010:
                    value = VisualStudioVersion.VS2010;
                    break;

                case VisualStudioVersionExtensions.VS2013:
                    value = VisualStudioVersion.VS2013;
                    break;

                case VisualStudioVersionExtensions.VS2015:
                    value = VisualStudioVersion.VS2015;
                    break;

                case VisualStudioVersionExtensions.VS2017:
                    value = VisualStudioVersion.VS2017;
                    break;

                default:
                    output = false;
                    break;
            }

            return output;
        }
    }
}