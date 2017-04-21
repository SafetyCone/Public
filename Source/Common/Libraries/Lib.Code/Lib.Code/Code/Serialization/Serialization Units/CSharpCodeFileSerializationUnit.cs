using System;

using Public.Common.Lib.Code.Physical.CSharp;
using Public.Common.Lib.IO.Serialization;


namespace Public.Common.Lib.Code.Serialization
{
    /// <summary>
    /// A C# code file serialize unit.
    /// </summary>
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
