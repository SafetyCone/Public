using System;
using System.IO;

using Public.Common.Lib.Extensions;
using Public.Common.Lib.Production;


namespace Public.Common.Granby
{
    public class Configuration
    {
        public const string DefaultScheduledTasksFileName = @"Scheduled Tasks.txt";


        #region Static

        public static string DefaultScheduledTasksFilePath
        {
            get
            {
                string output = Path.Combine(Production.UserConfigurationDataDirectoryPath, Configuration.DefaultScheduledTasksFileName);
                return output;
            }
        }



        public static bool TryParseArguments(out Configuration configuration, TextWriter outputStream, string[] args)
        {
            bool output = true;

            configuration = new Configuration();

            try
            {
                int numArgs = args.Length;

                string scheduledTasksTextFilePath = numArgs > 0 ? args[1] : Configuration.DefaultScheduledTasksFilePath;
                if (!File.Exists(scheduledTasksTextFilePath))
                {
                    string message  = String.Format(@"Scheduled tasks file path does not exist: {0}", scheduledTasksTextFilePath);
                    throw new FileNotFoundException(message, scheduledTasksTextFilePath);
                }

                if (1 < numArgs)
                {
                    string message = String.Format(@"Too many input arguments found: {0}. Expected 1.", numArgs);
                    throw new InvalidOperationException(message);
                }

                configuration.ScheduledTasksFilePath = scheduledTasksTextFilePath;
            }
            catch (Exception ex)
            {
                output = false;
                
                outputStream.WriteLineAndBlankLine(ex.Message);

                Configuration.DisplayUsage(outputStream);
            }

            return output;
        }

        private static void DisplayUsage(TextWriter outputStream)
        {
            outputStream.WriteLineAndBlankLine(Constants.ProgramName);

            string line = String.Format(@"Usage: {0}.exe ScheduledTasksTextFilePath");
            outputStream.WriteLineAndBlankLine(line);
        }

        #endregion


        public string ScheduledTasksFilePath { get; set; }


        public Configuration()
        {
        }

        public Configuration(string scheduledTasksFilePath)
        {
            this.ScheduledTasksFilePath = scheduledTasksFilePath;
        }
    }
}
