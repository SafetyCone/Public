

namespace Augustus
{
    public static class Constants
    {
        public const string CommandShellExecutableName = @"cmd.exe";
        public const string OutputLogFileSuffix = @"Build.log";
        public const string ErrorLogFileSuffix = @"Build Error.log";

        public const string MSBuildShellCommandMask = @"""{0}"" ""{1}""";
        public const string CygwinShellCommandMask = @"set PATH={0};%PATH% & make --directory ""{1}"" clean & make --directory ""{1}""";

        // Configured.
        public const string BuildFileListFileRelativePath = @"..\..\..\..\..\..\..\..\..\Data\Augustus Build File List.txt";
        public const string DebugBuildFileListFileRelativePath = @"..\..\..\..\..\..\..\..\..\Data\Augustus Build File List - Debug.txt";

        public const string MSBuildExecutablePath = @"C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe";
        public const string MSBuildSuccessRegexPattern = @"^Build succeeded.";

        public const string CygwinBinDirectoryPath = @"C:\Tools\Cygwin64\bin";
        public const string CygwinSucessRegexPattern = @"^Done!";

        public const string ResultsFileName = @"Augustus Results.txt"; // Make into file path.
        public const string ResultsViewerPath = @"C:\Program Files (x86)\Notepad++\notepad++.exe";
    }
}
