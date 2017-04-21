using System;
using System.IO;


namespace Public.Common.Lib.IO.Serialization
{
    public class CreateDirectorySerializer : SerializerBase<CreateDirectorySerializationUnit>
    {
        protected override void Serialize(CreateDirectorySerializationUnit unit)
        {
            Directory.CreateDirectory(unit.Path);
        }
    }
}
