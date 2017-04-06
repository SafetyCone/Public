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
        private const string CommandShellExecutableName = @"cmd.exe";
        private const string MSBuildExecutablePath = @"C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe";
        private const string MSBuildShellCommandMask = @"/C > ""{2}"" ""{0}"" ""{1}""";
        private const string MSBuildSuccessRegexPattern = @"^Build succeeded.";
        private const string CygwinBinDirectoryPath = @"C:\Tools\Cygwin64\bin";
        private const string CygwinShellCommandMask = @"/C set PATH={0};%PATH% && make --directory ""{1}"" clean && > ""{2}"" make --directory ""{1}""";// > {2}";
        private const string CygwinSucessRegexPattern = @"^Done!";
        private const string LogFileSuffix = @"Build.log";
        private const string ErrorFileSuffix = @"Build Error.log";
        private const string ResultsFileName = @"Augustust Results.txt";
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

        private static List<string> GetBuildItemSpecifications()
        {
            string[] lines = File.ReadAllLines(Program.BuildFileListFileRelativePath);

            List<string> output = new List<string>(lines);
            return output;
        }

        private static Dictionary<string, bool> RunBuildItems(IEnumerable<IBuildItem> buildItems)
        {
            var output = new Dictionary<string, bool>();
            foreach (IBuildItem buildItem in buildItems)
            {
                Program.RunBuildItem(buildItem);

                bool success = Program.DetermineSuccess(buildItem);
                output.Add(buildItem.BuildFilePath, success);
            }

            return output;
        }

        private static bool DetermineSuccess(IBuildItem buildItem)
        {
            Platform platform = buildItem.Platform;

            string successRegexPattern;
            switch (platform)
            {
                case Platform.Cygwin:
                    successRegexPattern = Program.CygwinSucessRegexPattern;
                    break;

                case Platform.Windows:
                    successRegexPattern = Program.MSBuildSuccessRegexPattern;
                    break;

                default:
                    throw new UnexpectedEnumerationValueException<Platform>(platform);
            }

            Regex successRegex = new Regex(successRegexPattern);

            bool output = false;

            string buildLogFilePath = Program.GetBuildLogFilePath(buildItem);
            if (File.Exists(buildLogFilePath))
            {
                using (StreamReader fileReader = new StreamReader(buildLogFilePath))
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

        private static void RunBuildItem(IBuildItem buildItem)
        {
            Platform platform = buildItem.Platform;

            switch (platform)
            {
                case Platform.Cygwin:
                    Program.RunCygwinBuildItem(buildItem);
                    break;

                case Platform.Windows:
                    Program.RunWindowsBuildItem(buildItem);
                    break;

                default:
                    throw new UnexpectedEnumerationValueException<Platform>(platform);
            }
        }

        private static void RunWindowsBuildItem(IBuildItem buildItem)
        {
            WindowsBuildItem windowsBuildItem = (WindowsBuildItem)buildItem;

            string buildLogFilePath = Program.GetBuildLogFilePath(buildItem);

            string command = String.Format(Program.MSBuildShellCommandMask, Program.MSBuildExecutablePath, buildItem.BuildFilePath, buildLogFilePath);

            ProcessStartInfo startInfo = new ProcessStartInfo(Program.CommandShellExecutableName, command);
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            startInfo.RedirectStandardOutput = true; // Required to make the command run to completion.

            Process proc = Process.Start(startInfo);
            Console.WriteLine(proc.StandardOutput.ReadToEnd()); // Required to make the command run to completion, even though build text WILL be outputted to the log file.
            proc.WaitForExit();

            int exitCode = proc.ExitCode;
            proc.Close();
        }

        private static void RunCygwinBuildItem(IBuildItem buildItem)
        {
            CygwinBuildItem cygwinBuildItem = (CygwinBuildItem)buildItem;

            string directoryForMake = Path.GetDirectoryName(buildItem.BuildFilePath);

            string buildLogFilePath = Program.GetBuildLogFilePath(buildItem);
            string buildErrorLogFilePath = Program.GetBuildErrorLogFilePath(buildItem);

            string command = String.Format(Program.CygwinShellCommandMask, Program.CygwinBinDirectoryPath, directoryForMake, buildLogFilePath);

            ProcessStartInfo startInfo = new ProcessStartInfo(Program.CommandShellExecutableName, command);
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            startInfo.RedirectStandardOutput = true; // Required to make the command run to completion.

            Process proc = Process.Start(startInfo);

            Console.WriteLine(proc.StandardOutput.ReadToEnd()); // Required to make the command run to completion, even though build text WILL be outputted to the log file.
            proc.WaitForExit();

            int exitCode = proc.ExitCode;
            proc.Close();
        }

        private static string GetBuildLogFilePath(IBuildItem buildItem)
        {
            string output = Program.GetLogFilePath(buildItem, Program.LogFileSuffix);
            return output;
        }

        private static string GetBuildErrorLogFilePath(IBuildItem buildItem)
        {
            string output = Program.GetLogFilePath(buildItem, Program.ErrorFileSuffix);
            return output;
        }

        private static string GetLogFilePath(IBuildItem buildItem, string logFileSuffix)
        {
            string buildFilePath = buildItem.BuildFilePath;
            Platform platform = buildItem.Platform;

            string buildLogFileName;
            switch(platform)
            {
                case Platform.Cygwin:
                    buildLogFileName = logFileSuffix;
                    break;

                case Platform.Windows:
                    string buildFileNameNoExtension = Path.GetFileNameWithoutExtension(buildFilePath);
                    buildLogFileName = String.Format(@"{0} {1}", buildFileNameNoExtension, logFileSuffix);
                    break;

                default:
                    throw new UnexpectedEnumerationValueException<Platform>(platform);
            }

            string directoryPath = Path.GetDirectoryName(buildFilePath);

            string output = Path.Combine(directoryPath, buildLogFileName);
            return output;
        }

        private static List<IBuildItem> GetBuildItems(IEnumerable<string> buildItemSpecifications)
        {
            var output = new List<IBuildItem>();
            foreach (string buildItemSpecification in buildItemSpecifications)
            {
                var buildItem = BuildItemFactory.GetBuildItem(buildItemSpecification);
                output.Add(buildItem);
            }

            return output;
        }
    }
}
