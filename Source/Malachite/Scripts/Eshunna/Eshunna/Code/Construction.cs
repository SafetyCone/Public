using System;
using System.Collections.Generic;
using System.IO;

using Public.Common.Lib.IO;
using Public.Common.Lib.IO.Serialization;


using Eshunna.Lib;
using Eshunna.Lib.Logging;
using Eshunna.Lib.NVM;
using Eshunna.Lib.Patches;
using Eshunna.Lib.PLY;
using Eshunna.Lib.Verification;
using EshunnaProperties = Eshunna.Properties;


namespace Eshunna
{
    public static class Construction
    {
        public static void SubMain()
        {
            //Construction.Scratch();
            //Construction.RoundTripNvmFileToNetBinary();
            //Construction.RoundTripNViewMatch();
            //Construction.RoundTripNvmFile();
            //Construction.DeserializePatchCollection();
            //Construction.RoundTripPatchFile();
            //Construction.DeserializePlyTextFile();
            Construction.SerializePlyTextFile();
        }

        private static void RoundTripPlyFile()
        {
            var properties = Program.GetProjectProperties();

            string exampleFilePath = properties[EshunnaProperties.ExamplePlyTextFilePathPropertyName];
            string serializationFilePath = @"C:\temp\temp.ply";

            var serializer = new PlyV1TextSerializer();

            var log = new StringListLog();
            //var patchCollectionComparer = new PatchCollectionEqualityComparer(log);

            bool patchCollectionsEqual = RoundTripExternalDataStructureVerifier.Verify(serializer, exampleFilePath, serializationFilePath, patchCollectionComparer);
            if (!patchCollectionsEqual)
            {
                string logFilePath = @"C:\temp\patch collections.txt";
                File.WriteAllLines(logFilePath, log.Lines);
            }

            var fileComparer = new TextFileComparer(log);

            bool textFilesEqual = RoundTripExternalFileFormat.Verify(serializer, exampleFilePath, serializationFilePath, fileComparer);
            if (!textFilesEqual)
            {
                string logFilePath = @"C:\temp\text file.txt";
                File.WriteAllLines(logFilePath, log.Lines);
            }
        }

        private static void SerializePlyTextFile()
        {
            var properties = Program.GetProjectProperties();

            string exampleFilePath = properties[EshunnaProperties.ExamplePlyTextFilePathPropertyName];
            string serializationFilePath = @"C:\temp\temp.ply";

            PlyFile plyFile = PlyV1TextSerializer.Deserialize(exampleFilePath);

            PlyV1TextSerializer.Serialize(serializationFilePath, plyFile);
        }

        private static void DeserializePlyTextFile()
        {
            var properties = Program.GetProjectProperties();

            string exampleFilePath = properties[EshunnaProperties.ExamplePlyTextFilePathPropertyName];

            PlyFile plyFile = PlyV1TextSerializer.Deserialize(exampleFilePath);
        }

        private static void RoundTripPatchFile()
        {
            var properties = Program.GetProjectProperties();

            string exampleFilePath = properties[EshunnaProperties.ExamplePatchFilePathPropertyName];
            string serializationFilePath = @"C:\temp\temp.patch";

            var serializer = new PatchCollectionV1Serializer();

            var log = new StringListLog();
            var patchCollectionComparer = new PatchCollectionEqualityComparer(log);

            bool patchCollectionsEqual = RoundTripExternalDataStructureVerifier.Verify(serializer, exampleFilePath, serializationFilePath, patchCollectionComparer);
            if (!patchCollectionsEqual)
            {
                string logFilePath = @"C:\temp\patch collections.txt";
                File.WriteAllLines(logFilePath, log.Lines);
            }

            var fileComparer = new TextFileComparer(log);

            bool textFilesEqual = RoundTripExternalFileFormat.Verify(serializer, exampleFilePath, serializationFilePath, fileComparer);
            if (!textFilesEqual)
            {
                string logFilePath = @"C:\temp\text file.txt";
                File.WriteAllLines(logFilePath, log.Lines);
            }
        }

        private static void DeserializePatchCollection()
        {
            var properties = Program.GetProjectProperties();

            string exampleFilePath = properties[EshunnaProperties.ExamplePatchFilePathPropertyName];

            PatchCollection patchCollection = PatchCollectionV1Serializer.Deserialize(exampleFilePath);
        }

        private static void RoundTripNvmFile()
        {
            var properties = Program.GetProjectProperties();

            string exampleFilePath = properties[EshunnaProperties.ExampleNvmFilePathPropertyName];
            string serializationFilePath = @"C:\temp\temp.nvm";

            var serializer = new NvmV3Serializer();

            var log = new StringListLog();
            var fileComparer = new TextFileComparer(log);

            bool textFilesEqual = RoundTripExternalFileFormat.Verify(serializer, exampleFilePath, serializationFilePath, fileComparer);
            if(!textFilesEqual)
            {
                string logFilePath = @"C:\temp\log.txt";
                File.WriteAllLines(logFilePath, log.Lines);
            }
        }

        private static void RoundTripNViewMatch()
        {
            var properties = Program.GetProjectProperties();

            string exampleFilePath = properties[EshunnaProperties.ExampleNvmFilePathPropertyName];
            string serializationFilePath = @"C:\temp\temp.nvm";

            var serializer = new NvmV3Serializer();

            var log = new StringListLog();
            var comparer = new NViewMatchEqualityComparer(log);

            bool nViewMatchesEqual = RoundTripExternalDataStructureVerifier.Verify(serializer, exampleFilePath, serializationFilePath, comparer);
            if (!nViewMatchesEqual)
            {
                string logFilePath = @"C:\temp\log.txt";
                File.WriteAllLines(logFilePath, log.Lines);
            }
        }

        private static void RoundTripNvmFileToNetBinary()
        {
            var properties = Program.GetProjectProperties();

            string exampleFilePath = properties[EshunnaProperties.ExampleNvmFilePathPropertyName];

            NViewMatch nViewMatch = NvmV3Serializer.Deserialize(exampleFilePath);

            string tempBinaryFileName = @"C:\temp\Model NVM.dat";
            BinaryFileSerializer.Serialize(tempBinaryFileName, nViewMatch);

            NViewMatch nViewMatch2 = BinaryFileSerializer.Deserialize<NViewMatch>(tempBinaryFileName);


        }

        private static void Scratch()
        {
            //double[] values = { -20.1572380596, -0.0269060720416, 103.726450237, 10.3726450237, 1.03726450237, -0.0700699378582, -0.00945108265552 };
            //string[] strings = { @"-20.1572380596", @"-0.0269060720416", @"103.726450237", @"10.3726450237", @"1.03726450237", @"-0.0700699378582", @"-0.00945108265552" };

            //int nValues = values.Length;
            //for (int iValue = 0; iValue < nValues; iValue++)
            //{
            //    double value = values[iValue];
            //    string @string = strings[iValue];

            //    string valueAs12DigitString = value.Format12SignificantDigits();
            //    if(@string != valueAs12DigitString)
            //    {
            //        throw new Exception();
            //    }
            //}

            //double value = 0.263498472003;
            //double value = -141;
            //double value = -2.77555756156e-017;
            //string valueStr = value.FormatNvm12SignificantDigits();

            //double value = -9.8642e-005;
            //string valueStr = value.FormatPatch6SignificantDigits();

            int[][] temp = new int[5][];

            temp[0] = new int[2];
            temp[1] = new int[3];
            temp[2] = new int[4];
        }
    }
}
