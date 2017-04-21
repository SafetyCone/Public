using System;


namespace Public.Common.Lib.Code.Physical
{
    /// <summary>
    /// Maps Visual Studio year version to Icon Handler and Visual Studio Version Selector solution version string.
    /// </summary>
    /// <remarks>
    /// Via the magic of Icon Handlers and Visual Studio Version Selector, despite having the same .sln file extensions, solution files can specifiy with their contents what version of Visual Studio they require.
    /// 
    /// See: https://msdn.microsoft.com/en-us/library/cc144122%28VS.85%29.aspx?f=255&MSPPError=-2147217396
    /// </remarks>
    public static class VisualStudioVersionSolutionFileExtensions
    {
        public const string VS2010 = @"# Visual Studio 2010";
        public const string VS2013 = @"# Visual Studio 2013";
        public const string VS2015 = @"# Visual Studio 14";
        public const string VS2017 = @"# Visual Studio 15";


        public static string ToDefaultString(this VisualStudioVersion visualStudioVersion)
        {
            string output;
            switch (visualStudioVersion)
            {
                case VisualStudioVersion.VS2010:
                    output = VisualStudioVersionSolutionFileExtensions.VS2010;
                    break;

                case VisualStudioVersion.VS2013:
                    output = VisualStudioVersionSolutionFileExtensions.VS2013;
                    break;

                case VisualStudioVersion.VS2015:
                    output = VisualStudioVersionSolutionFileExtensions.VS2015;
                    break;

                case VisualStudioVersion.VS2017:
                    output = VisualStudioVersionSolutionFileExtensions.VS2017;
                    break;

                default:
                    throw new UnexpectedEnumerationValueException<VisualStudioVersion>(visualStudioVersion);
            }

            return output;
        }

        public static VisualStudioVersion FromDefault(string visualStudioVersion)
        {
            VisualStudioVersion output;
            if (!VisualStudioVersionSolutionFileExtensions.TryFromDefault(visualStudioVersion, out output))
            {
                throw new ArgumentException(@"Unrecognized Visual Studio version string.");
            }

            return output;
        }

        public static bool TryFromDefault(string visualStudioVersion, out VisualStudioVersion value)
        {
            bool output = true;
            value = VisualStudioVersion.VS2015;

            switch (visualStudioVersion)
            {
                case VisualStudioVersionSolutionFileExtensions.VS2010:
                    value = VisualStudioVersion.VS2010;
                    break;

                case VisualStudioVersionSolutionFileExtensions.VS2013:
                    value = VisualStudioVersion.VS2013;
                    break;

                case VisualStudioVersionSolutionFileExtensions.VS2015:
                    value = VisualStudioVersion.VS2015;
                    break;

                case VisualStudioVersionSolutionFileExtensions.VS2017:
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
