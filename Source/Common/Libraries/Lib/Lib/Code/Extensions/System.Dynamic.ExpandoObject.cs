using System.Collections.Generic;
using System.Dynamic;

using Public.Common.Lib.IO.Serialization;


namespace Public.Common.Lib.Extensions
{
    public static class ExpandoObjectExtensions
    {
        /// <summary>
        /// Can be used to add properties, methods, or events.
        /// </summary>
        /// <remarks>
        /// If the expando object already contains an element with the same name, the element is replaced.
        /// </remarks>
        public static void AddElement(this ExpandoObject expando, string name, object element)
        {
            var expandoAsDictionary = expando as IDictionary<string, object>;
            if(expandoAsDictionary.ContainsKey(name))
            {
                expandoAsDictionary[name] = element;
            }
            else
            {
                expandoAsDictionary.Add(name, element);
            }
        }

        public static void AddDictionary(this ExpandoObject expando, IDictionary<string, object> dictionary)
        {
            foreach(var pair in dictionary)
            {
                expando.AddElement(pair.Key, pair.Value);
            }
        }

        public static Dictionary<string, object> ToDictionary(this ExpandoObject expando)
        {
            var expandoAsDictionary = expando as IDictionary<string, object>;
            var output = new Dictionary<string, object>();
            foreach(string key in expandoAsDictionary.Keys)
            {
                object value = expandoAsDictionary[key];
                if(value is ExpandoObject subExpando)
                {
                    value = subExpando.ToDictionary();
                }

                output.Add(key, value);
            }

            return output;
        }

        public static void FromDictionary(this ExpandoObject expando, IDictionary<string, object> dictionary)
        {
            expando.AddDictionary(dictionary);
        }

        public static void SerializeToFile(this ExpandoObject expando, string filePath)
        {
            var dictionary = expando.ToDictionary();

            BinaryFileSerializer.Serialize(filePath, dictionary);
        }

        public static void DeserializeFromFile(this ExpandoObject expando, string filePath)
        {
            var dictionary = BinaryFileSerializer.Deserialize<Dictionary<string, object>>(filePath);

            expando.AddDictionary(dictionary);
        }

        /// <summary>
        /// Performs a shallow copy of all fields except those that are themselves ExpandoObjects, which are recursively copied in the same way.
        /// </summary>
        public static ExpandoObject Copy(this ExpandoObject expando)
        {
            ExpandoObject output = new ExpandoObject();

            var expandoDict = expando as IDictionary<string, object>;
            foreach(string key in expandoDict.Keys)
            {
                object value = expandoDict[key];
                if(value is ExpandoObject subExpando)
                {
                    ExpandoObject subExpandoCopy = subExpando.Copy();
                    output.AddElement(key, subExpandoCopy);
                }
                else
                {
                    output.AddElement(key, value);
                }
            }

            return output;
        }

        public static ExpandoObject ShallowCopy(this ExpandoObject expando)
        {
            ExpandoObject output = new ExpandoObject();

            var expandoDict = expando as IDictionary<string, object>;
            output.AddDictionary(expandoDict);

            return output;
        }
    }
}
