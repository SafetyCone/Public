using System;


namespace Public.Common.Lib.Code
{
    /// <summary>
    /// Specifies whether a Visual Studio project content file should be copied to the output build directory.
    /// </summary>
    public enum CopyToOutputDirectory
    {
        Never,
        PreserveNewest,
        Always,
        //WindowsForms, // TODO
        //WPF,
        //WebSite,
        //WebApp,
    }


    /// <summary>
    /// For use in reading/writing a Visual Studio project file.
    /// </summary>
    public static class CopyToOutputDirectoryExtensions
    {
        public const string Never = @"Never";
        public const string PreserveNewest = @"PreserveNewest";
        public const string Always = @"Always";


        public static string ToDefaultString(this CopyToOutputDirectory copyToOutputDirectory)
        {
            string output;
            switch (copyToOutputDirectory)
            {
                case CopyToOutputDirectory.Never:
                    output = CopyToOutputDirectoryExtensions.Never;
                    break;

                case CopyToOutputDirectory.PreserveNewest:
                    output = CopyToOutputDirectoryExtensions.PreserveNewest;
                    break;

                case CopyToOutputDirectory.Always:
                    output = CopyToOutputDirectoryExtensions.Always;
                    break;

                default:
                    throw new UnexpectedEnumerationValueException<CopyToOutputDirectory>(copyToOutputDirectory);
            }

            return output;
        }

        public static CopyToOutputDirectory FromDefault(string copyToOutputDirectory)
        {
            CopyToOutputDirectory output;
            if (!CopyToOutputDirectoryExtensions.TryFromDefault(copyToOutputDirectory, out output))
            {
                string message = String.Format(@"Unrecognized copy to output directory string: {0}.", copyToOutputDirectory);
                throw new ArgumentException(message);
            }

            return output;
        }

        public static bool TryFromDefault(string copyToOutputDirectory, out CopyToOutputDirectory value)
        {
            bool output = true;
            value = CopyToOutputDirectory.Never;

            switch (copyToOutputDirectory)
            {
                case CopyToOutputDirectoryExtensions.Never:
                    value = CopyToOutputDirectory.Never;
                    break;

                case CopyToOutputDirectoryExtensions.PreserveNewest:
                    value = CopyToOutputDirectory.PreserveNewest;
                    break;

                case CopyToOutputDirectoryExtensions.Always:
                    value = CopyToOutputDirectory.Always;
                    break;

                default:
                    output = false;
                    break;
            }

            return output;
        }
    }
}
