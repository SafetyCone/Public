using System;
using System.IO;
using System.Text;

using Public.Common.Lib.IO;
using Public.Common.Lib.IO.Serialization;


namespace Eshunna.Lib.PLY
{
    public class PlyV1BinarySerializer : IFileSerializer<PlyFile>
    {
        #region Static

        public static PlyFile Deserialize(string filePath)
        {
            PlyFileHeader header;
            long maxPosition;
            using (LineReader reader = new LineReader(filePath))
            {
                PlyV1TextSerializer.EnsureFileMarker(reader);

                // Read the header.
                header = PlyV1TextSerializer.DeserializeHeader(reader);

                maxPosition = reader.StreamReader.BaseStream.Position;
            }

            // Build the output.
            PlyFile output = PlyFile.Build(header);

            // Build the value converters.
            ValueConverter[][] elementValueConverters = PlyFile.BuildValueConverters(output);

            using (FileStream fStream = new FileStream(filePath, FileMode.Open))
            {
                int binaryStart = PlyV1BinarySerializer.FindBinaryStartPosition(fStream, maxPosition);

                fStream.Seek(binaryStart - 5, SeekOrigin.Begin);

                int nBytes = 20;
                byte[] bytes = new byte[nBytes];
                fStream.Read(bytes, 0, nBytes);

                string text = Encoding.Default.GetString(bytes);
            }

            // Fill the output.
            using (FileStream fStream = new FileStream(filePath, FileMode.Open))
            {
                // Seek the correct position in the file.
                int binaryStart = PlyV1BinarySerializer.FindBinaryStartPosition(fStream, maxPosition);
                fStream.Seek(binaryStart, SeekOrigin.Begin);

                int nElements = header.Elements.Count;
                for (int iElement = 0; iElement < nElements; iElement++)
                {
                    var elementDescriptor = header.Elements[iElement];
                    ValueConverter[] valueConverters = elementValueConverters[iElement];

                    int nProperties = elementDescriptor.PropertyDescriptors.Count;
                    int nElementValues = elementDescriptor.Count;
                    for (int iElementValue = 0; iElementValue < nElementValues; iElementValue++)
                    {
                        for (int iProperty = 0; iProperty < nProperties; iProperty++)
                        {
                            PlyPropertyDescriptor propertyDescriptor = elementDescriptor.PropertyDescriptors[iProperty];
                            ValueConverter valueConverter = valueConverters[iProperty];
                            valueConverter.ConvertAndAdd(fStream, iElementValue);
                        }
                    }
                }
            }

            return output;
        }

        private static int FindBinaryStartPosition(FileStream fStream, long maxPosition)
        {
            byte[] bytesToFind = Encoding.Default.GetBytes(PlyFile.PlyFileHeaderEnd);
            int nBytesToFind = bytesToFind.Length;

            fStream.Seek(0, SeekOrigin.Begin);
            byte[] bytesToSearch = new byte[maxPosition + 10]; // Add extra in case we ended the header serialization on a perfect boundary.
            fStream.Read(bytesToSearch, 0, Convert.ToInt32(maxPosition)); // Conversion from long to int. For now we assume we will never have a gigabyte sized header.

            int binaryStart = 0;
            int output = 0;
            while(true)
            {
                bool found = true;
                for (int iByte = 0; iByte < nBytesToFind; iByte++)
                {
                    if(bytesToSearch[binaryStart + iByte] != bytesToFind[iByte])
                    {
                        found = false;
                        break;
                    }
                }

                if(found)
                {
                    output = binaryStart + nBytesToFind;

                    if ((byte)13 == bytesToSearch[output])
                    {
                        output++;
                    }

                    if ((byte)10 == bytesToSearch[output])
                    {
                        output++;
                    }

                    break;
                }
                else
                {
                    binaryStart++;
                }
            }

            fStream.Seek(0, SeekOrigin.Begin);

            return output;
        }

        public static void Serialize(string filePath, PlyFile plyFile, bool overwrite = true)
        {
            FileMode fileMode = FileMode.Create;
            if (!overwrite)
            {
                fileMode = FileMode.CreateNew;
            }

            using (FileStream fStream = new FileStream(filePath, fileMode)) // Will allow the familiar error to percolate upwards if no overwriting.
            {
                PlyFileHeader header = plyFile.Header;

                // Write the header text.
                using (StreamWriter writer = new StreamWriter(fStream, Encoding.Default, 1024, true)) // If you need text writing.
                {
                    writer.WriteLine(PlyFile.PlyFileMarker);

                    PlyV1TextSerializer.SerializeFormat(writer, header.FileDataFormat, header.FileFormatVersion);

                    foreach (var comment in header.Comments)
                    {
                        string commentLine = PlyV1TextSerializer.SerializeComment(comment);
                        writer.WriteLine(commentLine);
                    }

                    foreach (var elementDescriptor in header.Elements)
                    {
                        PlyV1TextSerializer.SerializeElementDescriptor(writer, elementDescriptor);
                    }

                    writer.WriteLine(PlyFile.PlyFileHeaderEnd);
                }

                // Build the value converters.
                ValueConverter[][] elementValueConverters = PlyFile.BuildValueConverters(plyFile);

                // Now write values.
                int nElements = header.Elements.Count;
                for (int iElement = 0; iElement < nElements; iElement++)
                {
                    var elementDescriptor = header.Elements[iElement];
                    var element = plyFile.Values[elementDescriptor.Name];
                    ValueConverter[] valueConverters = elementValueConverters[iElement];

                    int nElementValues = elementDescriptor.Count;
                    int nProperties = elementDescriptor.PropertyDescriptors.Count;
                    for (int iElementValue = 0; iElementValue < nElementValues; iElementValue++)
                    {
                        for (int iProperty = 0; iProperty < nProperties; iProperty++)
                        {
                            ValueConverter valueConverter = valueConverters[iProperty];
                            byte[] bytes = valueConverter.ToBytes(iElementValue);
                            fStream.Write(bytes, 0, bytes.Length);
                        }
                    }
                }
            }
        }

        #endregion


        public PlyFile this[string filePath, bool overwrite = true]
        {
            get
            {
                var output = PlyV1BinarySerializer.Deserialize(filePath);
                return output;
            }
            set
            {
                PlyV1BinarySerializer.Serialize(filePath, value, overwrite);
            }
        }
    }
}
