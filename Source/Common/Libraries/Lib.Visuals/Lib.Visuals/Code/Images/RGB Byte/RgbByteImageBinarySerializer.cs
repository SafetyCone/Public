using System;
using System.IO;

using Public.Common.Lib.IO.Serialization;


namespace Public.Common.Lib.Visuals
{
    public class RgbByteImageBinarySerializer : IFileSerializer<RgbByteImage>
    {
        #region Static

        public static RgbByteImage Deserialize(string filePath)
        {
            int rows;
            int cols;
            byte[] intBuffer = new byte[sizeof(Int32)];
            byte[] valuesByteBuffer;
            using (FileStream fStream = new FileStream(filePath, FileMode.Open))
            {
                fStream.Read(intBuffer, 0, sizeof(Int32));
                rows = BitConverter.ToInt32(intBuffer, 0);

                fStream.Read(intBuffer, 0, sizeof(Int32));
                cols = BitConverter.ToInt32(intBuffer, 0);

                int numBytes = rows * cols * RgbColor.NumberOfRgbColorChannels;
                valuesByteBuffer = new byte[numBytes];
                fStream.Read(valuesByteBuffer, 0, numBytes);
            }

            // Create the output image.
            RgbByteImage output = new RgbByteImage(rows, cols, valuesByteBuffer);
            byte[] values = output.Data;

            return output;
        }

        public static void Serialize(string filePath, RgbByteImage image, bool overwrite = true)
        {
            FileMode fileMode = FileMode.Create;
            if (!overwrite)
            {
                fileMode = FileMode.CreateNew;
            }

            using (FileStream fStream = new FileStream(filePath, fileMode))
            {
                fStream.Write(BitConverter.GetBytes(image.Rows), 0, sizeof(Int32));
                fStream.Write(BitConverter.GetBytes(image.Columns), 0, sizeof(Int32));
                fStream.Write(image.Data, 0, image.Data.Length);
            }
        }

        #endregion


        public RgbByteImage this[string filePath, bool overwrite = true]
        {
            get
            {
                RgbByteImage output = RgbByteImageBinarySerializer.Deserialize(filePath);
                return output;
            }
            set
            {
                RgbByteImageBinarySerializer.Serialize(filePath, value, overwrite);
            }
        }
    }
}
