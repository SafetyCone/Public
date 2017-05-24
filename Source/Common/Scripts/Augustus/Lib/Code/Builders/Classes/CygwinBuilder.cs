using System;
using System.IO;


namespace Public.Common.Augustus.Lib
{
    public class CygwinBuilder : IBuilder
    {
        public const string CygwinShellCommandMask = @"set PATH={0};%PATH% & make --directory ""{1}"" clean & make --directory ""{1}""";
        public const string CygwinBinDirectoryPath = @"C:\Tools\Cygwin64\bin";
        public const string CygwinSucessRegexPattern = @"^Done!";


        #region IBuilder Members

        public string SuccessRegexPattern
        {
            get
            {
                return CygwinBuilder.CygwinSucessRegexPattern;
            }
        }

        public BuildInfo GetBuildInfo(BuildItem buildItem)
        {
            string buildDirectoryPath = Path.GetDirectoryName(buildItem.FilePath);
            string outputLogPath = Path.Combine(buildDirectoryPath, Builder.OutputLogFileSuffix);
            string errorLogPath = Path.Combine(buildDirectoryPath, Builder.ErrorLogFileSuffix);

            string directoryForMake = Path.GetDirectoryName(buildItem.FilePath);
            string buildCommand = String.Format(CygwinBuilder.CygwinShellCommandMask, CygwinBuilder.CygwinBinDirectoryPath, directoryForMake);

            BuildInfo output = new BuildInfo(outputLogPath, errorLogPath, CygwinBuilder.CygwinSucessRegexPattern, buildCommand);
            return output;
        }

        #endregion
    }
}
