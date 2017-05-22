using System;
using System.IO;

using Public.Common.Lib.Production;


namespace Public.Common.Augustus
{
    public class Configuration
    {
        public const string DefaultBuildListFileName = @"Augustus Build List.txt";
        public const string DefaultOutputFilePath = @"C:\temp\logs\Augustus\Log.txt"; // Will be made into a dated path.


        #region Static

        public static string DefaultBuildListFilePath
        {
            get
            {
                string output = Path.Combine(Production.UserConfigurationDataDirectoryPath, Configuration.DefaultBuildListFileName);
                return output;
            }
        }

        #endregion



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
