using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


namespace Public.Common.Lib.IO.Serialization
{
    public class BinaryFileSerializer
    {
        #region Static

        public static T Deserialize<T>(string binaryFileRootedPath)
        {
            BinaryFormatter formatter = new BinaryFormatter();

            T output;
            using (Stream fileStream = new FileStream(binaryFileRootedPath, FileMode.Open))
            {
                output = (T)formatter.Deserialize(fileStream);
            }

            return output;
        }

        public static void Serialize<T>(string binaryFileRootedPath, T value, bool overwrite = true)
        {
            FileMode fileMode = FileMode.Create;
            if(!overwrite)
            {
                fileMode = FileMode.CreateNew;
            }

            BinaryFormatter formatter = new BinaryFormatter();
            using (Stream fileStream = new FileStream(binaryFileRootedPath, fileMode))
            {
                formatter.Serialize(fileStream, value);
            }
        }

        #endregion
    }
}
