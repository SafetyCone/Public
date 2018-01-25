using Public.Common.Lib.Logging;


namespace Public.Common.Lib.IO.Serialization
{
    /// <summary>
    /// De/serializes an object to a file, providing instrumentation of its operations.
    /// </summary>
    /// <remarks>
    /// The details of what file format (BinaryFormatter, custom binary format, XML, text, etc.) are left unspecified.
    /// </remarks>
    public interface IInstrumentedFileSerializer<T>
    {
        /// <summary>
        /// De/serializes an object to a file.
        /// </summary>
        /// <param name="filePath">The rooted file path to use.</param>
        /// <param name="loggingTiming">The logging and timing objects to use.</param>
        /// <remarks>
        /// The default usage syntax is via the indexer property, however extension methods allow a familiar Deserialize()/Serialize(), and implementors are encouraged to have instances call static Deserialize()/Serialize() methods.
        /// 
        /// Note to implementers:
        /// * Upon reading, you can assume the file exists, and if it doesn't throw an exception.
        /// * Upon writing, note that the option overwrite argument is true.
        /// 
        /// Note to implementors: You are encourage to have this indexer property call static methods Deserialize()/Serialize().
        /// </remarks>
        T this[string filePath, LoggingTiming loggingTiming = default(LoggingTiming), bool overwrite = true] { get; set; }
    }


    /// <summary>
    /// Allow the familiar instance based Deserialize()/Serialize() syntax.
    /// 
    /// Note: while implementors are encourage to define static Deserialize()/Serialize() methods, these extension methods of the same name will not conflict with the static methods as extension methods only exist on instances, where static methods can't, and vice-versa.
    /// </summary>
    public static class IInstrumentedFileSerializerExtensions
    {
        public static T Deserialize<T>(this IInstrumentedFileSerializer<T> fileSerializer, string filePath, LoggingTiming loggingTiming = default(LoggingTiming))
        {
            T output = fileSerializer[filePath, loggingTiming];
            return output;
        }

        public static void Serialize<T>(this IInstrumentedFileSerializer<T> fileSerializer, string filePath, T obj, LoggingTiming loggingTiming = default(LoggingTiming), bool overwrite = true)
        {
            fileSerializer[filePath, loggingTiming, overwrite] = obj;
        }
    }
}
