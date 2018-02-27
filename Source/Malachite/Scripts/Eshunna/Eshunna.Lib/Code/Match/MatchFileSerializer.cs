using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Public.Common.Lib.Extensions;
using Public.Common.Lib.IO.Serialization;


namespace Eshunna.Lib.Match
{
    public class MatchFileSerializer : IFileSerializer<MatchFile>
    {
        public const string DefaultMatchFileVersionMarker = @"MT03";
        public const string DefaultMatchRecordVersionMarker = @"MRV3";
        private const int NHeaderBytes = 20;
        private const int NRecordLocationBytes = 20;
        private const int NRecordBytes = 188; // 4 + (NM) + 4 (record version) + 152 (two view geometry) + 7 * 4 (7 extra ints).
        private const int NTwoViewGeometryBytes = 152;


        #region Static

        public static MatchFile Deserialize(string filePath)
        {
            using (FileStream fStream = new FileStream(filePath, FileMode.Open))
            {
                MatchFileHeader header = MatchFileSerializer.DeserializeHeader(fStream);

                MatchFileDefinition definition = MatchFileSerializer.DeserializeDefinition(fStream, header);

                var records = MatchFileSerializer.DeserializeRecords(fStream, definition.FileNamesInOrder, definition.RecordLocationsByFileName);

                MatchFile output = new MatchFile(header, definition, records.Item1, records.Item2, records.Item3);
                return output;
            }
        }

        private static MatchFileDefinition DeserializeDefinition(FileStream fStream, MatchFileHeader header)
        {
            var recordLocations = MatchFileSerializer.DeserializeRecordLocations(fStream, header.FileCount);
            int nRecords = recordLocations.Count;

            // Organize the record locations by file name.
            var fileNamesInOrder = new List<string>(nRecords);
            var recordLocationsByFileName = new Dictionary<string, RecordLocation>(nRecords);
            foreach (var recordLocation in recordLocations)
            {
                string fileName = recordLocation.FileName;
                fileNamesInOrder.Add(fileName);
                recordLocationsByFileName.Add(fileName, recordLocation);
            }

            int nTriviaBytes = header.DefinitionBufferSize + MatchFileSerializer.NHeaderBytes - Convert.ToInt32(fStream.Position);
            byte[] triviaBytes = new byte[nTriviaBytes];
            fStream.Read(triviaBytes, 0, nTriviaBytes);

            var output = new MatchFileDefinition(fileNamesInOrder, recordLocationsByFileName, triviaBytes);
            return output;
        }

        private static void SerializeDefinition(FileStream fStream, MatchFileDefinition definition)
        {
            List<RecordLocation> recordLocations = new List<RecordLocation>(definition.FileNamesInOrder.Count);
            foreach (var fileName in definition.FileNamesInOrder)
            {
                var recordLocation = definition.RecordLocationsByFileName[fileName];
                recordLocations.Add(recordLocation);
            }
            MatchFileSerializer.SerializeRecordLocations(fStream, recordLocations);

            fStream.Write(definition.Trivia, 0, definition.Trivia.Length);
        }

        private static Tuple<Dictionary<string, MatchRecordV3>, Dictionary<string, int[,]>, Dictionary<string, int[,]>> DeserializeRecords(FileStream fStream, List<string> fileNamesInOrder, Dictionary<string, RecordLocation> recordLocationsByFileName)
        {
            int nRecords = fileNamesInOrder.Count;

            var matchRecordsByFileName = new Dictionary<string, MatchRecordV3>(nRecords);
            var allMatchesByFileName = new Dictionary<string, int[,]>(nRecords);
            var inlierMatchesByFileName = new Dictionary<string, int[,]>(nRecords);

            foreach (var fileName in fileNamesInOrder)
            {
                var recordLocation = recordLocationsByFileName[fileName];

                var record = MatchFileSerializer.DeserializeRecord(fStream, recordLocation);

                matchRecordsByFileName.Add(fileName, record.Item1);
                allMatchesByFileName.Add(fileName, record.Item2);
                inlierMatchesByFileName.Add(fileName, record.Item3);
            }

            var output = Tuple.Create(matchRecordsByFileName, allMatchesByFileName, inlierMatchesByFileName);
            return output;
        }

