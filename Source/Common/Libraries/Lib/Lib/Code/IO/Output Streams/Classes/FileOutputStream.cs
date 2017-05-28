using System;
using System.IO;


namespace Public.Common.Lib.IO
{
    public class FileOutputStream : IOutputStream, IDisposable
    {
        #region IOutputStream Members

        public void Write(string value)
        {
            this.zStreamWriter.Write(value);
        }

        public void WriteLine(string value)
        {
            this.zStreamWriter.WriteLine(value);
        }

        #endregion

        #region IDisposable Members

        private bool zDisposed = false;


        public void Dispose()
        {
            this.CleanUp(true);

            GC.SuppressFinalize(this);
        }

        private void CleanUp(bool disposing)
        {
            if (!this.zDisposed)
            {
                if (disposing)
                {
                    // Call Dispose() on any contained managed disposable resources here.
                    this.zStreamWriter.Dispose();
                }

                // Clean-up unmanaged resources here.
            }

            this.zDisposed = true;
        }

        ~FileOutputStream()
        {
            this.CleanUp(false);
        }

        #endregion


        public string FilePath { get; private set; }
        private StreamWriter zStreamWriter;


        public FileOutputStream(string filePath)
        {
            this.FilePath = filePath;
            this.zStreamWriter = new StreamWriter(this.FilePath);
            this.zStreamWriter.AutoFlush = true;
        }
    }
}
