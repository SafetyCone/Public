using System;
using System.IO;


namespace Public.Common.Lib.Visuals.IO.Serialization
{
    /// <summary>
    /// Serializes RGB float images to binary files.
    /// </summary>
    /// <remarks>
    /// The format used is direct:
    /// * (int, 4-bytes) number of rows.
    /// * (int, 4-bytes) number of columns.
    /// * N = rows * columns * 3 color channel (float, 4-bytes) values.
    /// </remarks>
    public class RgbFloatImageBinarySerializer : IRgbFloatImageSerializer
    {
        #region Static

        public static RgbFloatImage Deserialize(string imageFilePath)
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

                int numBytes = rows * cols * RgbFloatImage.NumberOfRgbColorChannels * sizeof(float);
                valuesByteBuffer = new byte[numBytes];
                fStream.Read(valuesByteBuffer, 0, numBytes);
            }

            // Create the output image.
            RgbFloatImage output = new RgbFloatImage(rows, cols);
            float[] values = output.Data;

            // Fill the output image.
            int numValues = values.Length;
            int iByte = 0;
            for (int iValue = 0; iValue < numValues; iValue++)
            {
                float value = BitConverter.ToSingle(valuesByteBuffer, iByte);
                values[iValue] = value;

                iByte += sizeof(float);
            }

            return output;
        }

        public static void Serialize(string imageFilePath, RgbFloatImage image, bool overwrite = true)
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


        public ImageFormat[] SupportedImageFormats => new ImageFormat[] { ImageFormat.Dat };
        public RgbFloatImage this[string imageFilePath, bool overwrite = true]
        {
            get
            {
                RgbFloatImage output = RgbFloatImageBinarySerializer.Deserialize(imageFilePath);
                return output;
            }
            set
            {
                RgbFloatImageBinarySerializer.Serialize(imageFilePath, value);
            }
        }
    }
}
