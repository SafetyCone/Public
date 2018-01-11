using System;
using System.Collections.Generic;


namespace Public.Common.Lib.IO.Serialization
{
    /// <summary>
    /// A list of [serialization unit, serializer] pairs that can accumulate objects in need of serialization, then serialize all objects in one step.
    /// </summary>
    /// <remarks>
    /// Generally, a single instance of a serializer can be used to serialize multiple serialization units. A dictionary of serializers is provided, keyed by a friendly moniker that can be used to get a serializer for a particular serialization unit.
    /// 
    /// Extensions methods can be created to streamline the addition of serialization units and their serializers. For an example, see the serialization list extesions.
    /// </remarks>
    public class SerializationList
    {
        public Dictionary<string, ISerializationUnitSerializer> SerializersByMoniker { get; protected set; }
        public List<Tuple<ISerializationUnit, ISerializationUnitSerializer>> Units { get; protected set; }


        public SerializationList()
        {
            this.SerializersByMoniker = new Dictionary<string, ISerializationUnitSerializer>();
            this.Units = new List<Tuple<ISerializationUnit, ISerializationUnitSerializer>>();
        }

        public void AddUnitByMoniker(ISerializationUnit unit, string serializerMoniker)
        {
            ISerializationUnitSerializer serializer = this.SerializersByMoniker[serializerMoniker];
            this.Units.Add(new Tuple<ISerializationUnit, ISerializationUnitSerializer>(unit, serializer));
        }

        public void Serialize()
        {
            foreach(Tuple<ISerializationUnit, ISerializationUnitSerializer> pair in this.Units)
            {
                pair.Item2.Serialize(pair.Item1);
            }
        }
    }
}
