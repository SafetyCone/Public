using System;
using System.Dynamic;
using System.IO;

using Public.Common.Lib;
using Public.Common.Lib.Extensions;
using Public.Common.Lib.IO;
using PathUtilities = Public.Common.Lib.IO.Paths.Utilities;
using Public.Common.Lib.IO.Serialization;
using Public.Common.Lib.MATLAB;
using Public.Common.Lib.Visuals;
using ImageFileExtensions = Public.Common.Lib.Visuals.IO.FileExtensions;
using Public.Common.Lib.Visuals.MetadataExtractor;
using Public.Common.MATLAB;


namespace Opis
{
    class Program
    {
        static void Main(string[] args)
        {
            //Program.TestMatlab();
            //Program.TestMetadataExtractor();
            //Program.TestDynamic();
            //Program.TestDynamicCreationFromMatlab();
            Program.TestMarshallDynamicToMatlab();
            //Program.SubMain();
        }

        private static void SubMain()
        {
            string imageDirectoryPath = @"E:\Organizations\Minex\Data\Images\Camera Calibration\Checkerboard\iPhone 6\Session 2";
            string exampleImagePath = @"E:\Organizations\Minex\Data\Images\Princeton\Campus\IMG_0012.JPG";

            bool forceCheckerboardRedetection = false;
            string[] imageFileExtensions = new string[] { ImageFileExtensions.JpgFileExtension, ImageFileExtensions.PngFileExtension };

            // Load the checkerboard configuration parameters. // worldUnits, boardSize, and squareSize;
            

            // Get image file paths.
            string[] imageFilePaths = DirectoryExtensions.FilePathsByExtensions(imageDirectoryPath, imageFileExtensions);

            // Get an image size. Use the size of the first image assuming all images are the same size.
            IExternalFormatImageSizeProvider sizeProvider = new ExternalFormatImageSizeProvider();

            string firstImageFilePath = imageFilePaths[0];
            ImageSize imageSize = sizeProvider.GetSize(firstImageFilePath);

            // Get the camera calibration parameters.
            CameraCalibrationParameters cameraCalibrationParameters = Program.GetCameraCalibrationParameters(imageSize);

            // Setup MATLAB.
            var matlabApplication = new MatlabApplication(true, false) { LeaveOpen = true };
            //using (var matlabApplication = new MatlabApplication(true, false))
            //{
            matlabApplication.AddPublicCommonLibraryPath();

            string opisMFilesPath = Path.Combine(PathUtilities.ExecutableDirectoryPath, Matlab.MFilesDirectoryName, @"Opis");
            matlabApplication.AddPath(opisMFilesPath);

            double[,,] imagePoints = Program.DetectCheckerboardPoints(matlabApplication, imageDirectoryPath, imageFilePaths, forceCheckerboardRedetection);

            var parameterSets = Program.CalibrateCamera(matlabApplication, imagePoints, cameraCalibrationParameters);

            string cameraParametersFilePath = Path.Combine(imageDirectoryPath, @"CameraParameters.dat");
            parameterSets.Item1.SerializeToFile(cameraParametersFilePath);
            string estimationParametersFilePath = Path.Combine(imageDirectoryPath, @"estimationParameters.dat");
            parameterSets.Item2.SerializeToFile(estimationParametersFilePath);

            ExpandoObject test = new ExpandoObject();
            test.DeserializeFromFile(cameraParametersFilePath);
        }

        private static Tuple<ExpandoObject, ExpandoObject> CalibrateCamera(MatlabApplication matlabApplication, double[,,] imagePoints, CameraCalibrationParameters cameraCalibrationParameters)
        {
            matlabApplication.PutRealArray(@"imagePoints", imagePoints);

            using (Variable worldUnits = new Variable(matlabApplication, @"worldUnits", $@"worldUnits = '{cameraCalibrationParameters.BoardConfiguration.WorldUnits}'"))
            using (Variable boardSize = new Variable(matlabApplication, @"boardSize", $@"boardSize = [{cameraCalibrationParameters.BoardConfiguration.BoardSize.RowCount.ToString()}, {cameraCalibrationParameters.BoardConfiguration.BoardSize.ColumnCount.ToString()}];"))
            using (Variable squareSize = new Variable(matlabApplication, @"squareSize", cameraCalibrationParameters.BoardConfiguration.SquareSize))
            using (Variable boardConfiguration = new Variable(matlabApplication, @"boardConfiguration"))
            using (Variable estimateSkew = new Variable(matlabApplication, @"estimateSkew", cameraCalibrationParameters.CameraCalibrationEstimationParameters.EstimateSkew))
            using (Variable estimateTangentialDistortion = new Variable(matlabApplication, @"estimateTangentialDistortion", cameraCalibrationParameters.CameraCalibrationEstimationParameters.EstimateTangentialDistortion))
            using (Variable numRadialDistortionCoefficients = new Variable(matlabApplication, @"numRadialDistortionCoefficients", Convert.ToDouble(cameraCalibrationParameters.CameraCalibrationEstimationParameters.RadialDistortionCoefficientCount)))
            using (Variable estimationParameters = new Variable(matlabApplication, @"estimationParameters"))
            using (Variable imageSize = new Variable(matlabApplication, @"imageSize", $@"imageSize = [{cameraCalibrationParameters.ImageSize.Rows.ToString()}, {cameraCalibrationParameters.ImageSize.Columns.ToString()}]"))
            {
                matlabApplication.Execute(@"boardConfiguration.worldUnits = worldUnits;");
                matlabApplication.Execute(@"boardConfiguration.boardSize = boardSize;");
                matlabApplication.Execute(@"boardConfiguration.squareSize = squareSize;");
                matlabApplication.Execute(@"estimationParameters.estimateSkew = estimateSkew;");
                matlabApplication.Execute(@"estimationParameters.estimateTangentialDistortion = estimateTangentialDistortion;");
                matlabApplication.Execute(@"estimationParameters.numRadialDistortionCoefficients = numRadialDistortionCoefficients;");

                matlabApplication.Execute(@"[cameraParams, imagesUsed, estimationErrors] = calibrateCameraStructs(imagePoints, boardConfiguration, estimationParameters, imageSize);");

                ExpandoObject cameraParameters = matlabApplication.GetStructure(@"cameraParams");
                ExpandoObject estimationErrors = matlabApplication.GetStructure(@"estimationErrors");

                var output = Tuple.Create(cameraParameters, estimationErrors);
                return output;
            }
        }

