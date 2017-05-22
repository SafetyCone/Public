

namespace Augustus
{
    public class BuildInfo
    {
        public string OutputLogPath { get; set; }
        public string ErrorLogPath { get; set; }
        public string SuccessRegexPattern { get; set; }
        public string BuildCommand { get; set; }


        public BuildInfo()
        {
        }

        public BuildInfo(string outputLogPath, string errorLogPath, string successRegexPattern, string buildCommand)
        {
            this.OutputLogPath = outputLogPath;
            this.ErrorLogPath = errorLogPath;
            this.SuccessRegexPattern = successRegexPattern;
            this.BuildCommand = buildCommand;
        }
    }
}