        private static void SerializeRecords(FileStream fStream, MatchFile matchFile)
        {
            int nFiles = matchFile.Definition.FileNamesInOrder.Count;
            for (int iFileName = 0; iFileName< nFiles; iFileName++)
            {
                var fileName = matchFile.Definition.FileNamesInOrder[iFileName];

                var recordLocation = matchFile.Definition.RecordLocationsByFileName[fileName];
                var record = matchFile.RecordsByFileName[fileName];
                var allMatches = matchFile.AllMatchesByFileName[fileName];
                var inlierMatches = matchFile.InlierMatchesByFileName[fileName];

                bool isLastRecord = iFileName == (nFiles - 1);
                MatchFileSerializer.SerializeRecord(fStream, recordLocation, record, allMatches, inlierMatches, isLastRecord);
            }
        }

        private static Tuple<MatchRecordV3, int[,], int[,]> DeserializeRecord(FileStream fStream, RecordLocation recordLocation)
        {
            //fStream.Seek(recordLocation.ReadLocation, SeekOrigin.Begin); // Need to seek from the beginning for each record since they are not contiguous.

            // Get the record.
            byte[] bytes = new byte[MatchFileSerializer.NRecordBytes];
            fStream.Read(bytes, 0, MatchFileSerializer.NRecordBytes);

            int nMatchesOffset = 0;
            int versionOffset = nMatchesOffset + sizeof(int);
            int twoViewGeometryOffset = versionOffset + sizeof(int);
            int extraOffset = twoViewGeometryOffset + MatchFileSerializer.NTwoViewGeometryBytes;

            int nMatches = BitConverter.ToInt32(bytes, nMatchesOffset);

            string version = Encoding.Default.GetString(bytes, versionOffset, sizeof(int));
            if (MatchFileSerializer.DefaultMatchRecordVersionMarker != version)
            {
                string message = $@"Invalid match record. Recorder version marker: expected: {MatchFileSerializer.DefaultMatchRecordVersionMarker}, found: {version}";
                throw new InvalidDataException(message);
            }

            TwoViewGeometry twoViewGeometry = MatchFileSerializer.DeserializeTwoViewGeometry(bytes, twoViewGeometryOffset);

            int[] extraInts = MatchFileSerializer.DeserializeIntegerArray(bytes, extraOffset, MatchRecordV3.DefaultNumberOfExtraInts);

            // Get all the matches.
            int[,] allMatches = MatchFileSerializer.DeserializeIntegerArray2D(fStream, 2, nMatches);

            // Get inlier matches.
            int nInlierMatches = twoViewGeometry.NF;
            int[,] inlierMatches = MatchFileSerializer.DeserializeIntegerArray2D(fStream, 2, nInlierMatches);

            // Get the trivia bytes.
            int nTriviaBytes = recordLocation.ReadLocation + recordLocation.BlockSize - Convert.ToInt32(fStream.Position);
            byte[] triviaBytes = new byte[nTriviaBytes];
            fStream.Read(triviaBytes, 0, nTriviaBytes);

            MatchRecordV3 record = new MatchRecordV3(version, twoViewGeometry, extraInts, triviaBytes);

            var output = Tuple.Create(record, allMatches, inlierMatches);
            return output;
        }

        private static void SerializeRecord(FileStream fStream, RecordLocation recordLocation, MatchRecordV3 record, int[,] allMatches, int[,] inlierMatches, bool isLastRecord)
        {
            byte[] nMatchesBytes = BitConverter.GetBytes(allMatches.GetLength(1));
            byte[] versionBytes = Encoding.ASCII.GetBytes(record.Version);
            byte[] tvgBytes = MatchFileSerializer.SerializeTwoViewGeometry(record.TwoViewGeometry);
            byte[] extraBytes = MatchFileSerializer.SerializeIntegerArray(record.Extra);
            byte[] allMatchesBytes = MatchFileSerializer.SerializeIntegerArray2D(allMatches);
            byte[] inlierMatchesBytes = MatchFileSerializer.SerializeIntegerArray2D(inlierMatches);

            byte[][] bytesArray = new byte[][] { nMatchesBytes, versionBytes, tvgBytes, extraBytes, allMatchesBytes, inlierMatchesBytes };

            byte[] bytes = bytesArray.Flatten();
            fStream.Write(bytes, 0, bytes.Length);

            if(!isLastRecord)
            {
                fStream.Write(record.Trivia, 0, record.Trivia.Length);
            }
        }

        private static int[,] DeserializeIntegerArray2D(FileStream fileStream, int nRows, int nCols)
        {
            int byteCount = nRows * nCols * sizeof(Int32);
            byte[] bytes = new byte[byteCount];
            fileStream.Read(bytes, 0, byteCount);

            int[,] output = new int[nRows, nCols];

            int inlierOffset = 0;
            for (int iRow = 0; iRow < nRows; iRow++)
            {
                for (int iCol = 0; iCol < nCols; iCol++)
                {
                    int index = BitConverter.ToInt32(bytes, inlierOffset);
                    output[iRow, iCol] = index;

                    inlierOffset += sizeof(int);
                }
            }

            return output;
        }

