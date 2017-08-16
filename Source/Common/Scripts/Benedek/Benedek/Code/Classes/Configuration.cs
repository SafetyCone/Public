using System;
using System.IO;

using Public.Common.Lib.IO;
using Public.Common.Lib.IO.Extensions;


namespace Benedek
{
    class Configuration
    {
        #region Static

        public static string DefaultOutputFilePath
        {
            get
            {
                string output = Path.Combine(Constants.DefaultOutputDirectoryPath, Constants.DefaultOutputFileName);
                return output;
            }
        }


        public static bool ParseArguments(out Configuration configuration, string[] args, IOutputStream interactive)
        {
            bool output = true;

            configuration = new Configuration();

            int argCount = args.Length;
            switch (argCount)
            {
                case 1:
                    configuration.BitmapFilePath = args[0];
                    configuration.OutputFilePath = Configuration.DefaultOutputFilePath;
                    break;

                case 2:
                    configuration.BitmapFilePath = args[0];
                    configuration.OutputFilePath = args[1];
                    break;

                default:
                    output = false;

                    Configuration.DisplayUsage(interactive);
                    break;
            }

            return output;
        }

        public static void DisplayUsage(IOutputStream interactive)
        {
            string programName = Constants.ProgramName;
            interactive.WriteLineAndBlankLine(programName);

            string line;
            line = String.Format(@"Usage: {0}.exe [Bitmap file path] (optional)[Output file path = {1}]", programName, Configuration.DefaultOutputFilePath);
            interactive.WriteLineAndBlankLine(line);

            line = @"NB: Remember to put quotes """" around the file paths if there are spaces in the file path.";
            interactive.WriteLineAndBlankLine(line);
        }

        #endregion



        public string BitmapFilePath { get; private set; }
        public string OutputFilePath { get; private set; }
    }
}
