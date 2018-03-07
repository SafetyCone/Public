using System;
using System.IO;
using System.Text;

using Public.Common.Lib.Extensions;
using Public.Common.Lib.IO.Serialization;


namespace Eshunna.Lib.NVM
{
    /// <summary>
    /// De/serializes an NViewMatch from a text file in the NVM_V3 specification.
    /// </summary>
    public class NvmV3Serializer : IFileSerializer<NViewMatch>
    {
        private const string CheckToken = @"0";


        #region Static

        public static readonly string NVM_V3Marker = @"NVM_V3";
        public static readonly string[] PlyFileComments =
        {
            @"#the last part of NVM file points to the PLY files",
            @"#the first number is the number of associated PLY files",
            @"#each following number gives a model-index that has PLY"
        };


        public static NViewMatch Deserialize(string filePath)
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                // Header.
                string formatLine = reader.ReadLineTrim(); // Trailing space.
                if(NvmV3Serializer.NVM_V3Marker != formatLine)
                {
                    string message = $@"Unknown format: {formatLine}. NVM serializer requires {NvmV3Serializer.NVM_V3Marker}.";
                    throw new InvalidDataException(message);
                }

                // Cameras.
                string nCamerasStr = reader.ReadNextNonBlankLine();
                int nCameras = Convert.ToInt32(nCamerasStr);

                var cameras = new Camera[nCameras];
                var cameraSeparators = new char[] { ' ', '\t' }; // Tab after image file name.
                for (int iCamera = 0; iCamera < nCameras; iCamera++)
                {
                    string cameraLine = reader.ReadLineTrim(); // Trailing space.
                    var tokens = cameraLine.Split(cameraSeparators, StringSplitOptions.None);

                    Camera camera = NvmV3Serializer.DeserializeCamera(tokens);
                    cameras[iCamera] = camera;
                }

                // Feature points.
                string nFeaturePointsStr = reader.ReadNextNonBlankLine();
                int nFeaturePoints = Convert.ToInt32(nFeaturePointsStr);

                var featurePoints = new FeaturePoint[nFeaturePoints];
                var featurePointsSeparators = new char[] { ' ' };
                for (int iFeaturePoint = 0; iFeaturePoint < nFeaturePoints; iFeaturePoint++)
                {
                    string featurePointLine = reader.ReadLineTrim(); // Trailing space.
                    var tokens = featurePointLine.Split(featurePointsSeparators, StringSplitOptions.None);

                    FeaturePoint featurePoint = NvmV3Serializer.DeserializeFeaturePoint(tokens);
                    featurePoints[iFeaturePoint] = featurePoint;
                }

                string endOfFeaturesCheck = reader.ReadNextNonBlankLine();
                if(NvmV3Serializer.CheckToken != endOfFeaturesCheck)
                {
                    string message = $@"Features deserialization check token failure. Found: {endOfFeaturesCheck}, expected: {NvmV3Serializer.CheckToken}";
                    throw new InvalidDataException(message);
                }

                // Models with PLY files.
                reader.ReadNextNonBlankLine(); // Ignore three lines of comments.
                reader.ReadLine();
                reader.ReadLine();

                string modelsWithPlyLine = reader.ReadLine();
                var separators = new char[] { ' ' };
                var modelsWithPlyTokens = modelsWithPlyLine.Split(separators, StringSplitOptions.None);

                string nModelsWithPlyStr = modelsWithPlyTokens[0];
                int nModelsWithPly = Convert.ToInt32(nModelsWithPlyStr);

                if(modelsWithPlyTokens.Length - 1 != nModelsWithPly)
                {
                    string message = $@"Model count mismatch. Expected {nModelsWithPly} models to have PLY files, but only {modelsWithPlyTokens.Length - 1} found.";
                    throw new InvalidDataException(message);
                }

                int[] modelsWithPly = new int[nModelsWithPly];
                for (int iModelWithPly = 0; iModelWithPly < nModelsWithPly; iModelWithPly++)
                {
                    int index = Convert.ToInt32(modelsWithPlyTokens[iModelWithPly + 1]);
                    modelsWithPly[iModelWithPly] = index;
                }