        private static byte[] SerializeIntegerArray2D(int[,] array)
        {
            int rows = array.GetLength(0);
            int cols = array.GetLength(1);

            int nElements = rows * cols;
            byte[][] bytesArray = new byte[nElements][];
            int iElement = 0;
            for (int iRow = 0; iRow < rows; iRow++)
            {
                for (int iCol = 0; iCol < cols; iCol++)
                {
                    int value = array[iRow, iCol];
                    byte[] valueBytes = BitConverter.GetBytes(value);
                    bytesArray[iElement] = valueBytes;

                    iElement++;
                }
            }

            byte[] bytes = bytesArray.Flatten();
            return bytes;
        }

        private static int[] DeserializeIntegerArray(byte[] bytes, int startOffset, int count)
        {
            int[] output = new int[count];
            for (int iInt = 0; iInt < count; iInt++)
            {
                int value = BitConverter.ToInt32(bytes, startOffset + iInt * sizeof(Int32));
                output[iInt] = value;
            }

            return output;
        }

        private static byte[] SerializeIntegerArray(int[] array)
        {
            int nInts = array.Length;
            byte[][] bytesArray = new byte[nInts][];
            for (int iInt = 0; iInt < nInts; iInt++)
            {
                int value = array[iInt];
                byte[] valueBytes = BitConverter.GetBytes(value);
                bytesArray[iInt] = valueBytes;
            }

            byte[] output = bytesArray.Flatten();
            return output;
        }

        private static TwoViewGeometry DeserializeTwoViewGeometry(byte[] bytes, int startOffset)
        {
            int nfOffset = startOffset;
            int neOffset = nfOffset + sizeof(int);
            int nhOffset = neOffset + sizeof(int);
            int nh2Offset = nhOffset + sizeof(int);
            int fOffset = nh2Offset + sizeof(int);
            int rOffset = fOffset + 9 * sizeof(float);
            int tOffset = rOffset + 9 * sizeof(float);
            int f1Offset = tOffset + 3 * sizeof(float);
            int f2Offset = f1Offset + sizeof(float);
            int hOffset = f2Offset + sizeof(float);
            int geOffset = hOffset + 9 * sizeof(float);
            int aaOffset = geOffset + sizeof(float);

            int nf = BitConverter.ToInt32(bytes, nfOffset);
            int ne = BitConverter.ToInt32(bytes, neOffset);
            int nh = BitConverter.ToInt32(bytes, nhOffset);
            int nh2 = BitConverter.ToInt32(bytes, nh2Offset);
            MatrixFloat f = MatchFileSerializer.DeserializeMatrixFloat(bytes, fOffset, 3, 3);
            MatrixFloat r = MatchFileSerializer.DeserializeMatrixFloat(bytes, rOffset, 3, 3);
            float[] t = MatchFileSerializer.DeserializeFloatArray(bytes, tOffset, 3);
            float f1 = BitConverter.ToSingle(bytes, f1Offset);
            float f2 = BitConverter.ToSingle(bytes, f2Offset);
            MatrixFloat h = MatchFileSerializer.DeserializeMatrixFloat(bytes, hOffset, 3, 3);
            float ge = BitConverter.ToSingle(bytes, geOffset);
            float aa = BitConverter.ToSingle(bytes, aaOffset);

            TwoViewGeometry output = new TwoViewGeometry(nf, ne, nh, nh2, f, r, t, f1, f2, h, ge, aa);
            return output;
        }

        private static byte[] SerializeTwoViewGeometry(TwoViewGeometry twoViewGeometry)
        {
            var nfArray = BitConverter.GetBytes(twoViewGeometry.NF);
            var neArray = BitConverter.GetBytes(twoViewGeometry.NE);
            var nhArray = BitConverter.GetBytes(twoViewGeometry.NH);
            var nh2Array = BitConverter.GetBytes(twoViewGeometry.NH2);
            var fArray = MatchFileSerializer.SerializeMatrixFloat(twoViewGeometry.F);
            var rArray = MatchFileSerializer.SerializeMatrixFloat(twoViewGeometry.R);
            var tArray = MatchFileSerializer.SerializeFloatArray(twoViewGeometry.T);
            var f1Array = BitConverter.GetBytes(twoViewGeometry.F1);
            var f2Array = BitConverter.GetBytes(twoViewGeometry.F2);
            var hArray = MatchFileSerializer.SerializeMatrixFloat(twoViewGeometry.H);
            var geArray = BitConverter.GetBytes(twoViewGeometry.GE);
            var aaArray = BitConverter.GetBytes(twoViewGeometry.AA);

            byte[][] bytesArray = new byte[][] { nfArray, neArray, nhArray, nh2Array, fArray, rArray, tArray, f1Array, f2Array, hArray, geArray, aaArray };

            byte[] output = bytesArray.Flatten();
            return output;
        }

