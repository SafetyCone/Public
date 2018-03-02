using System;
using System.IO;

using Public.Common.Lib.Extensions;
using Public.Common.Lib.IO;
using Public.Common.Lib.IO.Serialization;


namespace Eshunna.Lib.Patches
{
    public class PatchCollectionV1Serializer : IFileSerializer<PatchCollection>
    {
        #region Static

        public static readonly string PatchesFileMarker = @"PATCHES";
        public static readonly string PatchMarker = @"PATCHS";
        private static readonly char[] Separators = new char[] { ' ' };


        public static PatchCollection Deserialize(string filePath)
        {
            using (LineReader reader = new LineReader(filePath))
            {
                string patchesMarkerLine = reader.ReadLine();
                if(PatchCollectionV1Serializer.PatchesFileMarker != patchesMarkerLine)
                {
                    string message = $@"Invalid patches file marker. Found: {patchesMarkerLine}, expected: {PatchCollectionV1Serializer.PatchesFileMarker}";
                    throw new InvalidDataException(message);
                }

                string nPatchesStr = reader.ReadLine();
                int nPatches = Convert.ToInt32(nPatchesStr);

                Patch[] patches = new Patch[nPatches];
                for (int iPatch = 0; iPatch < nPatches; iPatch++)
                {
                    Patch patch = PatchCollectionV1Serializer.DeserializePatch(reader);
                    patches[iPatch] = patch;
                }

                reader.ReadLine(); // Blank line.

                if(!reader.StreamReader.EndOfStream)
                {
                    string message = $@"End of file not reached after expected {nPatches.ToString()} patches.";
                    throw new InvalidDataException(message);
                }

                PatchCollection output = new PatchCollection(patches);
                return output;
            }
        }

        private static Patch DeserializePatch(LineReader reader)
        {
            string patchMarkerLine = reader.ReadLine();
            if(PatchCollectionV1Serializer.PatchMarker != patchMarkerLine)
            {
                string message = $@"Invalid patch marker. Found: {patchMarkerLine}, expected:{PatchCollectionV1Serializer.PatchMarker}";
                throw new InvalidDataException(message);
            }

            Location3DHomogenousDouble location = PatchCollectionV1Serializer.DeserializeLocation3DHomogenous(reader);
            Location3DHomogenousDouble normal = PatchCollectionV1Serializer.DeserializeLocation3DHomogenous(reader);

            string infoLine = reader.ReadLine();
            string[] infoTokens = infoLine.Split(PatchCollectionV1Serializer.Separators, StringSplitOptions.None);
            double[] infoValues = infoTokens.ConvertTo(Convert.ToDouble);
            double photometricConsistencyScore = infoValues[0];
            double debugging1 = infoValues[1];
            double debugging2 = infoValues[2];

            int[] imageIndicesWithGoodAgreement = PatchCollectionV1Serializer.DeserializeImageIndices(reader);
            int[] imageIndicesWithSomeAgreement = PatchCollectionV1Serializer.DeserializeImageIndices(reader);

            reader.ReadLine(); // Blank line.

            Patch output = new Patch(location, normal, photometricConsistencyScore, debugging1, debugging2, imageIndicesWithGoodAgreement, imageIndicesWithSomeAgreement);
            return output;
        }

        private static void SerializePatch(StreamWriter writer, Patch patch)
        {
            writer.WriteLine(PatchCollectionV1Serializer.PatchMarker);

            PatchCollectionV1Serializer.SerializeLocation3DHomogenous(writer, patch.Location);
            PatchCollectionV1Serializer.SerializeLocation3DHomogenous(writer, patch.Normal);

            string line = $@"{patch.PhotometricConsistencyScore.FormatPatch6SignificantDigits()} {patch.Debugging1.FormatPatch6SignificantDigits()} {patch.Debugging2.FormatPatch6SignificantDigits()}";
            writer.WriteLine(line);

            PatchCollectionV1Serializer.SerializeImageIndices(writer, patch.ImageIndicesWithGoodAgreement);
            PatchCollectionV1Serializer.SerializeImageIndices(writer, patch.ImageIndicesWithSomeAgreement);

            writer.WriteLine(); // Blank line.
        }

