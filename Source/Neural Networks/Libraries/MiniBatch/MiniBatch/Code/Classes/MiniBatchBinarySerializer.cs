using System;
using System.IO;


namespace Public.NeuralNetworks.MiniBatch
{
    /// <summary>
    /// For use in .NET Core where there is no default binary serializer.
    /// </summary>
    public class MiniBatchBinarySerializer
    {
        #region Static

        public static void SerializeStatic(string filePath, MiniBatch miniBatch)
        {
            Int32 numberOfMiniBatches = miniBatch.Values.Length;
            Int32 sizeOfMiniBatches = miniBatch.Values[0].Length;

            byte[] bytes;

            using (FileStream fStream = new FileStream(filePath, FileMode.Create))
            {
                bytes = BitConverter.GetBytes(numberOfMiniBatches);
                fStream.Write(bytes, 0, bytes.Length);

                bytes = BitConverter.GetBytes(sizeOfMiniBatches);
                fStream.Write(bytes, 0, bytes.Length);

                foreach (int value in miniBatch.EnumerateValues())
                {
                    bytes = BitConverter.GetBytes(value);
                    fStream.Write(bytes, 0, bytes.Length);
                }
            }
        }

        public static MiniBatch DeserializeStatic(string filePath)
        {
            MiniBatch output;

            byte[] bytes;
            int int32ByteCount = 4;

            using (FileStream fStream = new FileStream(filePath, FileMode.Open))
            {
                bytes = new byte[int32ByteCount];

                fStream.Read(bytes, 0, int32ByteCount);
                int numberOfMiniBatches = BitConverter.ToInt32(bytes, 0);

                fStream.Read(bytes, 0, int32ByteCount);
                int sizeOfMiniBatches = BitConverter.ToInt32(bytes, 0);

                output = new MiniBatch(numberOfMiniBatches, sizeOfMiniBatches);
                foreach (Tuple<int, int> position in output.EnumeratePositions())
                {
                    fStream.Read(bytes, 0, int32ByteCount);
                    int value = BitConverter.ToInt32(bytes, 0);

                    output.Values[position.Item1][position.Item2] = value;
                }
            }

            return output;
        }

        #endregion


        public void Serialize(string filePath, MiniBatch miniBatch)
        {
            MiniBatchBinarySerializer.SerializeStatic(filePath, miniBatch);
        }

        public MiniBatch Deserialize(string filePath)
        {
            MiniBatch output = MiniBatchBinarySerializer.DeserializeStatic(filePath);
            return output;
        }
    }
}