        private static float[] DeserializeFloatArray(byte[] bytes, int startOffset, int count)
        {
            float[] output = new float[count];
            for (int iValue = 0; iValue < count; iValue++)
            {
                int index = startOffset + iValue * sizeof(float);
                float value = BitConverter.ToSingle(bytes, index);
                output[iValue] = value;
            }

            return output;
        }

        private static byte[] SerializeFloatArray(float[] values)
        {
            int nValues = values.Length;
            byte[][] bytesArray = new byte[nValues][];
            for (int iValue = 0; iValue < nValues; iValue++)
            {
                float value = values[iValue];
                byte[] bytes = BitConverter.GetBytes(value);
                bytesArray[iValue] = bytes;
            }

            byte[] output = bytesArray.Flatten();
            return output;
        }

        private static MatrixFloat DeserializeMatrixFloat(byte[] bytes, int startOffset, int rows, int columns)
        {
            int nValues = rows * columns;
            float[] values = new float[nValues];
            int iValue = 0;
            for (int iRow = 0; iRow < rows; iRow++)
            {
                for (int iCol = 0; iCol < columns; iCol++)
                {
                    int index = startOffset + iValue * sizeof(float);
                    float value = BitConverter.ToSingle(bytes, index);
                    values[iValue] = value;

                    iValue++;
                }
            }

            MatrixFloat output = new MatrixFloat(rows, columns, values);
            return output;
        }

        private static byte[] SerializeMatrixFloat(MatrixFloat matrixFloat)
        {
            float[] values = matrixFloat.RowMajorValues;
            int nValues = values.Length;

            byte[][] bytesArray = new byte[nValues][];
            for (int iValue = 0; iValue < nValues; iValue++)
            {
                float value = values[iValue];
                byte[] valueBytes = BitConverter.GetBytes(value);
                bytesArray[iValue] = valueBytes;
            }

            byte[] output = bytesArray.Flatten();
            return output;
        }

        private static List<RecordLocation> DeserializeRecordLocations(FileStream fStream, int fileCount)
        {
            var output = new List<RecordLocation>(fileCount);
            for (int iFile = 0; iFile < fileCount; iFile++)
            {
                RecordLocation recordLocation = MatchFileSerializer.DeserializeRecordLocation(fStream);
                output.Add(recordLocation);
            }

            return output;
        }

        private static void SerializeRecordLocations(FileStream fStream, List<RecordLocation> recordLocations)
        {
            int nRecords = recordLocations.Count;
            for (int iRecord = 0; iRecord < nRecords; iRecord++)
            {
                RecordLocation recordLocation = recordLocations[iRecord];
                MatchFileSerializer.SerializeRecordLocation(fStream, recordLocation);
            }
        }

        private static RecordLocation DeserializeRecordLocation(FileStream fStream)
        {
            byte[] bytes = new byte[MatchFileSerializer.NRecordLocationBytes];
            fStream.Read(bytes, 0, MatchFileSerializer.NRecordLocationBytes);

            int intSize = sizeof(int);
            int featureCountOffset = 0 * intSize;
            int readLocationOffset = 1 * intSize;
            int blockSizeOffset = 2 * intSize;
            int trashSizeOffset = 3 * intSize;
            int extraSizeOffset = 4 * intSize;

            int featureCount = BitConverter.ToInt32(bytes, featureCountOffset);
            int readLocation = BitConverter.ToInt32(bytes, readLocationOffset);
            int blockSize = BitConverter.ToInt32(bytes, blockSizeOffset);
            int trashSize = BitConverter.ToInt32(bytes, trashSizeOffset);
            int extraSize = BitConverter.ToInt32(bytes, extraSizeOffset);

            int nExtraBytes = extraSize + 4; // Perhaps an oversight, the first four bytes (chars) of the filename are double-counted. For an 8 char file name, the extra bytes equals 8. Space is allocated for 4 chars, then for the 8 extra bytes. I think the first four chars of the filename are double-counted.
            byte[] extraBytes = new byte[nExtraBytes];
            fStream.Read(extraBytes, 0, nExtraBytes);

            string fileName = Encoding.Default.GetString(extraBytes, 0, extraSize);

            RecordLocation output = new RecordLocation(featureCount, readLocation, blockSize, trashSize, extraSize, fileName);
            return output;
        }

