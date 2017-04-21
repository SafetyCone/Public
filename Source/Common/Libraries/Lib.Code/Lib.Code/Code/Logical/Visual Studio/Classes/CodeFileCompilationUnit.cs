using System;

using Public.Common.Lib.Code.Physical;


namespace Public.Common.Lib.Code.Logical
{
    public class CodeFileCompilationUnit : CompilationUnit
    {
        public CodeFileBase CodeFile { get; set; }


        public CodeFileCompilationUnit()
        {
        }

        public CodeFileCompilationUnit(CodeFileBase codeFile)
        {
            this.CodeFile = codeFile;
        }
    }
}
