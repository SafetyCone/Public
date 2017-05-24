using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

using Public.Common.Lib.Extensions;
using Public.Common.Lib.IO.Email;
using Public.Common.Lib.Security;

using Public.Common.Augustus.Lib;


namespace Public.Common.Augustus
{
    class Program
    {
        static void Main(string[] args)
        {
            //Program.SubMain(args);
            Testing.SubMain();
            //Construction.SubMain();
        }

        public static void SubMain(string[] args)
        {
            TextWriter outputStream = Console.Out;

            Configuration config;
            if(Configuration.ParseArguments(out config, args, outputStream))
            {
                Dictionary<string, bool> successByBuildItemPath = Builder.RunBuildListFile(config.BuildListFilePath, outputStream);

                Program.CreateOutputDirectory(config.OutputFilePath);
                Program.WriteResults(config.OutputFilePath, successByBuildItemPath);

                if (config.OpenResults)
                {
                    Program.OpenResults(config.OutputFilePath);
                }

                if(config.EmailResults)
                {
                    Program.SendResultsEmail(config.OutputFilePath, successByBuildItemPath);
                }
            }   
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
