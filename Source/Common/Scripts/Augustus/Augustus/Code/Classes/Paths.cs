using System;
using System.IO;

using Public.Common.Lib.Extensions;
using Public.Common.Lib.Production;


namespace Public.Common.Augustus
{
    public class Paths
    {
        public const string DefaultResultsFileName = @"Augusts - Results.txt";


        public string OutputDirectoryPath { get; set; }
        public string ResultsFilePath { get; set; }
        public string LogsDirectoryPath { get; set; }
        public string LogFilePath { get; set; }


        public Paths()
        {
        }

        public Paths(string outputDirectoryPath, string programName, DateTime runTime)
        {
            this.OutputDirectoryPath = outputDirectoryPath;
            this.ResultsFilePath = Path.Combine(this.OutputDirectoryPath, Paths.DefaultResultsFileName);
            this.LogsDirectoryPath = Path.Combine(this.OutputDirectoryPath, Production.LogsDirectoryName);

            string logFileName = String.Format(@"{0} - {1}_{2}.log", Constants.ProgramName, runTime.ToYYYYMMDDStr(), runTime.ToHHMMSSStr());
            this.LogFilePath = Path.Combine(this.LogsDirectoryPath, logFileName);
        }

        public void EnsurePathsCreated()
        {
            PathExtensions.EnsureDirectoryPathCreated(this.OutputDirectoryPath);
            PathExtensions.EnsureDirectoryPathCreated(this.LogsDirectoryPath);
        }
    }
}
