using System;


namespace Public.Common.Augustus.Lib
{
    public class BuildInfo
    {
        public BuildItem Item { get; set; }
        public string BuildCommand { get; set; }
        public string SuccessRegexPattern { get; set; }


        public BuildInfo()
        {
        }

        public BuildInfo(BuildItem item, string buildCommand, string successRegexPattern)
        {
            this.Item = item;
            this.SuccessRegexPattern = successRegexPattern;
            this.BuildCommand = buildCommand;
        }
    }
}
