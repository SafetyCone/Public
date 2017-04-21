using System;
using System.IO;
using System.Text;


namespace Public.Common.Lib.IO
{
    public class FileWriter : IDisposable
    {
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
                    this.Writer.Dispose();
                }

                // Clean-up unmanaged resources here.
            }

            this.zDisposed = true;
        }

        ~FileWriter()
        {
            this.CleanUp(false);
        }

        #endregion


        public string Path { get; set; }
        public Encoding Encoding { get; protected set; }
        public StreamWriter Writer { get; protected set; }


        public FileWriter(string path)
            : this(path, Encoding.UTF8)
        {
        }

        public FileWriter(string path, Encoding encoding)
        {
            this.Path = path;
            this.Encoding = encoding;
            this.Writer = new StreamWriter(this.Path, false, this.Encoding);
        }

        public void WriteBlankLine()
        {
            this.Writer.WriteLine();
        }

        public void WriteLine(string line)
        {
            this.Writer.WriteLine(line);
        }

        public void Write(string str)
        {
            this.Writer.Write(str);
        }

        public void Write(char chr)
        {
            this.Writer.Write(chr);
        }
    }
}