        private static int[] DeserializeImageIndices(LineReader reader)
        {
            string nIndicesStr = reader.ReadLine();
            int nIndices = Convert.ToInt32(nIndicesStr);

            string indicesLine = reader.ReadLine().Trim(); // Trailing space.
            string[] indexTokens = indicesLine.Split(PatchCollectionV1Serializer.Separators, StringSplitOptions.RemoveEmptyEntries);
            if(indexTokens.Length != nIndices)
            {
                string message = $@"Image index count mismatch. Found: {indexTokens.Length}, expected: {nIndices}.";
                throw new InvalidDataException(message);
            }

            int[] output;
            if(0 < nIndices)
            {
                output = indexTokens.ConvertTo(Convert.ToInt32);
            }
            else
            {
                output = new int[0];
            }
            return output;
        }

        private static void SerializeImageIndices(StreamWriter writer, int[] imageIndices)
        {
            int nIndices = imageIndices.Length;
            writer.WriteLine(nIndices);

            if (0 == nIndices)
            {
                writer.WriteLine(); // Blank line.
            }
            else
            {
                string line = String.Empty;
                for (int iIndex = 0; iIndex < nIndices; iIndex++)
                {
                    int imageIndex = imageIndices[iIndex];
                    line += (imageIndex.ToString() + @" ");
                }

                writer.WriteLine(line);
            }
        }

        private static Location3DHomogenousDouble DeserializeLocation3DHomogenous(LineReader reader)
        {
            string line = reader.ReadLine();

            string[] tokens = line.Split(PatchCollectionV1Serializer.Separators, StringSplitOptions.None);

            double[] values = tokens.ConvertTo(Convert.ToDouble);

            double x = values[0];
            double y = values[1];
            double z = values[2];
            double h = values[3];

            Location3DHomogenousDouble output = new Location3DHomogenousDouble(x, y, z, h);
            return output;
        }

        private static void SerializeLocation3DHomogenous(StreamWriter writer, Location3DHomogenousDouble location3DHomogenous)
        {
            string xStr = location3DHomogenous.X.FormatPatch6SignificantDigits();
            string yStr = location3DHomogenous.Y.FormatPatch6SignificantDigits();
            string zStr = location3DHomogenous.Z.FormatPatch6SignificantDigits();
            string hStr = Convert.ToInt32(location3DHomogenous.H).ToString();

            string line = $@"{xStr} {yStr} {zStr} {hStr}";
            writer.WriteLine(line);
        }

        public static void Serialize(string filePath, PatchCollection patchCollection, bool overwrite = true)
        {
            FileMode fileMode = FileMode.Create;
            if (!overwrite)
            {
                fileMode = FileMode.CreateNew;
            }

            using (FileStream fStream = new FileStream(filePath, fileMode)) // Will allow the familiar error to percolate upwards if no overwriting.
            using (StreamWriter writer = new StreamWriter(fStream)) // If you need text writing.
            {
                writer.WriteLine(PatchCollectionV1Serializer.PatchesFileMarker);

                int nPatches = patchCollection.Patches.Length;
                writer.WriteLine(nPatches.ToString());

                for (int iPatch = 0; iPatch < nPatches; iPatch++)
                {
                    Patch patch = patchCollection.Patches[iPatch];
                    PatchCollectionV1Serializer.SerializePatch(writer, patch);
                }
            }
        }

        #endregion


        public PatchCollection this[string filePath, bool overwrite = true]
        {
            get
            {
                var output = PatchCollectionV1Serializer.Deserialize(filePath);
                return output;
            }
            set
            {
                PatchCollectionV1Serializer.Serialize(filePath, value, overwrite);
            }
        }
    }
}
