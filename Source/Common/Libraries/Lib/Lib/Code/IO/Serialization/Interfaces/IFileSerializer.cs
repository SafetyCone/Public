

namespace Public.Common.Lib.IO.Serialization
{
    /// <summary>
    /// De/serializes an object to a file.
    /// </summary>
    /// <remarks>
    /// The details of what file format (BinaryFormatter, custom binary format, XML, text, etc.) are left unspecified.
    /// </remarks>
    public interface IFileSerializer<T>
    {
        /// <summary>
        /// De/serializes an object to a file.
        /// </summary>
        /// <param name="filePath">The rooted file path to use.</param>
        /// <remarks>
        /// The default usage syntax is via the indexer property, however extension methods allow a familiar Deserialize()/Serialize(), and implementors are encouraged to have instances call static Deserialize()/Serialize() methods.
        /// 
        /// Note to implementers:
        /// * Upon reading, you can assume the file exists, and if it doesn't throw an exception.
        /// * Upon writing, note that the option overwrite argument is true.
        /// 
        /// Note to implementors: You are encourage to have this indexer property call static methods Deserialize()/Serialize().
        /// </remarks>
        T this[string filePath, bool overwrite = true] { get; set; }
    }


    /// <summary>
    /// Allow the familiar instance based Deserialize()/Serialize() syntax.
    /// 
    /// Note: while implementors are encourage to define static Deserialize()/Serialize() methods, these extension methods of the same name will not conflict with the static methods as extension methods only exist on instances, where static methods can't, and vice-versa.
    /// </summary>
    public static class IFileSerializerExtensions
    {
        public static T Deserialize<T>(this IFileSerializer<T> fileSerializer, string filePath)
        {
            T output = fileSerializer[filePath];
            return output;
        }

        public static void Serialize<T>(this IFileSerializer<T> fileSerializer, string filePath, T obj)
        {
            fileSerializer[filePath] = obj;
        }
    }
}
