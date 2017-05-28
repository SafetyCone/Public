using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

using Public.Common.Lib.Extensions;
using Public.Common.Lib.IO;
using Public.Common.Lib.IO.Email;
using Public.Common.Lib.IO.Extensions;
using Public.Common.Lib.Logging;
using Public.Common.Lib.Production;
using Public.Common.Lib.Security;

using Public.Common.Augustus.Lib;


namespace Public.Common.Augustus
{
    class Program
    {
        static void Main(string[] args)
        {
            Program.SubMain(args);
            //Testing.SubMain();
            //Construction.SubMain(args);
        }

        public static void SubMain(string[] args)
        {
            string programName = Constants.ProgramName;
            DateTime runTime = DateTime.Now;

            string outputDirectoryPath = Production.GetProgramRunOutputDirectoryPath(programName, runTime);

            Paths paths = new Paths(outputDirectoryPath, programName, runTime);
            paths.EnsurePathsCreated();

            ConsoleOutputStream console = new ConsoleOutputStream();
            DebugOutputStream debug = new DebugOutputStream();
            MultipleOutputStream debugAndConsole = new MultipleOutputStream(new IOutputStream[] { console, debug });
            debugAndConsole.WriteLineAndBlankLine(Constants.ProgramName);

            string logFilePath = paths.LogFilePath;
            debugAndConsole.WriteLine(@"Log file path:");
            debugAndConsole.WriteLineAndBlankLine(logFilePath);

            Log log = new Log(logFilePath);
            MultipleOutputStream logAndConsole = new MultipleOutputStream(new IOutputStream[] { log.OutputStream, console });

            Configuration config;
            if (Configuration.ParseArguments(out config, paths, args, logAndConsole))
            {
                Dictionary<string, bool> successByBuildItemPath = Program.RunBuildListFile(config, logAndConsole);

                Program.CreateOutputDirectory(config.Paths.ResultsFilePath);
                Program.WriteResults(config.Paths.ResultsFilePath, successByBuildItemPath);

                if (config.OpenResults)
                {
                    Program.OpenResults(config.Paths.ResultsFilePath);
                }

                if(config.EmailResults)
                {
                    Program.SendResultsEmail(config.Paths.ResultsFilePath, successByBuildItemPath);
                }
            }   
        }

        private static Dictionary<string, bool> RunBuildListFile(Configuration config, IOutputStream logAndConsole)
        {
            logAndConsole.WriteLine(@"Loading build list file...");
            List<BuildItem> buildItems = BuildItemTextFile.GetBuildItems(config.BuildListFilePath);
            logAndConsole.WriteLineAndBlankLine(String.Format(@"Loaded build list file. Found {0} build items.", buildItems.Count));

            Dictionary<string, bool> output = new Dictionary<string, bool>();
            foreach (BuildItem buildItem in buildItems)
            {
                string buildOutputLogFilePath = Program.GetOutputLogFilePath(config.Paths.LogsDirectoryPath, buildItem);
                string buildErrorLogFilePath = Program.GetErrorLogFilePath(config.Paths.LogsDirectoryPath, buildItem);

                logAndConsole.WriteLine(String.Format(@"Building file: {0}", buildItem.FilePath));
                logAndConsole.WriteLine(String.Format(@"Output log file: {0}", buildOutputLogFilePath));
                logAndConsole.WriteLine(String.Format(@"Error log file: {0}", buildErrorLogFilePath));

                bool success = Builder.Run(buildItem, new FileOutputStream(buildOutputLogFilePath), new FileOutputStream(buildErrorLogFilePath));

                logAndConsole.WriteLineAndBlankLine(String.Format(@"Success? {0}.", success));

                output.Add(buildItem.FilePath, success);
            }

            return output;
        }

        private static string GetOutputLogFilePath(string logsDirectoryPath, BuildItem buildItem)
        {
            string output;
            switch (buildItem.OsEnvironment)
            {
                case OsEnvironment.Cygwin:
                    output = CygwinBuilder.GetDefaultOutputLogFilePath(buildItem.FilePath);
                    break;

                default:
                    // Windows.
                    output = MSBuildBuilder.GetDefaultOutputLogFilePath(buildItem.FilePath);
                    break;
            }

            return output;
        }

        private static string GetErrorLogFilePath(string logsDirectoryPath, BuildItem buildItem)
        {
            string output;
            switch (buildItem.OsEnvironment)
            {
                case OsEnvironment.Cygwin:
                    output = CygwinBuilder.GetDefaultErrorLogFilePath(buildItem.FilePath);
                    break;

                default:
                    // Windows.
                    output = MSBuildBuilder.GetDefaultErrorLogFilePath(buildItem.FilePath);
                    break;
            }

            return output;
        }

        private static void SendResultsEmail(string outputFilePath, Dictionary<string, bool> successByBuildItemPath)
        {
            // Get authentication for GMail.
            Dictionary<string, Authentication> auths = AuthenticationsTextFile.DeserializeTextAuthenticationsByName();
            Authentication gmailAuth = auths[Authentication.MinexGmailAuthenticationName];

            // Build the output.
            StringBuilder results = Program.WriteResults(successByBuildItemPath);

            string subject = String.Format(@"Augustus Build - {0}", DateTime.Today.ToYYYYMMDDStr());
            string labeledSubject = Gmail.ApplyAutomationLabel(subject);
            string body = results.ToString();

            Gmail.SendEmail(gmailAuth.UserName, gmailAuth.UserName, labeledSubject, body, gmailAuth, new string[] { outputFilePath });
        }

        private static void CreateOutputDirectory(string outputFilePath)
        {
            string directoryPath = Path.GetDirectoryName(outputFilePath);
            if(!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }

        public static void OpenResults(string outputFilePath)
        {
            Process.Start(Constants.ResultsViewerPath, outputFilePath);
        }

        public static void OpenResults()
        {
            string outputFilePath = Program.GetResultsFilePath();
            Program.OpenResults(outputFilePath);   
        }

        public static void WriteResults(string outputFilePath, Dictionary<string, bool> successByBuildItemPath)
        {
            StringBuilder results = Program.WriteResults(successByBuildItemPath);
            using (StreamWriter writer = new StreamWriter(outputFilePath))
            {
                writer.Write(results.ToString());
            }
        }

        public static StringBuilder WriteResults(Dictionary<string, bool> successByBuildItemPath)
        {
            StringBuilder output = new StringBuilder();
            foreach (string buildItemPath in successByBuildItemPath.Keys)
            {
                bool success = successByBuildItemPath[buildItemPath];

                string line = Builder.WriteResultLine(buildItemPath, success);
                output.AppendLine(line);
            }

            return output;
        }

        private static string GetResultsFilePath()
        {
            string currentDirectory = Directory.GetCurrentDirectory();

            string output = Path.Combine(currentDirectory, Constants.ResultsFileName);
            return output;
        }
    }
}
