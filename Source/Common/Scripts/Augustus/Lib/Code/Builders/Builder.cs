using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

using Public.Common.Lib;
using Public.Common.Lib.Extensions;
using Public.Common.Lib.IO;
using Public.Common.Lib.IO.Extensions;
using Public.Common.Lib.Logging;


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
            Builder.BuildersByOS = Builder.GetDefaultBuildersByOS();
        }

        public static Dictionary<OsEnvironment, IBuilder> GetDefaultBuildersByOS()
        {
            Dictionary<OsEnvironment, IBuilder> output = new Dictionary<OsEnvironment, IBuilder>();
            output.Add(OsEnvironment.Cygwin, new CygwinBuilder());
            output.Add(OsEnvironment.Windows, new MSBuildBuilder());

            return output;
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

        public static bool RunBuildCommandAndDetermineSuccess(string buildCommand, string successRegexPattern, IOutputStream outputStream, IOutputStream errorStream)
        {
            StringListOutputStream determineSuccessOutputStream = new StringListOutputStream();
            MultipleOutputStream multipleOutputStream = new MultipleOutputStream(new IOutputStream[] { determineSuccessOutputStream, outputStream });

            Builder.RunBuildCommand(buildCommand, multipleOutputStream, errorStream);

            bool output = Builder.DetermineSuccess(successRegexPattern, determineSuccessOutputStream);
            return output;
        }

        private static bool DetermineSuccess(string successRegexPattern, StringListOutputStream stringListOutputStream)
        {
            Regex successRegex = new Regex(successRegexPattern);

            bool output = false;
            foreach (string line in stringListOutputStream.Lines)
            {
                if (successRegex.IsMatch(line))
                {
                    output = true;
                    break;
                }
            }

            return output;
        }

        private static void RunBuildCommand(string buildCommand, IOutputStream outputStream, IOutputStream errorStream)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo(Builder.CommandShellExecutableName);
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardInput = true;

            Process process = new Process();
            process.StartInfo = startInfo;
            process.OutputDataReceived += (sender, e) => { Builder.ProcessDataReceived(sender, e, outputStream); };
            process.ErrorDataReceived += (sender, e) => { Builder.ProcessDataReceived(sender, e, errorStream); };

            process.Start();
            StreamWriter standardInput = process.StandardInput;
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            standardInput.WriteLine(buildCommand);
            standardInput.Close();

            process.WaitForExit();
#if(DEBUG)
            int exitCode = process.ExitCode; // For debugging.
            string line = String.Format(@"Process complete. Exit code: {0}.", exitCode);
            outputStream.WriteLineAndBlankLine(line);
#endif
            process.Close();
        }

        private static void ProcessDataReceived(object sender, DataReceivedEventArgs e, IOutputStream stream)
        {
            if (!String.IsNullOrEmpty(e.Data))
            {
                stream.WriteLine(e.Data);
            }
        }

        public static bool RunBuildInfo(BuildInfo buildInfo, IOutputStream outputStream, IOutputStream errorStream)
        {
            bool output = Builder.RunBuildCommandAndDetermineSuccess(buildInfo.BuildCommand, buildInfo.SuccessRegexPattern, outputStream, errorStream);
            return output;
        }

        public static bool RunBuildInfo(BuildInfo buildInfo, string outputLogFilePath, string errorLogFilePath)
        {
            bool output = Builder.RunBuildInfo(buildInfo, new FileOutputStream(outputLogFilePath), new FileOutputStream(errorLogFilePath));
            return output;
        }

        public static bool Run(BuildItem item, IOutputStream outputStream, IOutputStream errorStream)
        {
            BuildInfo info = Builder.GetBuildInfo(item);

            bool output = Builder.RunBuildInfo(info, outputStream, errorStream);
            return output;
        }

        public static bool Run(BuildItem item, string outputLogFilePath, string errorLogFilePath)
        {
            bool output = Builder.Run(item, new FileOutputStream(outputLogFilePath), new FileOutputStream(errorLogFilePath));
            return output;
        }

        public static bool Run(string buildItemSpecification, IOutputStream outputStream, IOutputStream errorStream)
        {
            BuildItem item = BuildItem.Parse(buildItemSpecification);

            bool output = Builder.Run(item, outputStream, errorStream);
            return output;
        }

        public static bool Run(string buildItemSpecification, string outputLogFilePath, string errorLogFilePath)
        {
            bool output = Builder.Run(buildItemSpecification, new FileOutputStream(outputLogFilePath), new FileOutputStream(errorLogFilePath));
            return output;
        }

        #region Miscellaneous

        public static string WriteResultLine(string buildItemPath, bool success)
        {
            string output = String.Format(@"{0} - {1}", success, buildItemPath);
            return output;
        }

        private static bool DetermineSuccessFromOutputLogFile(string successRegexPattern, string outputLogFilePath)
        {
            Regex successRegex = new Regex(successRegexPattern);

            bool output = false;

            if (File.Exists(outputLogFilePath))
            {
                using (StreamReader fileReader = new StreamReader(outputLogFilePath))
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

        #endregion

        #endregion
    }
}