                NViewMatch output = new NViewMatch(cameras, featurePoints, modelsWithPly);
                return output;
            }
        }

        private static FeaturePoint DeserializeFeaturePoint(string[] tokens)
        {
            string locationXStr = tokens[0];
            double locationX = Convert.ToDouble(locationXStr);
            string locationYStr = tokens[1];
            double locationY = Convert.ToDouble(locationYStr);
            string locationZStr = tokens[2];
            double locationZ = Convert.ToDouble(locationZStr);
            Location3Double location = new Location3Double(locationX, locationY, locationZ);

            string redStr = tokens[3];
            byte red = Convert.ToByte(redStr);
            string greenStr = tokens[4];
            byte green = Convert.ToByte(greenStr);
            string blueStr = tokens[5];
            byte blue = Convert.ToByte(blueStr);
            Color color = new Color(red, green, blue);

            string nMeasurementsStr = tokens[6];
            int nMeasurements = Convert.ToInt32(nMeasurementsStr);

            var measurements = new Measurement[nMeasurements];
            int startIndex = 7;
            for (int iMeasurement = 0; iMeasurement < nMeasurements; iMeasurement++)
            {
                Measurement measurement = NvmV3Serializer.DeserializeMeasurement(tokens, startIndex);
                measurements[iMeasurement] = measurement;

                startIndex += 4;
            }

            FeaturePoint output = new FeaturePoint(location, color, measurements);
            return output;
        }

        private static void SerializeFeaturePoint(StringBuilder builder, FeaturePoint featurePoint)
        {
            string[] strings =
                {
                featurePoint.Location.X.FormatNvm12SignificantDigits(),
                featurePoint.Location.Y.FormatNvm12SignificantDigits(),
                featurePoint.Location.Z.FormatNvm12SignificantDigits(),
                featurePoint.Color.Red.ToString(),
                featurePoint.Color.Green.ToString(),
                featurePoint.Color.Blue.ToString(),
                featurePoint.Measurements.Length.ToString()
            };
            NvmV3Serializer.AppendStrings(builder, strings);

            foreach(var measurement in featurePoint.Measurements)
            {
                NvmV3Serializer.SerializeMeasurement(builder, measurement);
            }
        }

        private static Measurement DeserializeMeasurement(string[] tokens, int startIndex)
        {
            string imageIndexStr = tokens[startIndex + 0];
            int imageIndex = Convert.ToInt32(imageIndexStr);

            string featureIndexStr = tokens[startIndex + 1];
            int featureIndex = Convert.ToInt32(featureIndexStr);

            string locationXStr = tokens[startIndex + 2];
            double locationX = Convert.ToDouble(locationXStr);
            string locationYStr = tokens[startIndex + 3];
            double locationY = Convert.ToDouble(locationYStr);
            Location2Double location = new Location2Double(locationX, locationY);

            Measurement output = new Measurement(imageIndex, featureIndex, location);
            return output;
        }

        private static void SerializeMeasurement(StringBuilder builder, Measurement measurement)
        {
            string[] strings =
            {
                measurement.ImageIndex.ToString(),
                measurement.FeatureIndex.ToString(),
                measurement.Location.X.FormatNvm12SignificantDigits(),
                measurement.Location.Y.FormatNvm12SignificantDigits()
            };
            NvmV3Serializer.AppendStrings(builder, strings);
        }

        private static Camera DeserializeCamera(string[] tokens)
        {
            string fileName = tokens[0];

            string focalLengthStr = tokens[1];
            double focalLength = Convert.ToDouble(focalLengthStr);

            string quaternionWStr = tokens[2];
            double quaternionW = Convert.ToDouble(quaternionWStr);
            string quaternionXStr = tokens[3];
            double quaternionX = Convert.ToDouble(quaternionXStr);
            string quaternionYStr = tokens[4];
            double quaternionY = Convert.ToDouble(quaternionYStr);
            string quaternionZStr = tokens[5];
            double quaternionZ = Convert.ToDouble(quaternionZStr);
            QuaternionDouble rotation = new QuaternionDouble(quaternionW, quaternionX, quaternionY, quaternionZ);

            string locationXStr = tokens[6];
            double locationX = Convert.ToDouble(locationXStr);
            string locationYStr = tokens[7];
            double locationY = Convert.ToDouble(locationYStr);
            string locationZStr = tokens[8];
            double locationZ = Convert.ToDouble(locationZStr);
            Location3Double location = new Location3Double(locationX, locationY, locationZ);

            string radialDistortionCoefficientStr = tokens[9];
            double radialDistortionCoefficient = Convert.ToDouble(radialDistortionCoefficientStr);

            string checkToken = tokens[10];
            if(NvmV3Serializer.CheckToken != checkToken)
            {
                string message = $@"Camera deserialization check token failure. Found: {checkToken}, expected: {NvmV3Serializer.CheckToken}";
                throw new InvalidDataException(message);
            }

            Camera output = new Camera(fileName, focalLength, rotation, location, radialDistortionCoefficient);
            return output;
        }

        private static void SerializeCamera(StringBuilder builder, Camera camera)
        {
            builder.Append(camera.FileName);
            builder.Append('\t');

            double[] values = {
                camera.FocalLength,
                camera.Rotation.W,
                camera.Rotation.X,
                camera.Rotation.Y,
                camera.Rotation.Z,
                camera.Location.X,
                camera.Location.Y,
                camera.Location.Z,
                camera.RadialDistortionCoefficient,
            };
            NvmV3Serializer.AppendValues(builder, values);

            builder.Append(@"0");
            builder.Append(@" ");
        }

        private static void AppendValues(StringBuilder builder, double[] values)
        {
            int nValues = values.Length;
            string[] strings = new string[nValues];
            for (int iValue = 0; iValue < nValues; iValue++)
            {
                double value = values[iValue];
                string valuestr = value.FormatNvm12SignificantDigits();
                strings[iValue] = valuestr;
            }

            NvmV3Serializer.AppendStrings(builder, strings);
        }

        private static void AppendStrings(StringBuilder builder, string[] strings)
        {
            foreach(var valueStr in strings)
            {
                builder.Append(valueStr);
                builder.Append(@" ");
            }
        }

        public static void Serialize(string filePath, NViewMatch nvm, bool overwrite = true)
        {
            FileMode fileMode = FileMode.Create;
            if (!overwrite)
            {
                fileMode = FileMode.CreateNew;
            }

            using (FileStream fStream = new FileStream(filePath, fileMode))
            using (StreamWriter writer = new StreamWriter(fStream))
            {
                StringBuilder builder = new StringBuilder();

                // Header.
                writer.WriteLine(NvmV3Serializer.NVM_V3Marker + @" ");
                writer.WriteLine();

                // Cameras.
                writer.WriteLine(nvm.Cameras.Length.ToString());
                foreach(var camera in nvm.Cameras)
                {
                    builder.Clear();
                    NvmV3Serializer.SerializeCamera(builder, camera);

                    string line = builder.ToString();
                    writer.WriteLine(line);
                }

                writer.WriteLine();

                // Feature points.
                writer.WriteLine(nvm.FeaturePoints.Length.ToString());
                foreach (var featurePoint in nvm.FeaturePoints)
                {
                    builder.Clear();
                    NvmV3Serializer.SerializeFeaturePoint(builder, featurePoint);

                    string line = builder.ToString();
                    writer.WriteLine(line);
                }

                writer.WriteLine();
                writer.WriteLine();
                writer.WriteLine();
                writer.WriteLine(@"0");
                writer.WriteLine();

                foreach(var line in NvmV3Serializer.PlyFileComments)
                {
                    writer.WriteLine(line);
                }

                // Models with ply.
                builder.Clear();
                int nModelsWithPly = nvm.ModelIndicessWithPlyFiles.Length;
                builder.Append(nModelsWithPly.ToString());
                for (int iModelWithPly = 0; iModelWithPly < nModelsWithPly; iModelWithPly++)
                {
                    builder.Append(@" ");
                    string valueStr = nvm.ModelIndicessWithPlyFiles[iModelWithPly].ToString();
                    builder.Append(valueStr);
                }

                string modelsWithPlyLine = builder.ToString();
                writer.WriteLine(modelsWithPlyLine);
            }
        }

        #endregion


        public NViewMatch this[string filePath, bool overwrite = true]
        {
            get
            {
                var output = NvmV3Serializer.Deserialize(filePath);
                return output;
            }
            set
            {
                NvmV3Serializer.Serialize(filePath, value, overwrite);
            }
        }
    }
}
