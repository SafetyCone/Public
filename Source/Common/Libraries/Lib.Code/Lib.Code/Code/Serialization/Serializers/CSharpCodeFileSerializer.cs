using System;
using System.IO;

using Public.Common.Lib.Code.Physical;
using Public.Common.Lib.IO.Serialization;


namespace Public.Common.Lib.Code.Serialization
{
    public class CSharpCodeFileSerializer : SerializerBase<CSharpCodeFileSerializationUnit>
    {
        protected override void Serialize(CSharpCodeFileSerializationUnit unit)
        {
            string directoryPath = Path.GetDirectoryName(unit.Path);
            Directory.CreateDirectory(directoryPath);

            SerializeCodeFileToPath command = new SerializeCodeFileToPath(unit.Path, unit.CodeFile);
            command.Run();
        }
    }
}
