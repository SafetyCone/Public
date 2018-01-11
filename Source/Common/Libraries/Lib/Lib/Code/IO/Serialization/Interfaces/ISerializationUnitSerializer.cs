using System;


namespace Public.Common.Lib.IO.Serialization
{
    /// <summary>
    /// A serializer contains all the logic necessary to serialize a serialization unit, with which it is paired when placed into the serialization list.
    /// </summary>
    public interface ISerializationUnitSerializer
    {
        void Serialize(ISerializationUnit unit);
    }
}
