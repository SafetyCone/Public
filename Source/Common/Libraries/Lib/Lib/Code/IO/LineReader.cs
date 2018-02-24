using System;
using System.IO;


namespace Public.Common.Lib.IO
{
    public class LineReader : IDisposable
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
                    if(!this.LeaveOpen)
                    {
                        this.StreamReader.Dispose();
                    }
                }

                // Clean-up unmanaged resources here.
            }

            this.zDisposed = true;
        }

        ~LineReader()
        {
            this.Dispose(false);
        }

        #endregion


        public StreamReader StreamReader { get; }
        public int LinesRead { get; private set; } = 0;
        public bool LeaveOpen { get; }


        public LineReader(StreamReader reader, int linesRead, bool leaveOpen = false)
        {
            this.StreamReader = reader;
            this.LinesRead = linesRead;
            this.LeaveOpen = leaveOpen;
        }

        public LineReader(string filePath)
            : this(new StreamReader(filePath), 0)
        {
        }

        public string ReadLine()
        {
            string output = this.StreamReader.ReadLine();
            this.LinesRead++;

            return output;
        }
    }
}
