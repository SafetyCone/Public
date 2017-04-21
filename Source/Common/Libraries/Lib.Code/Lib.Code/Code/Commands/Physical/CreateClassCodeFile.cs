using System;

using Public.Common.Lib.Code.Logical;
using Public.Common.Lib.Code.Physical.CSharp;


namespace Public.Common.Lib.Code.Physical
{
    public class CreateClassCodeFile : ParentCommandBase
    {
        public Class Class { get; set; }
        public CodeFile CodeFile { get; set; }


        public CreateClassCodeFile(Class classObj)
        {
            this.Class = classObj;
        }

        public override void Run()
        {
            this.CodeFile = CodeFile.ProcessClass(this.Class);
        }
    }
}
