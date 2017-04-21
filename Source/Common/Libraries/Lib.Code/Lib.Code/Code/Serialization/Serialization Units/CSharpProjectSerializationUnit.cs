using System;

using Public.Common.Lib.Code.Physical.CSharp;
using Public.Common.Lib.IO.Serialization;


namespace Public.Common.Lib.Code.Serialization
{
    public class CSharpProjectSerializationUnit : SerializationUnitBase
    {
        public Project Project { get; set; }


        public CSharpProjectSerializationUnit(string path, Project project)
            : base(path)
        {
            this.Project = project;
        }
    }
}
