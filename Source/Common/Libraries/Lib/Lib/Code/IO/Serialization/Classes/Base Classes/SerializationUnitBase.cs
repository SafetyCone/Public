using System;


namespace Public.Common.Lib.IO.Serialization
{
    public abstract class SerializationUnitBase : ISerializationUnit
    {
        public string Path { get; set; }


        public SerializationUnitBase()
        {
        }

        public SerializationUnitBase(string path)
        {
            this.Path = path;
        }
    }
}