        private static void SerializeRecordLocation(FileStream fStream, RecordLocation recordLocation)
        {
            byte[] featureCountBytes = BitConverter.GetBytes(recordLocation.FeatureCount);
            byte[] readLocationBytes = BitConverter.GetBytes(recordLocation.ReadLocation);
            byte[] blockSizeBytes = BitConverter.GetBytes(recordLocation.BlockSize);
            byte[] trashSize = BitConverter.GetBytes(recordLocation.TrashSize);
            byte[] extraSizeBytes = BitConverter.GetBytes(recordLocation.ExtraSize);
            byte[] fileNameBytes = Encoding.ASCII.GetBytes(recordLocation.FileName);
            byte[] extraFourBytes = new byte[4];

            byte[][] byteArray = new byte[][] { featureCountBytes, readLocationBytes, blockSizeBytes, trashSize, extraSizeBytes, fileNameBytes, extraFourBytes };

            byte[] bytes = byteArray.Flatten();
            fStream.Write(bytes, 0, bytes.Length);
        }

        private static MatchFileHeader DeserializeHeader(FileStream fStream)
        {
            byte[] bytes = new byte[MatchFileSerializer.NHeaderBytes];
            fStream.Read(bytes, 0, MatchFileSerializer.NHeaderBytes);

            int intSize = sizeof(int);
            int versionOffset = 0 * intSize;
            int fileCountOffset = 1 * intSize;
            int definitionSizeOffset = 2 * intSize;
            int defintionBufferOffset = 3 * intSize;
            int featureCountOffset = 4 * intSize;

            string version = Encoding.Default.GetString(bytes, 0, intSize);
            if(MatchFileSerializer.DefaultMatchFileVersionMarker != version)
            {
                string message = $@"Invalid match file. File version marker expected: {MatchFileSerializer.DefaultMatchFileVersionMarker}, found: {version}";
                throw new InvalidDataException(message);
            }

            int fileCount = BitConverter.ToInt32(bytes, fileCountOffset);
            int definitionSize = BitConverter.ToInt32(bytes, definitionSizeOffset);
            int definitionBuffer = BitConverter.ToInt32(bytes, defintionBufferOffset);
            int featureCount = BitConverter.ToInt32(bytes, featureCountOffset);

            MatchFileHeader output = new MatchFileHeader(version, fileCount, definitionSize, definitionBuffer, featureCount);
            return output;
        }

        private static void SerializeHeader(FileStream fStream, MatchFileHeader header)
        {
            byte[] versionBytes = Encoding.ASCII.GetBytes(header.Version);
            byte[] fileCountBytes = BitConverter.GetBytes(header.FileCount);
            byte[] definitionSizeBytes = BitConverter.GetBytes(header.DefinitionSize);
            byte[] definitionBufferSizeBytes = BitConverter.GetBytes(header.DefinitionBufferSize);
            byte[] featureCountBytes = BitConverter.GetBytes(header.FeatureCount);

            byte[][] byteArray = new byte[][] { versionBytes, fileCountBytes, definitionSizeBytes, definitionBufferSizeBytes, featureCountBytes };

            byte[] bytes = byteArray.Flatten();
            fStream.Write(bytes, 0, bytes.Length);
        }

        public static void Serialize(string filePath, MatchFile matchFile, bool overwrite = true)
        {
            FileMode fileMode = FileMode.Create;
            if (!overwrite)
            {
                fileMode = FileMode.CreateNew;
            }

            using (FileStream fStream = new FileStream(filePath, fileMode)) // Will allow the familiar error to percolate upwards if no overwriting.
            {
                MatchFileSerializer.SerializeHeader(fStream, matchFile.Header);

                MatchFileSerializer.SerializeDefinition(fStream, matchFile.Definition);

                MatchFileSerializer.SerializeRecords(fStream, matchFile);
            }
        }

        #endregion


        public MatchFile this[string filePath, bool overwrite = true]
        {
            get
            {
                var output = MatchFileSerializer.Deserialize(filePath);
                return output;
            }
            set
            {
                MatchFileSerializer.Serialize(filePath, value, overwrite);
            }
        }
    }
}
