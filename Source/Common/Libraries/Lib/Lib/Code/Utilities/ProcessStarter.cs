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

        #endregion
    }
}
