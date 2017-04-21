using System;


namespace Public.Common.Lib.Code.Physical
{
    /// <summary>
    /// Maps Visual Studio year version to majory product version.
    /// </summary>
    public static class VisualStudioVersionToolsVersionExtensions
    {
        public const int VS2010 = 4;
        public const int VS2013 = 12;
        public const int VS2015 = 14;
        public const int VS2017 = 15;


        public static int ToDefaultInt(this VisualStudioVersion visualStudioVersion)
        {
            int output;
            switch (visualStudioVersion)
            {
                case VisualStudioVersion.VS2010:
                    output = VisualStudioVersionToolsVersionExtensions.VS2010;
                    break;

                case VisualStudioVersion.VS2013:
                    output = VisualStudioVersionToolsVersionExtensions.VS2013;
                    break;

                case VisualStudioVersion.VS2015:
                    output = VisualStudioVersionToolsVersionExtensions.VS2015;
                    break;

                case VisualStudioVersion.VS2017:
                    output = VisualStudioVersionToolsVersionExtensions.VS2017;
                    break;

                default:
                    throw new UnexpectedEnumerationValueException<VisualStudioVersion>(visualStudioVersion);
            }

            return output;
        }

        public static VisualStudioVersion FromDefault(int toolsVersion)
        {
            VisualStudioVersion output;
            if (!VisualStudioVersionToolsVersionExtensions.TryFromDefault(toolsVersion, out output))
            {
                throw new ArgumentException(@"Unrecognized Visual Studio tools version.");
            }

            return output;
        }

        public static bool TryFromDefault(int toolsVersion, out VisualStudioVersion value)
        {
            bool output = true;
            value = VisualStudioVersion.VS2015;

            switch (toolsVersion)
            {
                case VisualStudioVersionToolsVersionExtensions.VS2010:
                    value = VisualStudioVersion.VS2010;
                    break;

                case VisualStudioVersionToolsVersionExtensions.VS2013:
                    value = VisualStudioVersion.VS2013;
                    break;

                case VisualStudioVersionToolsVersionExtensions.VS2015:
                    value = VisualStudioVersion.VS2015;
                    break;

                case VisualStudioVersionToolsVersionExtensions.VS2017:
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
