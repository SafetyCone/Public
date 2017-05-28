using System;
using System.IO;


namespace Public.Common.Augustus.Lib
{
    public class CygwinBuilder : IBuilder
    {
        public const string CygwinShellCommandMask = @"set PATH={0};%PATH% & make --directory ""{1}"" clean & make --directory ""{1}""";
        public const string CygwinBinDirectoryPath = @"C:\Tools\Cygwin64\bin";
        public const string CygwinSucessRegexPattern = @"^Done!";


        #region Static

        public static string GetDefaultOutputLogFilePath(string buildFilePath)
        {
            string buildDirectoryPath = Path.GetDirectoryName(buildFilePath);

            string output = Path.Combine(buildDirectoryPath, Builder.OutputLogFileSuffix);
            return output;
        }

        public static string GetDefaultErrorLogFilePath(string buildFilePath)
        {
            string buildDirectoryPath = Path.GetDirectoryName(buildFilePath);
            
            string output = Path.Combine(buildDirectoryPath, Builder.ErrorLogFileSuffix);
            return output;
        }

        #endregion

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
            string directoryForMake = Path.GetDirectoryName(buildItem.FilePath);

            string buildCommand = String.Format(CygwinBuilder.CygwinShellCommandMask, CygwinBuilder.CygwinBinDirectoryPath, directoryForMake);

            BuildInfo output = new BuildInfo(buildItem, buildCommand, CygwinBuilder.CygwinSucessRegexPattern);
            return output;
        }

        #endregion
    }
}
