using System;
using System.IO;


namespace Public.Common.Augustus.Lib
{
    public class MSBuildBuilder : IBuilder
    {
        public const string MSBuildShellCommandMask = @"""{0}"" ""{1}""";
        public const string MSBuildExecutablePath = @"C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe";
        public const string MSBuildSuccessRegexPattern = @"^Build succeeded.";


        #region Static

        public static string GetDefaultOutputLogFilePath(string buildFilePath)
        {
            string buildFileNameNoExtension = Path.GetFileNameWithoutExtension(buildFilePath);
            string buildDirectoryPath = Path.GetDirectoryName(buildFilePath);

            string outputLogFileName = String.Format(@"{0} {1}", buildFileNameNoExtension, Builder.OutputLogFileSuffix);

            string output = Path.Combine(buildDirectoryPath, outputLogFileName);
            return output;
        }

        public static string GetDefaultErrorLogFilePath(string buildFilePath)
        {
            string buildFileNameNoExtension = Path.GetFileNameWithoutExtension(buildFilePath);
            string buildDirectoryPath = Path.GetDirectoryName(buildFilePath);

            string errorLogFileName = String.Format(@"{0} {1}", buildFileNameNoExtension, Builder.ErrorLogFileSuffix);

            string output = Path.Combine(buildDirectoryPath, errorLogFileName);
            return output;
        }

        #endregion

        #region IBuilder Members

        public string SuccessRegexPattern
        {
            get
            {
                return MSBuildBuilder.MSBuildSuccessRegexPattern;
            }
        }

        public BuildInfo GetBuildInfo(BuildItem buildItem)
        {
            string buildCommand = String.Format(MSBuildBuilder.MSBuildShellCommandMask, MSBuildBuilder.MSBuildExecutablePath, buildItem.FilePath);

            BuildInfo output = new BuildInfo(buildItem, buildCommand, MSBuildBuilder.MSBuildSuccessRegexPattern);
            return output;
        }

        #endregion
    }
}