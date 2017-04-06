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

        private static Dictionary<string, bool> RunBuildItems(IEnumerable<BuildItem> buildItems)
        {
            var output = new Dictionary<string, bool>();
            foreach (BuildItem buildItem in buildItems)
            {
                Program.RunBuildItem(buildItem);

                bool success = Program.DetermineSuccess(buildItem);
                output.Add(buildItem.BuildFilePath, success);
            }

            return output;
        }

        private static bool DetermineSuccess(BuildItem buildItem)
        {
            OsEnvironment platform = buildItem.Platform;

            string successRegexPattern;
            switch (platform)
            {
                case OsEnvironment.Cygwin:
                    successRegexPattern = Program.CygwinSucessRegexPattern;
                    break;

                case OsEnvironment.Windows:
                    successRegexPattern = Program.MSBuildSuccessRegexPattern;
                    break;

                default:
                    throw new UnexpectedEnumerationValueException<OsEnvironment>(platform);
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

        private static void RunBuildItem(BuildItem buildItem)
        {
            OsEnvironment platform = buildItem.Platform;

            switch (platform)
            {
                case OsEnvironment.Cygwin:
                    Program.RunCygwinBuildItem(buildItem);
                    break;

                case OsEnvironment.Windows:
                    Program.RunWindowsBuildItem(buildItem);
                    break;

                default:
                    throw new UnexpectedEnumerationValueException<OsEnvironment>(platform);
            }
        }

        private static void RunWindowsBuildItem(BuildItem buildItem)
        {
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

        private static void RunCygwinBuildItem(BuildItem buildItem)
        {
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

        private static string GetBuildLogFilePath(BuildItem buildItem)
        {
            string output = Program.GetLogFilePath(buildItem, Program.LogFileSuffix);
            return output;
        }

        private static string GetBuildErrorLogFilePath(BuildItem buildItem)
        {
            string output = Program.GetLogFilePath(buildItem, Program.ErrorFileSuffix);
            return output;
        }

        private static string GetLogFilePath(BuildItem buildItem, string logFileSuffix)
        {
            string buildFilePath = buildItem.BuildFilePath;
            OsEnvironment platform = buildItem.Platform;

            string buildLogFileName;
            switch(platform)
            {
                case OsEnvironment.Cygwin:
                    buildLogFileName = logFileSuffix;
                    break;

                case OsEnvironment.Windows:
                    string buildFileNameNoExtension = Path.GetFileNameWithoutExtension(buildFilePath);
                    buildLogFileName = String.Format(@"{0} {1}", buildFileNameNoExtension, logFileSuffix);
                    break;

                default:
                    throw new UnexpectedEnumerationValueException<OsEnvironment>(platform);
            }

            string directoryPath = Path.GetDirectoryName(buildFilePath);

            string output = Path.Combine(directoryPath, buildLogFileName);
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
            string buildListFileRelativePath = Program.DebugBuildFileListFileRelativePath;
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
