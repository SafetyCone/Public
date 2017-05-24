using System;
using System.IO;


namespace Public.Common.Augustus.Lib
{
    public class MSBuildBuilder : IBuilder
    {
        public const string MSBuildShellCommandMask = @"""{0}"" ""{1}""";
        public const string MSBuildExecutablePath = @"C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe";
        public const string MSBuildSuccessRegexPattern = @"^Build succeeded.";

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
            string buildFileNameNoExtension = Path.GetFileNameWithoutExtension(buildItem.FilePath);
            string buildDirectoryPath = Path.GetDirectoryName(buildItem.FilePath);

            string outputLogFileName = String.Format(@"{0} {1}", buildFileNameNoExtension, Builder.OutputLogFileSuffix);
            string outputLogPath = Path.Combine(buildDirectoryPath, outputLogFileName);

            string errorLogFileName = String.Format(@"{0} {1}", buildFileNameNoExtension, Builder.ErrorLogFileSuffix);
            string errorLogPath = Path.Combine(buildDirectoryPath, errorLogFileName);

            string buildCommand = String.Format(MSBuildBuilder.MSBuildShellCommandMask, MSBuildBuilder.MSBuildExecutablePath, buildItem.FilePath);

            BuildInfo output = new BuildInfo(outputLogPath, errorLogPath, MSBuildBuilder.MSBuildSuccessRegexPattern, buildCommand);
            return output;
        }

        #endregion
    }
}