        private static CameraCalibrationParameters GetCameraCalibrationParameters(ImageSize imageSize)
        {
            bool estimateSkew = false;
            bool estimateTangentialDistortion = false;
            RadialDistortionCoefficientCount radialDistortionCoefficientCount = RadialDistortionCoefficientCount.Two;

            string worldUnits = @"millimeters";
            int[] boardSize = new int[] { 7, 10 }; // Height, width or rows, columns.
            double squareSize = 23; // In world units.

            CameraCalibrationEstimationParameters cameraCalibrationEstimationParameters = new CameraCalibrationEstimationParameters(estimateSkew, estimateTangentialDistortion, radialDistortionCoefficientCount);
            BoardConfiguration boardConfiguration = new BoardConfiguration(worldUnits, new MatrixSize(boardSize), squareSize);

            CameraCalibrationParameters output = new CameraCalibrationParameters(boardConfiguration, cameraCalibrationEstimationParameters, imageSize);
            return output;
        }

        private static double[,,] DetectCheckerboardPoints(MatlabApplication matlabApplication, string imageDirectoryPath, string[] imageFilePaths, bool forceCheckerboardRedetection)
        {
            // Detect checkerboards in images, and get image points array.
            // Determine if the image points have already been calculated. If they have, skip computation and load the file (unless we have forced checkerboard redetection).
            string dataDirectoryPath = Path.Combine(imageDirectoryPath, @"Data");
            string imagePointsFileName = @"imagePoints.dat";
            string imagePointsFilePath = Path.Combine(dataDirectoryPath, imagePointsFileName);

            double[,,] imagePoints;
            if (File.Exists(imagePointsFilePath) && !forceCheckerboardRedetection)
            {
                // Load the file.
                imagePoints = BinaryFileSerializer.Deserialize<double[,,]>(imagePointsFilePath);
            }
            else
            {
                // Compute or recompute checkerboard image points.
                matlabApplication.PutData(@"imageFilePaths", imageFilePaths);

                string command = @"[imagePoints, imagesUsedLogicals] = detectCheckerboardImagePoints(imageFilePaths)";
                matlabApplication.Execute(command);

                imagePoints = matlabApplication.GetData<double[,,]>(@"imagePoints");

                // If computed, save the image points.
                if(!Directory.Exists(dataDirectoryPath))
                {
                    Directory.CreateDirectory(dataDirectoryPath);
                }

                // Save the world points array since it requires so much time to calculate.
                BinaryFileSerializer.Serialize(imagePointsFilePath, imagePoints);
            }

            return imagePoints;
        }

        private static void TestMarshallDynamicToMatlab()
        {
            var temp = new ExpandoObject();
            int aValue = 1;
            temp.AddElement(@"a", aValue);
            bool bValue = true;
            temp.AddElement(@"b", bValue);
            string cValue = @"ccc";
            temp.AddElement(@"c", cValue);
            double[,] dValue = new double[,] { { 1, 2 }, { 3, 4 } };
            temp.AddElement(@"d", dValue);
            double[,,] eValue = new double[,,] { { { 1, 2 }, { 3, 4 } }, { { 5, 6 }, { 7, 8 } } };
            temp.AddElement(@"e", eValue);
            ExpandoObject fValue = temp.ShallowCopy(); // Copy the current state to avoid infinite recursion.
            temp.AddElement(@"f", fValue);
            object[] gValue = new object[] { aValue, bValue, cValue, dValue, eValue, fValue };
            temp.AddElement(@"g", gValue);

            using (var matlabApplication = new MatlabApplication())
            {
                matlabApplication.PutStructure(@"temp2", temp);
            }
        }

