using System;

using Public.Common.Lib.Code.Physical.CSharp;
using Public.Common.Lib.IO.Serialization;


namespace Public.Common.Lib.Code.Serialization
{
    public class CSharpCodeFileSerializationUnit : SerializationUnitBase
    {
        public CodeFile CodeFile { get; set; }


        public CSharpCodeFileSerializationUnit(string path, CodeFile codeFile)
            : base(path)
        {
            this.CodeFile = codeFile;
        }
    }
}
