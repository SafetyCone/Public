using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


namespace PythonFromNet.Lib
{
    public class BinarySerializer<T>
    {
        #region Static

        public static T DeserializeStatic(string binaryFileRootedPath)
        {
            BinaryFormatter formatter = new BinaryFormatter();

            T output;
            using (Stream fileStream = new FileStream(binaryFileRootedPath, FileMode.Open))
            {
                output = (T)formatter.Deserialize(fileStream);
            }

            return output;
        }

        public static void SerializeStatic(T value, string binaryFileRootedPath)
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
