using System;


namespace Public.Common.Granby.Lib
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
