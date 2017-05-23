using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

using Public.Common.Lib;
using Public.Common.Lib.Extensions;
using Public.Common.Lib.IO.Email;
using Public.Common.Lib.Security;


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
                List<BuildItem> buildItems = Program.GetBuildItems(config.BuildListFilePath);
                Dictionary<string, bool> successByBuildItemPath = Program.RunBuildItems(buildItems, outputStream);

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

                string line = Program.WriteResultLine(buildItemPath, success);
                output.AppendLine(line);
            }

            return output;
        }

        private static string WriteResultLine(string buildItemPath, bool success)
        {
            string output = String.Format(@"{0} - {1}", success, buildItemPath);
            return output;
        }

        private static string GetResultsFilePath()
        {
            string currentDirectory = Directory.GetCurrentDirectory();

            string output = Path.Combine(currentDirectory, Constants.ResultsFileName);
            return output;
        }

        private static bool DetermineSuccess(BuildInfo buildInfo)
        {
            Regex successRegex = new Regex(buildInfo.SuccessRegexPattern);

            bool output = false;

            string outputLogPath = buildInfo.OutputLogPath;
            if (File.Exists(outputLogPath))
            {
                using (StreamReader fileReader = new StreamReader(outputLogPath))
                {
                    while (!fileReader.EndOfStream)
                    {
                        string line = fileReader.ReadLine();
                        if (successRegex.IsMatch(line))
                        {
                            output = true;
                        }
                    }
                }
            }

            return output;
        }

        private static bool RunBuildItem(BuildInfo info)
        {
            using (StreamWriter standardOutput = new StreamWriter(info.OutputLogPath))
            using (StreamWriter standardError = new StreamWriter(info.ErrorLogPath))
            {
                Program.RunBuildItem(info, standardOutput, standardError);
            }

            bool output = Program.DetermineSuccess(info);
            return output;
        }

        private static void RunBuildItem(BuildInfo info, StreamWriter standardOutput, StreamWriter standardError)
        {
            //ProcessStartInfo startInfo = new ProcessStartInfo(Program.CommandShellExecutableName, info.BuildCommand);
            ProcessStartInfo startInfo = new ProcessStartInfo(Constants.CommandShellExecutableName);
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardInput = true;

            Process process = new Process();
            process.StartInfo = startInfo;
            process.ErrorDataReceived += (sender, e) => { Program.ProcessDataReceived(sender, e, standardError); };
            process.OutputDataReceived += (sender, e) => { Program.ProcessDataReceived(sender, e, standardOutput); };

            process.Start();
            StreamWriter standardInput = process.StandardInput;
            process.BeginErrorReadLine();
            process.BeginOutputReadLine();

            standardInput.WriteLine(info.BuildCommand);
            standardInput.Close();

            process.WaitForExit();
#if(DEBUG)
            int exitCode = process.ExitCode; // For debugging.
#endif
            process.Close();
        }

        private static void ProcessDataReceived(object sender, DataReceivedEventArgs e, StreamWriter output)
        {
            if(!String.IsNullOrEmpty(e.Data))
            {
                output.WriteLine(e.Data);
            }
        }

        private static BuildInfo GetWindowsBuildInfo(BuildItem buildItem)
        {
            string buildFileNameNoExtension = Path.GetFileNameWithoutExtension(buildItem.BuildFilePath);
            string buildDirectoryPath = Path.GetDirectoryName(buildItem.BuildFilePath);
            string outputLogFileName = String.Format(@"{0} {1}", buildFileNameNoExtension, Constants.OutputLogFileSuffix);
            string outputLogPath = Path.Combine(buildDirectoryPath, outputLogFileName);
            string errorLogFileName = String.Format(@"{0} {1}", buildFileNameNoExtension, Constants.ErrorLogFileSuffix);
            string errorLogPath = Path.Combine(buildDirectoryPath, errorLogFileName);

            string successRegexPattern = Constants.MSBuildSuccessRegexPattern;

            string buildCommand = String.Format(Constants.MSBuildShellCommandMask, Constants.MSBuildExecutablePath, buildItem.BuildFilePath);

            var output = new BuildInfo(outputLogPath, errorLogPath, successRegexPattern, buildCommand);
            return output;
        }

        private static BuildInfo GetCygwinBuildInfo(BuildItem buildItem)
        {
            string buildDirectoryPath = Path.GetDirectoryName(buildItem.BuildFilePath);
            string outputLogPath = Path.Combine(buildDirectoryPath, Constants.OutputLogFileSuffix);
            string errorLogPath = Path.Combine(buildDirectoryPath, Constants.ErrorLogFileSuffix);

            string successRegexPattern = Constants.CygwinSucessRegexPattern;

            string directoryForMake = Path.GetDirectoryName(buildItem.BuildFilePath);
            string buildCommand = String.Format(Constants.CygwinShellCommandMask, Constants.CygwinBinDirectoryPath, directoryForMake);

            var output = new BuildInfo(outputLogPath, errorLogPath, successRegexPattern, buildCommand);
            return output;
        }

        private static BuildInfo GetBuildInfo(BuildItem buildItem)
        {
            OsEnvironment osEnvironment = buildItem.OsEnvironment;

            BuildInfo output;
            switch (osEnvironment)
            {
                case OsEnvironment.Cygwin:
                    output = Program.GetCygwinBuildInfo(buildItem);
                    break;

                case OsEnvironment.Windows:
                    output = Program.GetWindowsBuildInfo(buildItem);
                    break;

                default:
                    throw new UnexpectedEnumerationValueException<OsEnvironment>(osEnvironment);
            }

            return output;
        }

        public static Dictionary<string, bool> RunBuildItems(IEnumerable<BuildItem> buildItems, TextWriter inspectionStream)
        {
            var output = new Dictionary<string, bool>();
            foreach (BuildItem buildItem in buildItems)
            {
                BuildInfo info = Program.GetBuildInfo(buildItem);

                string buildFilePath = buildItem.BuildFilePath;
                bool success = Program.RunBuildItem(info);
                output.Add(buildFilePath, success);

                string inspectionMessage = Program.WriteResultLine(buildFilePath, success);
                inspectionStream.WriteLine(inspectionMessage);
            }

            return output;
        }

        private static List<BuildItem> GetBuildItems(string buildListFilePath)
        {
            List<string> buildItemSpecifications = Program.GetBuildItemSpecifications(buildListFilePath);

            List<BuildItem> output = Program.GetBuildItems(buildItemSpecifications);
            return output;
        }

        public static List<BuildItem> GetBuildItems(IEnumerable<string> buildItemSpecifications)
        {
            var output = new List<BuildItem>();
            foreach (string buildItemSpecification in buildItemSpecifications)
            {
                var buildItem = BuildItem.Parse(buildItemSpecification);
                output.Add(buildItem);
            }

            return output;
        }

        public static List<string> GetBuildItemSpecifications(string buildListFilePath)
        {
            string[] lines = File.ReadAllLines(buildListFilePath);

            var output = new List<string>();
            foreach (string line in lines)
            {
                if (!String.IsNullOrEmpty(line))
                {
                    output.Add(line);
                }
            }

            return output;
        }
    }
}
