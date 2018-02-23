using System;
using System.Diagnostics;


namespace Public.Common.Lib.OS
{
    public class ProcessStarter
    {
        #region Static

        public static void StartProcess(string executablePath, string arguments)
        {
            ProcessStartInfo info = new ProcessStartInfo(executablePath, arguments);
            Process.Start(info);
        }

        public static void RunProcess(string executableFilePath, string arguments)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo(executableFilePath, arguments)
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                RedirectStandardInput = true
            };

            Process process = new Process
            {
                StartInfo = startInfo
            };
            process.OutputDataReceived += (sender, e) => { ProcessStarter.ProcessDataReceived(sender, e); };
            process.ErrorDataReceived += (sender, e) => { ProcessStarter.ProcessDataReceived(sender, e); };

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            //StreamWriter standardInput = process.StandardInput;
            //standardInput.WriteLine(executableFilePath);
            //standardInput.Close();

            process.WaitForExit();
#if (DEBUG)
            int exitCode = process.ExitCode; // For debugging.
            string line = String.Format(@"Process complete. Exit code: {0}.", exitCode);
            Console.WriteLine(line);
#endif
            process.Close();
        }

        private static void ProcessDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!String.IsNullOrEmpty(e.Data))
            {
                Console.WriteLine(e.Data);
            }
        }

        #endregion
    }
}
