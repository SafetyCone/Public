using System;
using System.IO;
using System.Text;

using Public.Common.Lib.Extensions;
using Public.Common.Lib.IO.Serialization;


namespace Eshunna.Lib.SIFT
{
    public class SiftBinarySerializer : IFileSerializer<SiftFile>
    {
        private const int HeaderIntCount = 5;

        #region Static

        private static readonly int HeaderByteCount = SiftBinarySerializer.HeaderIntCount * sizeof(Int32);


        public static SiftFile Deserialize(string filePath)
        {
            using (FileStream fStream = new FileStream(filePath, FileMode.Open))
            {
                SiftHeader header = SiftBinarySerializer.DeserializeHeader(fStream);
                SiftFeature[] features = SiftBinarySerializer.DeserializeFeatures(fStream, header);
                SiftDescriptor[] descriptors = SiftBinarySerializer.DeserializeDescriptors(fStream, header);
                byte endByte = SiftBinarySerializer.DerserializeEofMarker(fStream);

                SiftFile output = new SiftFile(header, features, descriptors)
                {
                    EndByte = endByte
                };
                return output;
            }
        }

        private static byte DerserializeEofMarker(FileStream fileStream)
        {
            int nBytes = 4;
            byte[] bytes = new byte[nBytes];
            fileStream.Read(bytes, 0, nBytes);

            string eofStr = Encoding.Default.GetString(bytes);
            string eofMarker = eofStr.Substring(1);
            if(SiftFile.EndOfFileMarker != eofMarker)
            {
                string message = $@"Invalid end-of-file marker found. Expected: {SiftFile.EndOfFileMarker}, found: {eofMarker}";
                throw new InvalidDataException(message);
            }

            byte output = bytes[0];
            return output;
        }

        private static void SerializeEofMarker(FileStream fileStream, byte endByte)
        {
            byte[] eofBytes = Encoding.Default.GetBytes(SiftFile.EndOfFileMarker);

            int nBytes = sizeof(Int32);
            byte[] bytes = new byte[nBytes];
            Array.Copy(eofBytes, 0, bytes, 1, eofBytes.Length); // Leave the first byte blank.

            bytes[0] = endByte;

            fileStream.Write(bytes, 0, nBytes);
        }

        private static SiftDescriptor[] DeserializeDescriptors(FileStream fStream, SiftHeader header)
        {
            // Read bytes.
            int nFeatures = header.NumberOfFeaturePoints;
            int nDescriptorComponents = header.NumberOfDescriptorComponents;
            int nBytes = nFeatures * nDescriptorComponents;
            byte[] bytes = new byte[nBytes];

            fStream.Read(bytes, 0, nBytes);

            // Parse bytes.
            SiftDescriptor[] output = new SiftDescriptor[nFeatures];
            int nDescriptorBytes = nDescriptorComponents * sizeof(byte);
            for (int iFeature = 0; iFeature < nFeatures; iFeature++)
            {
                byte[] components = new byte[nDescriptorComponents];
                Array.Copy(bytes, iFeature * nDescriptorBytes, components, 0, nDescriptorBytes);

                SiftDescriptor descriptor = new SiftDescriptor(components);
                output[iFeature] = descriptor;
            }

            return output;
        }

        private static void SerializeDescriptors(FileStream fStream, SiftDescriptor[] descriptors)
        {
            foreach (var descriptor in descriptors)
            {
                fStream.Write(descriptor.Components, 0, descriptor.Components.Length);
            }
        }

        private static SiftFeature[] DeserializeFeatures(FileStream fStream, SiftHeader header)
        {
            // Read bytes.
            int nFeatures = header.NumberOfFeaturePoints;
            int valueSize = sizeof(float);
            int nBytes = nFeatures * header.NumberOfFeatureProperties * valueSize;
            byte[] bytes = new byte[nBytes];

            fStream.Read(bytes, 0, nBytes);

            // Parse bytes.
            int xOffset = 0 * valueSize;
            int yOffset = 1 * valueSize;
            int colorOffset = 2 * valueSize;
            int scaleOffset = 3 * valueSize;
            int orientationOffset = 4 * valueSize;

            SiftFeature[] output = new SiftFeature[nFeatures];
            int iByte = 0;
            for (int iFeature = 0; iFeature < nFeatures; iFeature++, iByte += 5 * valueSize)
            {
                float x = BitConverter.ToSingle(bytes, iByte + xOffset);
                float y = BitConverter.ToSingle(bytes, iByte + yOffset);
                byte red = bytes[iByte + colorOffset + 0];
                byte green = bytes[iByte + colorOffset + 1];
                byte blue = bytes[iByte + colorOffset + 2];
                byte alpha = bytes[iByte + colorOffset + 3];
                ColorAlpha color = new ColorAlpha(red, green, blue, alpha);
                float scale = BitConverter.ToSingle(bytes, iByte + scaleOffset);
                float orientation = BitConverter.ToSingle(bytes, iByte + orientationOffset);

                SiftFeature feature = new SiftFeature(x, y, scale, orientation, color);
                output[iFeature] = feature;
            }

            return output;
        }

