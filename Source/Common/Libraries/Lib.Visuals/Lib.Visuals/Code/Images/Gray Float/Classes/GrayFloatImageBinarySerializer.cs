using System;
using System.IO;

using Public.Common.Lib.IO.Serialization;


namespace Public.Common.Lib.Visuals
{
    public class GrayFloatImageBinarySerializer : IFileSerializer<GrayFloatImage>
    {
        #region Static

        public static GrayFloatImage Deserialize(string imageFilePath)
        {
            // Determine the size of the image and get the image byte data.
            int rows;
            int cols;
            byte[] intBuffer = new byte[sizeof(Int32)];
            byte[] valuesByteBuffer;
            using (FileStream fStream = new FileStream(imageFilePath, FileMode.Open))
            {
                fStream.Read(intBuffer, 0, sizeof(Int32));
                rows = BitConverter.ToInt32(intBuffer, 0);

                fStream.Read(intBuffer, 0, sizeof(Int32));
                cols = BitConverter.ToInt32(intBuffer, 0);

                int numBytes = rows * cols * GrayFloatImage.NumberOfGrayColorChannels * sizeof(float);
                valuesByteBuffer = new byte[numBytes];
                fStream.Read(valuesByteBuffer, 0, numBytes);
            }

            // Create the output image.
            GrayFloatImage output = new GrayFloatImage(rows, cols);
            float[] values = output.Data;

            // Fill the output image.
            int numValues = rows * cols;
            int iByte = 0;
            for (int iValue = 0; iValue < numValues; iValue++)
            {
                float value = BitConverter.ToSingle(valuesByteBuffer, iByte);
                values[iValue] = value;

                iByte += sizeof(float);
            }

            return output;
        }

        public static void Serialize(string imageFilePath, GrayFloatImage image, bool overwrite = true)
        {
            FileMode fileMode = FileMode.Create;
            if(!overwrite)
            {
                fileMode = FileMode.CreateNew;
            }

            using (FileStream fStream = new FileStream(imageFilePath, fileMode))
            {
                fStream.Write(BitConverter.GetBytes(image.Rows), 0, sizeof(Int32));
                fStream.Write(BitConverter.GetBytes(image.Columns), 0, sizeof(Int32));

                float[] values = image.Data;
                int numValues = values.Length;
                for (int iValue = 0; iValue < numValues; iValue++)
                {
                    float value = values[iValue];
                    fStream.Write(BitConverter.GetBytes(value), 0, sizeof(float));
                }
            }
        }

        #endregion

        public GrayFloatImage this[string filePath, bool overwrite = true]
        {
            get
            {
                GrayFloatImage output = GrayFloatImageBinarySerializer.Deserialize(filePath);
                return output;
            }
            set
            {
                GrayFloatImageBinarySerializer.Serialize(filePath, value, overwrite);
            }
        }
    }
}
