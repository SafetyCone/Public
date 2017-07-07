using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


namespace Public.Common.Lib.IO.Serialization
{
    public class BinarySerializer
    {
        #region Static

        public static T DeserializeStatic<T>(string binaryFileRootedPath)
        {
            BinaryFormatter formatter = new BinaryFormatter();

            T output;
            using (Stream fileStream = new FileStream(binaryFileRootedPath, FileMode.Open))
            {
                output = (T)formatter.Deserialize(fileStream);
            }

            return output;
        }

        public static void SerializeStatic<T>(T value, string binaryFileRootedPath)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (Stream fileStream = new FileStream(binaryFileRootedPath, FileMode.Create))
            {
                formatter.Serialize(fileStream, value);
            }
        }

        #endregion
    }
}
