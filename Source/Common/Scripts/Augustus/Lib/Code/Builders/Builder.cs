using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

using Public.Common.Lib;
using Public.Common.Lib.Extensions;


namespace Public.Common.Augustus.Lib
{
    public class Builder
    {
        public const string CommandShellExecutableName = @"cmd.exe";

        public const string OutputLogFileSuffix = @"Build.log";
        public const string ErrorLogFileSuffix = @"Build Error.log";


        #region Static

        private static Dictionary<OsEnvironment, IBuilder> BuildersByOS;


        static Builder()
        {
            Dictionary<OsEnvironment, IBuilder> builders = new Dictionary<OsEnvironment, IBuilder>();
            Builder.BuildersByOS = builders;

            builders.Add(OsEnvironment.Cygwin, new CygwinBuilder());
            builders.Add(OsEnvironment.Windows, new MSBuildBuilder());
        }

        public static BuildInfo GetBuildInfo(BuildItem buildItem)
        {
            OsEnvironment osEnvironment = buildItem.OsEnvironment;

            BuildInfo output;
            if (Builder.BuildersByOS.ContainsKey(buildItem.OsEnvironment))
            {
                IBuilder builderForOs = Builder.BuildersByOS[buildItem.OsEnvironment];
                output = builderForOs.GetBuildInfo(buildItem);
            }
            else
            {
                throw new UnexpectedEnumerationValueException<OsEnvironment>(osEnvironment);
            }

            return output;
        }

        public static void Run(BuildInfo info, StreamWriter standardOutput, StreamWriter standardError)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo(Builder.CommandShellExecutableName);
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardInput = true;

            Process process = new Process();
            process.StartInfo = startInfo;
            process.ErrorDataReceived += (sender, e) => { Builder.ProcessDataReceived(sender, e, standardError); };
            process.OutputDataReceived += (sender, e) => { Builder.ProcessDataReceived(sender, e, standardOutput); };

            process.Start();
            StreamWriter standardInput = process.StandardInput;
            process.BeginErrorReadLine();
            process.BeginOutputReadLine();

            standardInput.WriteLine(info.BuildCommand);
            standardInput.Close();

            process.WaitForExit();
#if(DEBUG)
            int exitCode = process.ExitCode; // For debugging.
            string line = String.Format(@"Process complete. Exit code: {0}.", exitCode);
            standardOutput.WriteLineAndBlankLine(line);
#endif
            process.Close();
        }

        private static void ProcessDataReceived(object sender, DataReceivedEventArgs e, StreamWriter output)
        {
            if (!String.IsNullOrEmpty(e.Data))
            {
                output.WriteLine(e.Data);
            }
        }

        private static bool Run(BuildInfo info)
        {
            using (StreamWriter standardOutput = new StreamWriter(info.OutputLogPath))
            using (StreamWriter standardError = new StreamWriter(info.ErrorLogPath))
            {
                Builder.Run(info, standardOutput, standardError);
            }

            bool output = Builder.DetermineSuccess(info);
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

        public static Dictionary<string, bool> Run(IEnumerable<BuildItem> buildItems, TextWriter inspectionStream)
        {
            Dictionary<string, bool> output = new Dictionary<string, bool>();
            foreach (BuildItem buildItem in buildItems)
            {
                BuildInfo info = Builder.GetBuildInfo(buildItem);

                bool success = Builder.Run(info);

                string buildFilePath = buildItem.FilePath;
                output.Add(buildFilePath, success);

                string inspectionMessage = Builder.WriteResultLine(buildFilePath, success);
                inspectionStream.WriteLine(inspectionMessage);
            }

            return output;
        }

        public static string WriteResultLine(string buildItemPath, bool success)
        {
            string output = String.Format(@"{0} - {1}", success, buildItemPath);
            return output;
        }

        public static bool Run(string buildItemSpecification)
        {
            BuildItem item = BuildItem.Parse(buildItemSpecification);
            BuildInfo info = Builder.GetBuildInfo(item);

            bool output = Builder.Run(info);
            return output;
        }

        public static Dictionary<string, bool> RunBuildListFile(string buildListFilePath, TextWriter outputStream)
        {
            List<BuildItem> items = BuildItemTextFile.GetBuildItems(buildListFilePath);

            Dictionary<string, bool> output = Builder.Run(items, Console.Out);
            return output;
        }

        public static Dictionary<string, bool> RunBuildListFile(string buildListFilePath)
        {
            Dictionary<string, bool> output = Builder.RunBuildListFile(buildListFilePath, Console.Out);
            return output;
        }

        #endregion
    }
}
