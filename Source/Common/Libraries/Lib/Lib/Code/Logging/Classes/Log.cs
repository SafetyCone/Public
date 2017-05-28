using System;
using System.IO;

using Public.Common.Lib.Extensions;
using Public.Common.Lib.IO;
using Prod = Public.Common.Lib.Production.Production;


namespace Public.Common.Lib.Logging
{
    public class Log : ILog
    {
        #region Static

        /// <summary>
        /// Gets a log that uses the debug window as its output stream.
        /// </summary>
        public static Log DebugLog()
        {
            Log output = new Log(new DebugOutputStream());
            return output;
        }

        /// <summary>
        /// Gets a log that uses the console as its output stream.
        /// </summary>
        public static Log ConsoleLog()
        {
            Log output = new Log(new ConsoleOutputStream());
            return output;
        }

        /// <summary>
        /// Gets a log that uses a string list as its output stream.
        /// </summary>
        public static Log StringListLog()
        {
            Log output = new Log(new StringListOutputStream());
            return output;
        }

        public static string GetDefaultLogFilePath(string programName)
        {
            string userLogsDirectoryPath = Prod.UserLogsDirectoryPath;
            
            string dateYYYYMMDD = DateTime.Today.ToYYYYMMDDStr();
            string logFileName = String.Format(@"{0} - {1}_{2:HHmmss}.log", programName, dateYYYYMMDD, DateTime.Now);

            string output = Path.Combine(userLogsDirectoryPath, programName, dateYYYYMMDD, logFileName);
            return output;
        }

        public static string GetAndEnsureDefaultLogFilePath(string programName)
        {
            string defaultLogFilePath = Log.GetDefaultLogFilePath(programName);

            string directoryPath = Path.GetDirectoryName(defaultLogFilePath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            return defaultLogFilePath;
        }

        public static Log GetDefaultLog(string programName)
        {
            string defaultLogFilePath = Log.GetAndEnsureDefaultLogFilePath(programName);

            Log output = new Log(defaultLogFilePath);
            return output;
        }

        #endregion

        #region ILog Members

        public void Write(string value)
        {
            this.OutputStream.Write(value);
        }

        public void WriteLine(string value)
        {
            this.OutputStream.WriteLine(value);
        }

        #endregion


        public IOutputStream OutputStream { get; set; }


        public Log(IOutputStream outputStream)
        {
            this.OutputStream = outputStream;
        }

        public Log(string filePath)
            : this(new FileOutputStream(filePath))
        {
        }
    }
}
