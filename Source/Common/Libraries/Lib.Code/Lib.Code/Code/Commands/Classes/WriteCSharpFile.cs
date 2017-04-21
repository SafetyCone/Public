using System;
using System.Collections.Generic;

using Public.Common.Lib.Code.Physical.CSharp;


namespace Public.Common.Lib.Code.Physical
{
    public abstract class WriteCSharpFile : WriteCodeFile
    {
        #region Static

        public static readonly List<UsingDeclaration> DefaultUsingNamespaces;


        static WriteCSharpFile()
        {
            WriteCSharpFile.DefaultUsingNamespaces = new List<UsingDeclaration>();
            WriteCSharpFile.DefaultUsingNamespaces.Add(new UsingDeclaration(@"System"));
        }

        public static string FormatUsing(UsingDeclaration usingDeclaration)
        {
            string declaration = usingDeclaration.ToString();

            string output = String.Format(@"using {0}", declaration);
            return output;
        }

        #endregion


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
