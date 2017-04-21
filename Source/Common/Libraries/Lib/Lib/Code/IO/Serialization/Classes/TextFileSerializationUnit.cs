using System;


namespace Public.Common.Lib.IO.Serialization
{
    public class TextFileSerializationUnit : SerializationUnitBase
    {
        public TextFile TextFile { get; set; }


        public TextFileSerializationUnit(string path, TextFile textFile)
            : base(path)
        {
            this.TextFile = textFile;
        }
    }
}