        private static void SerializeSiftFeatures(FileStream fStream, SiftFeature[] features)
        {
            int valueSize = sizeof(float);
            int nBytes = SiftFile.HeaderPropertyNumber * valueSize;
            byte[] bytes = new byte[nBytes];

            int xOffset = 0 * valueSize;
            int yOffset = 1 * valueSize;
            int colorOffset = 2 * valueSize;
            int scaleOffset = 3 * valueSize;
            int orientationOffset = 4 * valueSize;
            
            foreach(var feature in features)
            {
                byte[] xBytes = BitConverter.GetBytes(feature.X);
                Array.Copy(xBytes, 0, bytes, xOffset, valueSize);

                byte[] yBytes = BitConverter.GetBytes(feature.Y);
                Array.Copy(yBytes, 0, bytes, yOffset, valueSize);

                byte[] colorBytes = new byte[] { feature.Color.Red, feature.Color.Green, feature.Color.Blue, feature.Color.Alpha };
                Array.Copy(colorBytes, 0, bytes, colorOffset, valueSize);

                byte[] scaleBytes = BitConverter.GetBytes(feature.Scale);
                Array.Copy(scaleBytes, 0, bytes, scaleOffset, valueSize);

                byte[] orientationBytes = BitConverter.GetBytes(feature.Orientation);
                Array.Copy(orientationBytes, 0, bytes, orientationOffset, valueSize);

                fStream.Write(bytes, 0, nBytes);
            }
        }

        private static SiftHeader DeserializeHeader(FileStream fStream)
        {
            // Read bytes.
            byte[] bytes = new byte[SiftBinarySerializer.HeaderByteCount];
            fStream.Read(bytes, 0, SiftBinarySerializer.HeaderByteCount);

            // Parse bytes.
            string siftFileMarker = Encoding.Default.GetString(bytes, 0, sizeof(Int32));
            if(SiftFile.SiftFileMarker != siftFileMarker) // Check that we have a SIFT file.
            {
                string message = $@"Invalid SIFT file marker: expected: {SiftFile.SiftFileMarker}, found: {siftFileMarker}.";
                throw new InvalidDataException(message);
            }

            string vVersionStr = Encoding.Default.GetString(bytes, sizeof(Int32), sizeof(Int32));
            string vStr = vVersionStr.Substring(0, 1);
            if(SiftFile.VersionMarker != vStr) // Check that the version string begins with V.
            {
                string message = $@"Invalid file version marker: expected {SiftFile.VersionMarker}, found: {vStr}";
                throw new InvalidDataException(message);
            }
            string versionStr = vVersionStr.Substring(1); // Remove the V prefix.
            Version version = Version.Parse(versionStr);

            int nPoints = BitConverter.ToInt32(bytes, 2 * sizeof(Int32));
            int nFeatureProperties = BitConverter.ToInt32(bytes, 3 * sizeof(Int32));
            int nDescriptorComponents = BitConverter.ToInt32(bytes, 4 * sizeof(Int32));

            SiftHeader output = new SiftHeader(siftFileMarker, version, nPoints, nFeatureProperties, nDescriptorComponents);
            return output;
        }

        private static void SerializeHeader(FileStream fStream, SiftHeader header)
        {
            byte[][] bytes = new byte[SiftFile.HeaderPropertyNumber][];

            byte[] nameBytes = Encoding.Default.GetBytes(header.Name);
            bytes[0] = nameBytes;

            string versionStr = $@"V{header.Version.ToString()}";
            byte[] versionBytes = Encoding.Default.GetBytes(versionStr);
            bytes[1] = versionBytes;

            byte[] nPointsBytes = BitConverter.GetBytes(header.NumberOfFeaturePoints);
            bytes[2] = nPointsBytes;

            byte[] nFeaturePropertyBytes = BitConverter.GetBytes(SiftFile.HeaderPropertyNumber);
            bytes[3] = nFeaturePropertyBytes;

            byte[] nDescriptorComponentBytes = BitConverter.GetBytes(header.NumberOfDescriptorComponents);
            bytes[4] = nDescriptorComponentBytes;

            byte[] bytesToWrite = bytes.Flatten();
            fStream.Write(bytesToWrite, 0, bytesToWrite.Length);
        }

        public static void Serialize(string filePath, SiftFile siftFile, bool overwrite = true)
        {
            FileMode fileMode = FileMode.Create;
            if (!overwrite)
            {
                fileMode = FileMode.CreateNew;
            }

            using (FileStream fStream = new FileStream(filePath, fileMode)) // Will allow the familiar error to percolate upwards if no overwriting.
            {
                SiftBinarySerializer.SerializeHeader(fStream, siftFile.Header);
                SiftBinarySerializer.SerializeSiftFeatures(fStream, siftFile.Features);
                SiftBinarySerializer.SerializeDescriptors(fStream, siftFile.Descriptors);
                SiftBinarySerializer.SerializeEofMarker(fStream, siftFile.EndByte);
            }
        }

        #endregion


        public SiftFile this[string filePath, bool overwrite = true]
        {
            get
            {
                var output = SiftBinarySerializer.Deserialize(filePath);
                return output;
            }
            set
            {
                SiftBinarySerializer.Serialize(filePath, value, overwrite);
            }
        }
    }
}
