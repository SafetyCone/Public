using System.IO;

using Public.Common.Lib.IO.Serialization;


namespace Public.Examples.Code
{
    public class TExample
    {
    }

    /// <summary>
    /// Shows the standard pattern for (mis)using a property to cause de/serialization, while having both static De/Serialize() methods, and instance De/Serialize() methods via extension methods.
    /// </summary>
    public class IFileSerializerExample : IFileSerializer<TExample>
    {
        #region Static

        public static TExample Deserialize(string filePath)
        {
            using (FileStream fStream = new FileStream(filePath, FileMode.Open))
            {
                throw new System.NotImplementedException();
            }
        }

        public static void Serialize(string filePath, TExample t, bool overwrite = true)
        {
            FileMode fileMode = FileMode.Create;
            if (!overwrite)
            {
                fileMode = FileMode.CreateNew;
            }

            using (FileStream fStream = new FileStream(filePath, fileMode)) // Will allow the familiar error to percolate upwards if no overwriting.
            //using (StreamWriter writer = new StreamWriter(fStream)) // If you need text writing.
            {
                throw new System.NotImplementedException();
            }
        }

        #endregion


        public TExample this[string filePath, bool overwrite = true]
        {
            get
            {
                var output = IFileSerializerExample.Deserialize(filePath);
                return output;
            }
            set
            {
                IFileSerializerExample.Serialize(filePath, value, overwrite);
            }
        }
    }

    // The instance/static trick only works at the interface level.

    //public static class IFileSerializerExampleExtensions
    //{
    //    public static TExample Deserialize(this IFileSerializerExample serializer, string filePath)
    //    {
    //        var output = serializer[filePath];
    //        return output;
    //    }

    //    public static void Serialize(this IFileSerializerExample serializer, string filePath, TExample t, bool overwrite = true)
    //    {
    //        serializer[filePath, overwrite] = t;
    //    }
    //}
}