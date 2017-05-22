using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

using Public.Common.Lib;
using Public.Common.Lib.Extensions;


namespace Augustus
{
    class Program
    {
        static void Main(string[] args)
        {
            //Construction.SubMain();

            Program.SubMain(args);
        }

        private static void SubMain(string[] args)
        {
            Configuration config = Program.ParseArguments(args);

            List<BuildItem> buildItems = Program.GetBuildItems(config.BuildListFilePath);
            Dictionary<string, bool> successByBuildItemPath = Program.RunBuildItems(buildItems, Console.Out);

            Program.CreateOutputDirectory(config.OutputFilePath);
            Program.WriteResults(config.OutputFilePath, successByBuildItemPath);
            Program.OpenResults(config.OutputFilePath);
        }

        private static void CreateOutputDirectory(string outputFilePath)
        {
            string directoryPath = Path.GetDirectoryName(outputFilePath);
            if(!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
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
        private static Configuration ParseArguments(string[] args)
        {
            Configuration output = new Configuration();

            int argCount = args.Length;
            if(0 == argCount)
            {
                // Use the default paths.
                output.BuildListFilePath = Configuration.DefaultBuildListFilePath;
                output.OutputFilePath = Program.GetDatedDefaultOutputFilePath();
            }

            if(1 == argCount)
            {
                output.BuildListFilePath = Program.VerifyFileExistence(args[0], @"Specified build list file not found: {0}");
                output.OutputFilePath = Program.GetDatedDefaultOutputFilePath();
            }
            
            if(2 == argCount)
            {
                output.BuildListFilePath = Program.VerifyFileExistence(args[0], @"Specified build list file not found: {0}");
                output.OutputFilePath = Program.VerifyFileExistence(args[1], @"Specified output file not found: {0}");
            }

            if(2 < argCount)
            {
                string message = String.Format(@"Too many input arguments. Found: {0}. Usage: Augustus.exe (optional)[build list file path] (optional)[output file path]", argCount);
                throw new InvalidOperationException(message);
            }

            return output;
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
            string output = Program.DateMarkPath(Configuration.DefaultOutputFilePath);
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

        public static void OpenResults(string outputFilePath)
        {
            Process.Start(Constants.ResultsViewerPath, outputFilePath);
        }

        public static void OpenResults()
        {
            string outputFilePath = Program.GetResultsFilePath();
            Program.OpenResults(outputFilePath);   
        }

        private static string WriteResultLine(string buildItemPath, bool success)
        {
            string output = String.Format(@"{0} - {1}", success, buildItemPath);
            return output;
        }

        public static void WriteResults(string outputFilePath, Dictionary<string, bool> successByBuildItemPath)
        {
            using (StreamWriter writer = new StreamWriter(outputFilePath))
            {
                foreach (string buildItemPath in successByBuildItemPath.Keys)
                {
                    bool success = successByBuildItemPath[buildItemPath];

                    string line = Program.WriteResultLine(buildItemPath, success);
                    writer.WriteLine(line);
                }
            }
        }

        public static void WriteResults(Dictionary<string, bool> successByBuildItemPath)
        {
            string outputFilePath = Program.GetResultsFilePath();

            Program.WriteResults(outputFilePath, successByBuildItemPath);
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
