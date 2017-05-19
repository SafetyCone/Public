using System;


namespace Augustus
{
    public class Configuration
    {
        public const string DefaultBuildListFilePath = @"C:\Organizations\Minex\Data\Default Augustus Build List.txt";
        public const string DefaultOutputFilePath = @"C:\temp\logs\Augustus\Log.txt"; // Will be made into a dated path.


        public string BuildListFilePath { get; set; }
        public string OutputFilePath { get; set; }


        public Configuration()
        {
        }

        public Configuration(string buildListFilePath, string outputFilePath)
        {
            this.BuildListFilePath = buildListFilePath;
            this.OutputFilePath = outputFilePath;
        }
    }
}
