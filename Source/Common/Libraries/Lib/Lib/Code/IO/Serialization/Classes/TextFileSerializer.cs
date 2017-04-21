using System;
using System.IO;


namespace Public.Common.Lib.IO.Serialization
{
    public class TextFileSerializer : SerializerBase<TextFileSerializationUnit>
    {
        protected override void Serialize(TextFileSerializationUnit unit)
        {
            File.WriteAllLines(unit.Path, unit.TextFile.Lines);
        }
    }
}
