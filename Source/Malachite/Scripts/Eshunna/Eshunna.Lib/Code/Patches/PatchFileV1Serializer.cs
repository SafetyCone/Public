using System;
using System.IO;

using Public.Common.Lib.Extensions;
using Public.Common.Lib.IO;
using Public.Common.Lib.IO.Serialization;


namespace Eshunna.Lib.Patches
{
    public class PatchFileV1Serializer : IFileSerializer<PatchFile>
    {
        #region Static

        public static readonly string PatchesFileMarker = @"PATCHES";
        public static readonly string PatchMarker = @"PATCHS";
        private static readonly char[] Separators = new char[] { ' ' };


        public static PatchFile Deserialize(string filePath)
        {
            using (LineReader reader = new LineReader(filePath))
            {
                string patchesMarkerLine = reader.ReadLine();
                if(PatchFileV1Serializer.PatchesFileMarker != patchesMarkerLine)
                {
                    string message = $@"Invalid patches file marker. Found: {patchesMarkerLine}, expected: {PatchFileV1Serializer.PatchesFileMarker}";
                    throw new InvalidDataException(message);
                }

                string nPatchesStr = reader.ReadLine();
                int nPatches = Convert.ToInt32(nPatchesStr);

                Patch[] patches = new Patch[nPatches];
                for (int iPatch = 0; iPatch < nPatches; iPatch++)
                {
                    Patch patch = PatchFileV1Serializer.DeserializePatch(reader);
                    patches[iPatch] = patch;
                }

                reader.ReadLine(); // Blank line.

                if(!reader.StreamReader.EndOfStream)
                {
                    string message = $@"End of file not reached after expected {nPatches.ToString()} patches.";
                    throw new InvalidDataException(message);
                }

                PatchFile output = new PatchFile(patches);
                return output;
            }
        }

        private static Patch DeserializePatch(LineReader reader)
        {
            string patchMarkerLine = reader.ReadLine();
            if(PatchFileV1Serializer.PatchMarker != patchMarkerLine)
            {
                string message = $@"Invalid patch marker. Found: {patchMarkerLine}, expected:{PatchFileV1Serializer.PatchMarker}";
                throw new InvalidDataException(message);
            }

            Location3HomogenousDouble location = PatchFileV1Serializer.DeserializeLocation3DHomogenous(reader);
            Vector4Double normal = PatchFileV1Serializer.DeserializeLocation3DHomogenous(reader).ToVector4Double();

            string infoLine = reader.ReadLine();
            string[] infoTokens = infoLine.Split(PatchFileV1Serializer.Separators, StringSplitOptions.None);
            double[] infoValues = infoTokens.ConvertTo(Convert.ToDouble);
            double photometricConsistencyScore = infoValues[0];
            double debugging1 = infoValues[1];
            double debugging2 = infoValues[2];

            int[] imageIndicesWithGoodAgreement = PatchFileV1Serializer.DeserializeImageIndices(reader);
            int[] imageIndicesWithSomeAgreement = PatchFileV1Serializer.DeserializeImageIndices(reader);

            reader.ReadLine(); // Blank line.

            Patch output = new Patch(location, normal, photometricConsistencyScore, debugging1, debugging2, imageIndicesWithGoodAgreement, imageIndicesWithSomeAgreement);
            return output;
        }

        private static void SerializePatch(StreamWriter writer, Patch patch)
        {
            writer.WriteLine(PatchFileV1Serializer.PatchMarker);

            PatchFileV1Serializer.SerializeLocation3DHomogenous(writer, patch.Location);
            PatchFileV1Serializer.SerializeLocation3DHomogenous(writer, patch.Normal.ToLocation3HomogenousDouble());

            string line = $@"{patch.PhotometricConsistencyScore.FormatPatch6SignificantDigits()} {patch.Debugging1.FormatPatch6SignificantDigits()} {patch.Debugging2.FormatPatch6SignificantDigits()}";
            writer.WriteLine(line);

            PatchFileV1Serializer.SerializeImageIndices(writer, patch.ImageIndicesWithGoodAgreement);
            PatchFileV1Serializer.SerializeImageIndices(writer, patch.ImageIndicesWithSomeAgreement);

            writer.WriteLine(); // Blank line.
        }

        private static int[] DeserializeImageIndices(LineReader reader)
        {
            string nIndicesStr = reader.ReadLine();
            int nIndices = Convert.ToInt32(nIndicesStr);

            string indicesLine = reader.ReadLine().Trim(); // Trailing space.
            string[] indexTokens = indicesLine.Split(PatchFileV1Serializer.Separators, StringSplitOptions.RemoveEmptyEntries);
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

        private static Location3HomogenousDouble DeserializeLocation3DHomogenous(LineReader reader)
        {
            string line = reader.ReadLine();

            string[] tokens = line.Split(PatchFileV1Serializer.Separators, StringSplitOptions.None);

            double[] values = tokens.ConvertTo(Convert.ToDouble);

            double x = values[0];
            double y = values[1];
            double z = values[2];
            double h = values[3];

            Location3HomogenousDouble output = new Location3HomogenousDouble(x, y, z, h);
            return output;
        }

        private static void SerializeLocation3DHomogenous(StreamWriter writer, Location3HomogenousDouble location3DHomogenous)
        {
            string xStr = location3DHomogenous.X.FormatPatch6SignificantDigits();
            string yStr = location3DHomogenous.Y.FormatPatch6SignificantDigits();
            string zStr = location3DHomogenous.Z.FormatPatch6SignificantDigits();
            string hStr = Convert.ToInt32(location3DHomogenous.H).ToString();

            string line = $@"{xStr} {yStr} {zStr} {hStr}";
            writer.WriteLine(line);
        }

        public static void Serialize(string filePath, PatchFile patchCollection, bool overwrite = true)
        {
            FileMode fileMode = FileMode.Create;
            if (!overwrite)
            {
                fileMode = FileMode.CreateNew;
            }

            using (FileStream fStream = new FileStream(filePath, fileMode)) // Will allow the familiar error to percolate upwards if no overwriting.
            using (StreamWriter writer = new StreamWriter(fStream)) // If you need text writing.
            {
                writer.WriteLine(PatchFileV1Serializer.PatchesFileMarker);

                int nPatches = patchCollection.Patches.Length;
                writer.WriteLine(nPatches.ToString());

                for (int iPatch = 0; iPatch < nPatches; iPatch++)
                {
                    Patch patch = patchCollection.Patches[iPatch];
                    PatchFileV1Serializer.SerializePatch(writer, patch);
                }
            }
        }

        #endregion


        public PatchFile this[string filePath, bool overwrite = true]
        {
            get
            {
                var output = PatchFileV1Serializer.Deserialize(filePath);
                return output;
            }
            set
            {
                PatchFileV1Serializer.Serialize(filePath, value, overwrite);
            }
        }
    }
}
