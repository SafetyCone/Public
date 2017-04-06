using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

using Public.Common.Lib;


namespace Augustus
{
    class Program
    {
        private const string BuildFileListFileRelativePath = @"..\..\..\..\..\..\..\..\..\Data\Augustus Build File List.txt";
        private const string DebugBuildFileListFileRelativePath = @"..\..\..\..\..\..\..\..\..\Data\Augustus Build File List - Debug.txt";
        private const string CommandShellExecutableName = @"cmd.exe";
        private const string MSBuildExecutablePath = @"C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe";
        private const string MSBuildShellCommandMask = @"""{0}"" ""{1}""";
        private const string MSBuildSuccessRegexPattern = @"^Build succeeded.";
        private const string CygwinBinDirectoryPath = @"C:\Tools\Cygwin64\bin";
        private const string CygwinShellCommandMask = @"set PATH={0};%PATH% & make --directory ""{1}"" clean & make --directory ""{1}""";
        private const string CygwinSucessRegexPattern = @"^Done!";
        private const string OutputLogFileSuffix = @"Build.log";
        private const string ErrorLogFileSuffix = @"Build Error.log";
        private const string ResultsFileName = @"Augustus Results.txt";
        private const string ResultsViewerPath = @"C:\Program Files (x86)\Notepad++\notepad++.exe";


        static void Main(string[] args)
        {
            Program.SubMain();
        }

        private static void SubMain()
        {
            var buildItemSpecifications = Program.GetBuildItemSpecifications();
            var buildItems = Program.GetBuildItems(buildItemSpecifications);
            var successByBuildItemPath = Program.RunBuildItems(buildItems);

            Program.WriteResults(successByBuildItemPath);
            Program.OpenResults();
        }

        private static void OpenResults()
        {
            string resultsFilePath = Program.GetResultsFilePath();

            Process.Start(Program.ResultsViewerPath, resultsFilePath);
        }

        private static void WriteResults(Dictionary<string, bool> successByBuildItemPath)
        {
            string resultsFilePath = Program.GetResultsFilePath();

            using (StreamWriter writer = new StreamWriter(resultsFilePath))
            {
                foreach (string buildItemPath in successByBuildItemPath.Keys)
                {
                    bool success = successByBuildItemPath[buildItemPath];

                    string line = String.Format(@"{0} - {1}", success, buildItemPath);
                    writer.WriteLine(line);
                }
            }
        }

        private static string GetResultsFilePath()
        {
            string currentDirectory = Directory.GetCurrentDirectory();

            string output = Path.Combine(currentDirectory, Program.ResultsFileName);
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
            ProcessStartInfo startInfo = new ProcessStartInfo(Program.CommandShellExecutableName);
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
            string outputLogFileName = String.Format(@"{0} {1}", buildFileNameNoExtension, Program.OutputLogFileSuffix);
            string outputLogPath = Path.Combine(buildDirectoryPath, outputLogFileName);
            string errorLogFileName = String.Format(@"{0} {1}", buildFileNameNoExtension, Program.ErrorLogFileSuffix);
            string errorLogPath = Path.Combine(buildDirectoryPath, errorLogFileName);

            string successRegexPattern = Program.MSBuildSuccessRegexPattern;

            string buildCommand = String.Format(Program.MSBuildShellCommandMask, Program.MSBuildExecutablePath, buildItem.BuildFilePath);

            var output = new BuildInfo(outputLogPath, errorLogPath, successRegexPattern, buildCommand);
            return output;
        }

        private static BuildInfo GetCygwinBuildInfo(BuildItem buildItem)
        {
            string buildDirectoryPath = Path.GetDirectoryName(buildItem.BuildFilePath);
            string outputLogPath = Path.Combine(buildDirectoryPath, Program.OutputLogFileSuffix);
            string errorLogPath = Path.Combine(buildDirectoryPath, Program.ErrorLogFileSuffix);

            string successRegexPattern = Program.CygwinSucessRegexPattern;

            string directoryForMake = Path.GetDirectoryName(buildItem.BuildFilePath);
            string buildCommand = String.Format(Program.CygwinShellCommandMask, Program.CygwinBinDirectoryPath, directoryForMake);

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

        private static Dictionary<string, bool> RunBuildItems(IEnumerable<BuildItem> buildItems)
        {
            var output = new Dictionary<string, bool>();
            foreach (BuildItem buildItem in buildItems)
            {
                BuildInfo info = Program.GetBuildInfo(buildItem);

                bool success = Program.RunBuildItem(info);
                output.Add(buildItem.BuildFilePath, success);
            }

            return output;
        }

        private static List<BuildItem> GetBuildItems(IEnumerable<string> buildItemSpecifications)
        {
            var output = new List<BuildItem>();
            foreach (string buildItemSpecification in buildItemSpecifications)
            {
                var buildItem = BuildItem.Parse(buildItemSpecification);
                output.Add(buildItem);
            }

            return output;
        }

        private static List<string> GetBuildItemSpecifications()
        {
#if (DEBUG)
            //string buildListFileRelativePath = Program.DebugBuildFileListFileRelativePath;
            string buildListFileRelativePath = Program.BuildFileListFileRelativePath;
#else
            string buildListFileRelativePath = Program.BuildFileListFileRelativePath;
#endif

            string[] lines = File.ReadAllLines(buildListFileRelativePath);

            var output = new List<string>();
            foreach(string line in lines)
            {
                if(!String.IsNullOrEmpty(line))
                {
                    output.Add(line);
                }
            }

            return output;
        }
    }
}
