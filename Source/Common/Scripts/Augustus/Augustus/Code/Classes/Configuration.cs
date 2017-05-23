using System;
using System.IO;

using Public.Common.Lib.Extensions;
using Public.Common.Lib.Production;


namespace Public.Common.Augustus
{
    public class Configuration
    {
        public const char DefaultResultsHandlingTokenSeparator = '|';
        public const string OpenResultsToken = @"OpenResults";
        public const string EmailResultsToken = @"EmailResults";
        public const string DefaultBuildListFileName = @"Augustus Build List.txt";
        public const string DefaultUndatedOutputFilePath = @"C:\temp\logs\Augustus\Log.txt"; // Will be made into a dated path.


        #region Static

        public static string DefaultBuildListFilePath
        {
            get
            {
                string output = Path.Combine(Production.UserConfigurationDataDirectoryPath, Configuration.DefaultBuildListFileName);
                return output;
            }
        }
        public static string DefaultOutputFilePath
        {
            get
            {
                string output = Configuration.GetDatedDefaultOutputFilePath();
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
        public static bool ParseArguments(out Configuration configuration, string[] args, TextWriter outputStream)
        {
            bool output = true;

            configuration = new Configuration();

            try
            {
                int argCount = args.Length;
                string buildListFilePath = Configuration.DefaultBuildListFilePath;
                string outputFilePath = Configuration.GetDatedDefaultOutputFilePath();
                bool openResults = false;
                bool emailResults = true;
                switch (argCount)
                {
                    case 0:
                        // Use the default paths.
                        Configuration.VerifyFileExistence(buildListFilePath, @"Specified build list file not found: {0}");
                        outputStream.WriteLine(@"Using default build list file.");
                        outputStream.WriteLine(@"Using default output file.");
                        break;

                    case 1:
                        buildListFilePath = Configuration.VerifyFileExistence(args[0], @"Specified build list file not found: {0}");
                        outputStream.WriteLine(@"Using default output file.");
                        break;

                    case 2:
                        buildListFilePath = Configuration.VerifyFileExistence(args[0], @"Specified build list file not found: {0}");
                        outputFilePath = args[1];
                        break;

                    case 3:
                        buildListFilePath = Configuration.VerifyFileExistence(args[0], @"Specified build list file not found: {0}");
                        outputFilePath = args[1];
                        Configuration.ParseHandleResultsToken(ref openResults, ref emailResults, args[2], Configuration.DefaultResultsHandlingTokenSeparator);
                        break;

                    default:
                        string message = String.Format(@"Too many input arguments. Found: {0}. Usage: Augustus.exe (optional)[BuildListFilePath=DefaultPath] (optional)[OutputFilePath=DefaultPath] (optional)[HandleResults1=OpenResults[|HandleResults2]]", argCount);
                        throw new InvalidOperationException(message);
                }

                outputStream.WriteLine();

                string line;
                line = String.Format(@"Using build list file: {0}", buildListFilePath);
                outputStream.WriteLineAndBlankLine(line);
                configuration.BuildListFilePath = buildListFilePath;

                line = String.Format(@"Using output file: {0}", outputFilePath);
                outputStream.WriteLineAndBlankLine(line);
                configuration.OutputFilePath = outputFilePath;

                configuration.OpenResults = openResults;
                configuration.EmailResults = emailResults;
            }
            catch (Exception ex)
            {
                outputStream.WriteLineAndBlankLine(ex.Message);

                output = false;
            }

            return output;
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

        /// <remarks>
        /// I will want the output files (basically logs) to be dated.
        /// </remarks>
        private static string GetDatedDefaultOutputFilePath()
        {
            string output = Configuration.DateMarkPath(Configuration.DefaultUndatedOutputFilePath);
            return output;
        }

        private static string DateMarkPath(string path)
        {
            string todayYYYYMMDD = DateTime.Today.ToYYYYMMDDStr();

            string directoryPath = Path.GetDirectoryName(path);
            string fileNameOnly = Path.GetFileNameWithoutExtension(path);
            string extension = PathExtensions.GetExtensionOnly(path);

            string datedFileName = String.Format(@"{0} - {1}", fileNameOnly, todayYYYYMMDD);
            string fullDatedFileName = PathExtensions.GetFullFileName(datedFileName, extension);

            string output = Path.Combine(directoryPath, fullDatedFileName);
            return output;
        }

        #endregion



        public string BuildListFilePath { get; set; }
        public string OutputFilePath { get; set; }
        public bool OpenResults { get; set; }
        public bool EmailResults { get; set; }


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
