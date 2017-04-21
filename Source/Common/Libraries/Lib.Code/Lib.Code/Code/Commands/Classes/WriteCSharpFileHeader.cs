using System;
using System.Collections.Generic;

using Public.Common.Lib.Code.Physical.CSharp;


namespace Public.Common.Lib.Code.Physical
{
    public class WriteCSharpFileHeader : CommandBase
    {
        public WriteCSharpFile WriteCSharpFile { get; protected set; }
        public List<UsingDeclaration> SystemUsings { get; protected set; }
        public List<UsingDeclaration> OwnUsings { get; protected set; }
        public List<UsingDeclaration> ThirdPartyUsings { get; protected set; }


        public WriteCSharpFileHeader(WriteCSharpFile writeCSharpFile)
        {
            this.WriteCSharpFile = writeCSharpFile;

            this.SystemUsings = new List<UsingDeclaration>();
            this.OwnUsings = new List<UsingDeclaration>();
            this.ThirdPartyUsings = new List<UsingDeclaration>();
        }

        public override void Run()
        {
            this.WriteUsings(this.SystemUsings);
            this.WriteUsings(this.ThirdPartyUsings);

            // Separate the well-test "solid" usings from my usings with a blank line.
            if (null != this.OwnUsings)
            {
                this.WriteCSharpFile.WriteBlankLine();
                this.WriteUsings(this.OwnUsings);
            }
        }

        public void WriteUsings(List<UsingDeclaration> usings)
        {
            if (null != usings)
            {
                usings.Sort();

                foreach (UsingDeclaration usingDeclaration in usings)
                {
                    string line = WriteCSharpFile.FormatUsing(usingDeclaration);
                    this.WriteCSharpFile.WriteCodeLine(line);
                }
            }
        }
    }
}
