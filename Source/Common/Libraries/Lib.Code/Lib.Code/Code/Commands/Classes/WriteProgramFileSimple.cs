using System;
using System.IO;
using System.Text;


namespace Public.Common.Lib.Code
{
    public class WriteProgramFileSimple : CommandBase
    {
        public string FilePath { get; set; }
        public StreamWriter Writer { get; set; }


        public WriteProgramFileSimple(string filePath)
        {
            this.FilePath = filePath;
        }

        public override void Run()
        {
            using (this.Writer = new StreamWriter(this.FilePath, false, new UTF8Encoding(true))) // Use byte-order-mark since all files produced by VS seem to have it.
            {
                Writer.WriteLine(@"using System;");
                Writer.WriteLine(@"using System.Collections.Generic;");
                Writer.WriteLine(@"using System.Linq;");
                Writer.WriteLine(@"using System.Text;");
                Writer.WriteLine();
                Writer.WriteLine(@"namespace CsConsoleApplication1");
                Writer.WriteLine(@"{");

                Writer.WriteLine(@"    class Program");
                Writer.WriteLine(@"    {");
                Writer.WriteLine(@"        static void Main(string[] args)");
                Writer.WriteLine(@"        {");
                Writer.WriteLine(@"        }");
                Writer.WriteLine(@"    }");
                Writer.WriteLine(@"}");
            }
        }
    }
}
