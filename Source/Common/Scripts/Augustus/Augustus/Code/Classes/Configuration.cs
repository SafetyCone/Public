using System;
using System.IO;

using Public.Common.Lib.Extensions;
using Public.Common.Lib.IO;
using Public.Common.Lib.IO.Extensions;
using Public.Common.Lib.Production;


namespace Public.Common.Augustus
{
    public class Configuration
    {
        public const char DefaultResultsHandlingTokenSeparator = '|';
        public const string OpenResultsToken = @"OpenResults";
        public const string EmailResultsToken = @"EmailResults";
        public const string DefaultBuildListFileName = @"Augustus Build List.txt";


        #region Static

        public static string DefaultBuildListFilePath
        {
            get
            {
                string output = Path.Combine(Production.UserConfigurationDataDirectoryPath, Configuration.DefaultBuildListFileName);
                return output;
            }
        }


        /// <remarks>
        /// The input arguments are:
        /// 
        /// 1. Build list file path.
        /// 2. Output file path.
        /// 
        /// If no input arguments are supplied, the default build list file path is used, and a dated default output file path is used.
        /// </remarks>
        public static bool ParseArguments(out Configuration configuration, Paths paths, string[] args, IOutputStream outputStream)
        {
            bool output = true;

            configuration = new Configuration();
            configuration.Paths = paths;

            try
            {
                int argCount = args.Length;
                string buildListFilePath = Configuration.DefaultBuildListFilePath;
                string resultsFilePath = configuration.Paths.ResultsFilePath;
                bool openResults = false;
                bool emailResults = true;
                switch (argCount)
                {
                    case 0:
                        // Use the default paths.
                        Configuration.VerifyFileExistence(buildListFilePath, @"Specified build list file not found: {0}");
                        outputStream.WriteLine(@"Using default build list file.");
                        outputStream.WriteLine(@"Writing to default results file.");
                        break;

                    case 1:
                        buildListFilePath = Configuration.VerifyFileExistence(args[0], @"Specified build list file not found: {0}");
                        outputStream.WriteLine(@"Writing to default results file.");
                        break;

                    case 2:
                        buildListFilePath = Configuration.VerifyFileExistence(args[0], @"Specified build list file not found: {0}");
                        resultsFilePath = args[1];
                        break;

                    case 3:
                        buildListFilePath = Configuration.VerifyFileExistence(args[0], @"Specified build list file not found: {0}");
                        resultsFilePath = args[1];
                        Configuration.ParseHandleResultsToken(ref openResults, ref emailResults, args[2], Configuration.DefaultResultsHandlingTokenSeparator);
                        break;

                    default:
                        string message = String.Format(@"Too many input arguments. Found: {0}.", argCount);
                        throw new InvalidOperationException(message);
                }

                outputStream.WriteLine();

                string line;
                line = String.Format(@"Using build list file: {0}", buildListFilePath);
                outputStream.WriteLineAndBlankLine(line);
                configuration.BuildListFilePath = buildListFilePath;

                line = String.Format(@"Writing to results file: {0}", resultsFilePath);
                outputStream.WriteLineAndBlankLine(line);
                configuration.Paths.ResultsFilePath = resultsFilePath;

                configuration.OpenResults = openResults;
                configuration.EmailResults = emailResults;
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

            string line = String.Format(@"Usage: {0}.exe (optional)[BuildListFilePath = DefaultPath] (optional)[OutputFilePath = DefaultPath] (optional)[HandleResults1 = OpenResults[|HandleResults2]]", programName);
            outputStream.WriteLineAndBlankLine(line);
        }

        private static void ParseHandleResultsToken(ref bool openResults, ref bool emailResults, string handleResultsToken, char separator)
        {
            openResults = false;
            emailResults = false;

            string[] tokens = handleResultsToken.Split(separator);
            foreach(string token in tokens)
            {
                if(Configuration.OpenResultsToken == token)
                {
                    openResults = true;
                }

                if(Configuration.EmailResultsToken == token)
                {
                    emailResults = true;
                }
            }
        }

        private static string VerifyFileExistence(string filePath, string fileNotFoundExceptionMessageMask)
        {
            if (!File.Exists(filePath))
            {
                string message = String.Format(fileNotFoundExceptionMessageMask, filePath);
                throw new FileNotFoundException(message);
            }

            return filePath;
        }

        #endregion


        public DateTime RunTime { get; private set; }
        public string BuildListFilePath { get; set; }
        public Paths Paths { get; set; }
        public bool OpenResults { get; set; }
        public bool EmailResults { get; set; }


        public Configuration()
        {
            this.RunTime = DateTime.Now;
        }

        public Configuration(string buildListFilePath)
            : this()
        {
            this.BuildListFilePath = buildListFilePath;
        }
    }
}