        private static void TestDynamicCreationFromMatlab()
        {
            using (var matlabApplication = new MatlabApplication())
            {
                matlabApplication.Reset();

                string command;

                //matlabApplication.Clear(@"AA");
                //matlabApplication.Clear(@"AA"); OK to clear variables that are not present.

                // Build an object in MATLAB.
                command = @"temp.a = 1;";
                matlabApplication.Execute(command);
                command = @"temp.b = true;";
                matlabApplication.Execute(command);
                command = @"temp.c = 'ccc';";
                matlabApplication.Execute(command);
                command = @"temp.d = zeros(2, 2, 2);";
                matlabApplication.Execute(command);
                command = @"temp.e = temp;";
                matlabApplication.Execute(command);
                command = @"temp.f = {1, true, 'ccc', zeros(2, 2, 2), temp };"; // Add a structure parameter.
                matlabApplication.Execute(command);

                dynamic temp = matlabApplication.GetStructure(@"temp");

                command = $@"{Matlab.AnswerVariableName} = fieldnames(temp);";
                matlabApplication.Execute(command);
                string[] fieldNames = matlabApplication.GetStringArray(Matlab.AnswerVariableName);

                string structureName = @"temp";

                ExpandoObject expando = new ExpandoObject();
                foreach (string fieldName in fieldNames)
                {
                    command = $@"{Matlab.AnswerVariableName} = class({structureName}.{fieldName})";
                    matlabApplication.Execute(command);

                    string fieldType = matlabApplication.GetString(Matlab.AnswerVariableName);

                    command = $@"{Matlab.AnswerVariableName} = numel({structureName}.{fieldName})";
                    matlabApplication.Execute(command);

                    double numericalLength = matlabApplication.GetDouble(Matlab.AnswerVariableName); // Should be int.

                    if(1 < numericalLength && fieldType != @"char") // Because a string is a char array.
                    {
                        command = $@"{Matlab.AnswerVariableName} = size({structureName}.{fieldName})";
                        matlabApplication.Execute(command);

                        double[] size = matlabApplication.GetData<double[]>(Matlab.AnswerVariableName); // Should be an array of ints.
                    }
                }
            }
        }

        private static void TestDynamic()
        {
            dynamic dyn = new System.Dynamic.ExpandoObject();
            dyn.a = 1;
            dyn.b = true;
            dyn.c = @"ccc";
            dyn.d = new System.Dynamic.ExpandoObject();
            
            //var expandoDict = dyn as IDic
        }

        private static void TestMetadataExtractor()
        {
            string filePath = @"E:\Organizations\Minex\Data\Images\Princeton\Campus\IMG_0012.JPG";

            IExternalFormatImageSizeProvider sizeProvider = new ExternalFormatImageSizeProvider();

            var size = sizeProvider.GetSize(filePath);
        }

        private static void TestMatlab()
        {
            using (var app = new MatlabApplication(true))
            {
                //string variableName = @"x";
                //string command = $@"{variableName}= 1:3";
                //app.Execute(command);

                //object objValues = app.GetData(variableName);
                //double[,] values = objValues.ConvertTo<double[,]>();
                //int[] intValues = new int[] { Convert.ToInt32(values[0, 0]), Convert.ToInt32(values[0, 1]), Convert.ToInt32(values[0, 2]) };

                //string cdInitial = app.CurrentDirectory();
                //app.CurrentDirectory(Matlab.UserDirectoryPath);
                //string cdUserDirectory = app.CurrentDirectory();

                //app.Execute(@"startup");

                //app.CurrentDirectory(@"C:\Organizations\Minex\Repositories\Minex\Source\MyTheia\Scripts\Borsippa");
                //app.Which(@"startup");
                //app.Which(@"startup", true);

                //string[] path = app.Path();

                //bool initially = app.IsAvailable(@"filePathsByExtension");
                //app.AddPublicCommonLibraryPath();
                //bool afterStartup = app.IsAvailable(@"filePathsByExtension");

                //string imageDirectoryPath = @"E:\Organizations\Minex\Data\Images\Camera Calibration\Checkerboard\iPhone 6\Session 2";
                //string[] imageFileExtensions = new string[] { @"jpg", @"png" };

                //// Use MATLAB to get image file paths of interest.
                //string[] filePathsMatlab = app.FilePathsByExtension(imageDirectoryPath, imageFileExtensions);
                //string[] filePathsCSharp = DirectoryExtensions.FilePathsByExtensions(imageDirectoryPath, imageFileExtensions);

                string command;
                command = @"temp.a = 1;";
                app.Execute(command);
                command = @"temp.b = true;";
                app.Execute(command);
                command = @"temp.c = 'c';";
                app.Execute(command);

                //object tempObj = app.GetData(@"temp"); // Returns null.
                dynamic temp = app.GetDynamic(@"temp");
            }
        }
    }
}
