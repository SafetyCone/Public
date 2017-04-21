using System;


namespace Public.Common.Lib.IO.Serialization
{
    /// <summary>
    /// A serialization unit provides only its path, and is paired with a serializer when it is placed into the serialization list.
    /// </summary>
    public interface ISerializationUnit
    {
        string Path { get; set; }
    }
}
