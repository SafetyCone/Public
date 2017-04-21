using System;
using System.IO;
using System.Text;


namespace Public.Common.Lib.Code.Physical
{
    public class WriteFile : ParentCommandBase
    {
        public string Path { get; set; }
        public Encoding Encoding { get; set; }
        public StreamWriter Writer { get; protected set; }


        public WriteFile()
        {
            this.Encoding = Encoding.UTF8;
        }

        public WriteFile(string path)
            : this()
        {
            this.Path = path;
        }

        public WriteFile(string path, Encoding encoding)
        {
            this.Path = path;
            this.Encoding = encoding;
        }

        public override void Run()
        {
            this.Writer = new StreamWriter(this.Path, false, this.Encoding);

            base.Run();

            this.Writer.Dispose();
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
