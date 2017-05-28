using System;
using System.IO;

using Public.Common.Lib.Extensions;
using Public.Common.Lib.IO;
using Public.Common.Lib.IO.Extensions;
using Public.Common.Lib.Organizational;
using Public.Common.Lib.Production;


namespace Public.Common.Maynard
{
    public class Configuration
    {
        public const string DefaultProjectBuildListFileName = @"Project Build List for Binaries Directory.txt";


        #region Static

        public static string DefaultProjectListFilePath
        {
            get
            {
                string output = Path.Combine(Production.UserConfigurationDataDirectoryPath, Configuration.DefaultProjectBuildListFileName);
                return output;
            }
        }
        public static string DefaultOutputDirectoryPath
        {
            get
            {
                string output = Path.Combine(OrganizationalPaths.DefaultOrganizationsDirectoryPath, MinexOrganization.OrganizationName, Constants.BinariesDirectoryName);
                return output;
            }
        }


        public static bool TryParseArguments(out Configuration configuration, IOutputStream outputStream, string[] args)
        {
            bool output = true;

            configuration = new Configuration();

            try
            {
                int numArgs = args.Length;

                string solutionListFilePath = 0 < numArgs ? args[0] : Configuration.DefaultProjectListFilePath;
                if (!File.Exists(solutionListFilePath))
                {
                    string message = String.Format(@"Project build list file not found: {0}", solutionListFilePath);
                    throw new FileNotFoundException(message);
                }

                string outputDirectoryPath = 1 < numArgs ? args[1] : Configuration.DefaultOutputDirectoryPath;
                if (!Directory.Exists(outputDirectoryPath))
                {
                    Directory.CreateDirectory(outputDirectoryPath);
                }

                if (2 < numArgs)
                {
                    string message = String.Format(@"Too many input arguments found: {0}. Expected a maximum of 2.", numArgs);
                    throw new InvalidOperationException(message);
                }

                configuration.ProjectBuildListFilePath = solutionListFilePath;
                configuration.OutputDirectoryPath = outputDirectoryPath;
            }
            catch (Exception ex)
            {
                output = false;

                outputStream.WriteLineAndBlankLine(@"ERROR parsing input arguments.");
                outputStream.WriteLineAndBlankLine(ex.Message);

                Configuration.DisplayUsage(outputStream);
            }

            return output;
        }

        private static void DisplayUsage(IOutputStream outputStream)
        {
            string programName = Constants.ProgramName;
            outputStream.WriteLineAndBlankLine(programName);

            string line = String.Format(@"Usage: {0}.exe [ProjectBuildListTextFilePath=DefaultPath] [OutputDirectoryPath=DefaultPath]");
            outputStream.WriteLineAndBlankLine(line);
        }

        #endregion


        public string ProjectBuildListFilePath { get; set; }
        public string OutputDirectoryPath { get; set; }


        public Configuration()
        {
        }

        public Configuration(string solutionListFilePath, string outputDirectoryPath)
        {
            this.ProjectBuildListFilePath = solutionListFilePath;
            this.OutputDirectoryPath = outputDirectoryPath;
        }
    }
}
