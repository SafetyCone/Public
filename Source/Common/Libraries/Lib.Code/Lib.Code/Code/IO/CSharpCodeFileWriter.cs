using System;


namespace Public.Common.Lib.Code.Physical.CSharp
{
    /// <summary>
    /// Provides methods useful in writing C# code files.
    /// </summary>
    public class CSharpCodeFileWriter : CodeFileWriter
    {
        public Types Types { get; protected set; }
        public Methods Methods { get; protected set; }


        public CSharpCodeFileWriter(string path)
            : base(path)
        {
            this.Types = new Types();
            this.Methods = new Methods();
        }

        public void WriteNamespace(string namespaceStr)
        {
            string line = String.Format(@"namespace {0}", namespaceStr);
            this.WriteLine(line);
        }

        public void OpenScope()
        {
            this.WriteIndentedLine('{');
            this.IncreaseIndent();
        }

        public void CloseScope()
        {
            this.DecreaseIndent();
            this.WriteIndentedLine('}');
        }

        /// <summary>
        /// Adds a semi-colon to the end of the code line.
        /// </summary>
        /// <param name="line"></param>
        public void WriteCodeLine(string line)
        {
            this.Writer.Write(line);
            this.Writer.WriteLine(';');
        }
    }
}
