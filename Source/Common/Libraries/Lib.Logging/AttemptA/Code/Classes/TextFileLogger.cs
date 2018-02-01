using System;
using System.IO;


namespace Public.Common.Lib.Logging.AttemptA
{
    public class TextFileLogger : ILogger, IDisposable
    {
        #region IDisposable Members

        private bool zDisposed = false;


        public void Dispose()
        {
            this.Dispose(true);

            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!this.zDisposed)
            {
                if (disposing)
                {
                    // Call Dispose() on any contained managed disposable resources here.
                    this.Writer.Dispose();
                }

                // Clean-up unmanaged resources here.
            }

            this.zDisposed = true;
        }

        ~TextFileLogger()
        {
            this.Dispose(false);
        }

        #endregion

        private StreamWriter Writer { get; }


        public TextFileLogger(string filePath)
        {
            this.Writer = new StreamWriter(filePath)
            {
                AutoFlush = true
            };
        }

        public bool IsEnabled(Level level)
        {
            return true;
        }

        public void Log(Level level, object message)
        {
            string line = DefaultLogEntrySerializer.Serialize(level, message);
            this.Writer.WriteLine(line);
        }
    }
}
