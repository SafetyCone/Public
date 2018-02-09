using System;

using ML = MLApp;

using Public.Common.Lib.Extensions;


namespace Public.Common.MATLAB
{
    public class MatlabApplication : IDisposable
    {
        public const string BaseWorkspaceName = @"base";
        public const string ErrorIndicator = @"???";
        private static readonly int ErrorIndicatorLength = MatlabApplication.ErrorIndicator.Length;
        public const string DeleteFunctionName = @"delete";


        #region Static

        private static MatlabApplication zInstance;
        public static MatlabApplication Instance
        {
            get
            {
                MatlabApplication.Start();

                return zInstance;
            }
        }


        public static void Start()
        {
            if (null == MatlabApplication.zInstance)
            {
                MatlabApplication.zInstance = new MatlabApplication();
            }
        }

        public static void Stop()
        {
            if (null != MatlabApplication.zInstance)
            {
                MatlabApplication.zInstance.Dispose();

                MatlabApplication.zInstance = null;
            }
        }

        public static double[] GetComplementZeros(double[] realValues)
        {
            double[] output = new double[realValues.Length];
            return output;
        }

        public static double[,] GetComplementZeros(double[,] realValues)
        {
            double[,] output = new double[realValues.GetLength(0), realValues.GetLength(1)];
            return output;
        }

        public static double[,,] GetComplementZeros(double[,,] realValues)
        {
            double[,,] output = new double[realValues.GetLength(0), realValues.GetLength(1), realValues.GetLength(2)];
            return output;
        }

        public static Array GetComplementZeros(Array realValues)
        {
            Type elementType = realValues.GetElementType();
            int[] lengths = realValues.GetLengths();

            Array output = Array.CreateInstance(elementType, lengths);
            return output;
        }

        public static string GetTemporaryVariableName()
        {
            string output = Guid.NewGuid().ToString().Replace(@"-", String.Empty); // OK, less than the 63 character max variable name length of MATLAB.
            return output;
        }

        public static string GetTemporaryVariableName(string baseVariableName)
        {
            string suffix = MatlabApplication.GetTemporaryVariableName();

            string combination = baseVariableName + suffix;
            string output;
            if (64 > combination.Length)
            {
                output = combination;
            }
            else
            {
                output = combination.Substring(0, 63);
            }

            return output;
        }

        #endregion

        #region IDisposable Members

        private bool zDisposed = false;


        public void Dispose()
        {
            this.CleanUp(true);

            GC.SuppressFinalize(this);
        }

        private void CleanUp(bool disposing)
        {
            if (!this.zDisposed)
            {
                if (disposing)
                {
                    // Call Dispose() on any contained managed disposable resources here.
                }

                // Clean-up unmanaged resources here.
                this.MlApplication.Quit();
            }

            this.zDisposed = true;
        }

        ~MatlabApplication()
        {
            this.CleanUp(false);
        }

        #endregion


        public bool Visible
        {
            get
            {
                bool output = 1 == this.MlApplication.Visible;
                return output;
            }
            set
            {
                if (value)
                {
                    this.MlApplication.Visible = 1;
                }
                else
                {
                    this.MlApplication.Visible = 0;
                }
            }
        }
        public bool WindowMaximized
        {
            set
            {
                if (value)
                {
                    this.MlApplication.MaximizeCommandWindow();
                }
                else
                {
                    this.MlApplication.MinimizeCommandWindow();
                }
            }
        }


        internal ML.MLApp MlApplication { get; set; }


        public MatlabApplication() : this(false, false) { }

        public MatlabApplication(bool visible) : this(visible, visible) { }

        public MatlabApplication(bool visible, bool maximized)
        {
            this.MlApplication = new ML.MLApp();

            this.Visible = visible;
            this.WindowMaximized = maximized;
        }

        public string Execute(string command, bool throwOnError)
        {
            string output = this.MlApplication.Execute(command);
            if (null != output && String.Empty != output && output.Substring(0, MatlabApplication.ErrorIndicatorLength) == MatlabApplication.ErrorIndicator && throwOnError)
            {
                throw new MatlabException(output);
            }

            return output;
        }

        public string Execute(string command)
        {
            string output = this.Execute(command, true);
            return output;
        }

        public void PutData(string variableName, string workspaceName, object data)
        {
            this.MlApplication.PutWorkspaceData(variableName, workspaceName, data);
        }

        public void PutData(string variableName, object data)
        {
            this.PutData(variableName, MatlabApplication.BaseWorkspaceName, data);
        }

        public object GetData(string variableName, string workspaceName)
        {
            this.MlApplication.GetWorkspaceData(variableName, workspaceName, out object output);

            return output;
        }

        public object GetData(string variableName)
        {
            object output = this.GetData(variableName, MatlabApplication.BaseWorkspaceName);
            return output;
        }

        public void PutString(string variableName, string workspaceName, string value)
        {
            this.MlApplication.PutCharArray(variableName, workspaceName, value);
        }

        public void PutString(string variableName, string value)
        {
            this.PutString(variableName, MatlabApplication.BaseWorkspaceName, value);
        }

        public string GetString(string variableName, string workspaceName)
        {
            string output = this.MlApplication.GetCharArray(variableName, workspaceName);
            return output;
        }

        public string GetString(string variableName)
        {
            string output = this.GetString(variableName, MatlabApplication.BaseWorkspaceName);
            return output;
        }

        public double GetDouble(string variableName, string workspaceName)
        {
            double output = this.MlApplication.GetVariable(variableName, workspaceName);
            return output;
        }

        public double GetDouble(string variableName)
        {
            double output = this.GetDouble(variableName, MatlabApplication.BaseWorkspaceName);
            return output;
        }

        public void PutRealVector(string variableName, string workspaceName, double[] values)
        {
            double[] imaginaryComplement = MatlabApplication.GetComplementZeros(values);

            this.MlApplication.PutFullMatrix(variableName, workspaceName, values, imaginaryComplement);
        }

        public void PutRealVector(string variableName, double[] values)
        {
            this.PutRealVector(variableName, MatlabApplication.BaseWorkspaceName, values);
        }

        public double[] GetRealVector(string variableName, string workspaceName)
        {
            this.MlApplication.GetWorkspaceData(variableName, workspaceName, out object vectorAsObject);

            double[,] vectorAsMatrix = vectorAsObject as double[,];
            if (null == vectorAsMatrix)
            {
                throw new InvalidOperationException(@"Attempt to get vector from MATLAB failed.");
            }

            double[] output;
            if (1 != vectorAsMatrix.GetLength(0))
            {
                if (1 != vectorAsMatrix.GetLength(1))
                {
                    throw new InvalidOperationException(@"Returned value was not a vector.");
                }
                else
                {
                    int numElements = vectorAsMatrix.GetLength(0);
                    output = new double[numElements];
                    for (int iElement = 0; iElement < numElements; iElement++)
                    {
                        output[iElement] = vectorAsMatrix[iElement, 0];
                    }
                }
            }
            else
            {
                int numElements = vectorAsMatrix.GetLength(1);
                output = new double[numElements];
                for (int iElement = 0; iElement < numElements; iElement++)
                {
                    output[iElement] = vectorAsMatrix[0, iElement];
                }
            }

            return output;
        }

        public double[] GetRealVector(string variableName)
        {
            double[] output = this.GetRealVector(variableName, MatlabApplication.BaseWorkspaceName);
            return output;
        }

        public void PutRealArray(string variableName, Array values)
        {
            this.PutRealArray(variableName, MatlabApplication.BaseWorkspaceName, values);
        }

        public void PutRealArray(string variableName, string workspaceName, Array values)
        {
            int numDimensions = values.Rank;
            switch (numDimensions)
            {
                case 1:
                case 2:
                    this.MlApplication.PutWorkspaceData(variableName, workspaceName, values);
                    break;

                case 3:
                    this.PutRealArray3D(variableName, workspaceName, values);
                    break;

                default:
                    string message = String.Format(@"Unsupported number of dimensions: {0}. Only dimensions of 1, 2, and 3 are supported.", numDimensions);
                    throw new NotImplementedException(message);
            }
        }

        private void PutRealArray3D(string variableName, string workspaceName, Array values)
        {
            // The MATLAB COM server seems to have problems with N-dimensional arrays, but 1D and 2D work fine.
            // Thus the idea here is to copy the N-dimensional array data to a 1D array, then reshape it in MATLAB.

            int numRows = values.GetLength(0);
            int numCols = values.GetLength(1);
            int numPanes = values.GetLength(2);
            Type elementType = values.GetElementType();

            Array data = Array.CreateInstance(elementType, values.Length);
            for (int iPane = 0; iPane < numPanes; iPane++)
            {
                for (int iCol = 0; iCol < numCols; iCol++)
                {
                    for (int iRow = 0; iRow < numRows; iRow++)
                    {
                        int index = iPane * (numRows * numCols) + iCol * numRows + iRow;
                        object value = values.GetValue(iRow, iCol, iPane);
                        data.SetValue(value, index);
                    }
                }
            }

            string dataTempVariableName = MatlabApplication.GetTemporaryVariableName(variableName);
            this.MlApplication.PutWorkspaceData(dataTempVariableName, workspaceName, data);

            int[] lengths = values.GetLengths();

            string tempDimensionsVariableName = MatlabApplication.GetTemporaryVariableName(@"dimensions");
            this.MlApplication.PutWorkspaceData(tempDimensionsVariableName, workspaceName, lengths);

            string command;
            command = String.Format(@"{0} = reshape({1}, {2})", variableName, dataTempVariableName, tempDimensionsVariableName);
            this.MlApplication.Execute(command);
            command = String.Format(@"clear {0}", dataTempVariableName);
            this.MlApplication.Execute(command);
            command = String.Format(@"clear {0}", tempDimensionsVariableName);
            this.MlApplication.Execute(command);
        }

        public void PutRealMatrix(string variableName, string workspaceName, double[,] values)
        {
            double[,] imaginaryComplement = MatlabApplication.GetComplementZeros(values);

            this.MlApplication.PutFullMatrix(variableName, workspaceName, values, imaginaryComplement);
        }

        public void PutRealMatrix(string variableName, double[,] values)
        {
            this.PutRealMatrix(variableName, MatlabApplication.BaseWorkspaceName, values);
        }

        public double[,] GetRealMatrix(string variableName, string workspaceName)
        {
            this.MlApplication.GetWorkspaceData(variableName, workspaceName, out object vectorAsObject);

            double[,] vectorAsMatrix = vectorAsObject as double[,];
            if (null == vectorAsMatrix)
            {
                throw new InvalidOperationException(@"Attempt to get vector from MATLAB failed.");
            }

            return vectorAsMatrix;
        }

        public double[,] GetRealMatrix(string variableName)
        {
            double[,] output = this.GetRealMatrix(variableName, MatlabApplication.BaseWorkspaceName);
            return output;
        }
    }
}


namespace Public.Common.MATLAB.Commands
{
    public static class MatlabApplicationExtensions
    {
        private static string NotFound = @"not found";


        public static void AddPath(this MatlabApplication matlabApplication, string path)
        {
            matlabApplication.AddPathToTop(path);
        }

        public static void AddPathToTop(this MatlabApplication matlabApplication, string path)
        {
            matlabApplication.Path(path);
        }

        public static string CurrentDirectory(this MatlabApplication matlabApplication)
        {
            string command = $@"{Matlab.AnswerVariableName} = cd;";
            matlabApplication.Execute(command);

            string currentDirectory = matlabApplication.GetString(Matlab.AnswerVariableName);
            return currentDirectory;
        }

        public static void CurrentDirectory(this MatlabApplication matlabApplication, string directoryPath)
        {
            string command = $@"cd('{directoryPath}');";
            matlabApplication.Execute(command);
        }

        public static void ChangeCurrentDirectory(this MatlabApplication matlabApplication, string directoryPath)
        {
            matlabApplication.CurrentDirectory(directoryPath);
        }

        public static bool IsAvailable(this MatlabApplication matlabApplication, string item)
        {
            string whichFirstOnly = matlabApplication.WhichFirstOnly(item);

            bool output = !whichFirstOnly.Contains(MatlabApplicationExtensions.NotFound);
            return output;
        }

        public static string[] Path(this MatlabApplication matlabApplication)
        {
            string command = $@"{Matlab.AnswerVariableName} = path;";
            matlabApplication.Execute(command);

            string pathConcatenated = matlabApplication.GetString(Matlab.AnswerVariableName);

            string[] output = pathConcatenated.Split(';');
            return output;
        }

        /// <summary>
        /// Adds a path to the top of the MATLAB search path.
        /// </summary>
        public static void Path(this MatlabApplication matlabApplication, string path)
        {
            string command = $@"path('{path}', path);";
            matlabApplication.Execute(command);
        }

        /// <summary>
        /// Returns the all locations at which an item can be found. The item could be a function in a MATLAB code file, a method on a loaded Java class, a workspace variable, or a file (including its extension).
        /// </summary>
        /// <remarks>
        /// Always returns at least one element.
        /// </remarks>
        public static string[] Which(this MatlabApplication matlabApplication, string item, bool all = false)
        {
            string allSuffix = String.Empty;
            if (all)
            {
                allSuffix = @", '-all'";
            }

            string command = $@"{Matlab.AnswerVariableName} = which('{item}'{allSuffix});";
            matlabApplication.Execute(command);

            object answer = matlabApplication.GetData(Matlab.AnswerVariableName);

            string[] output;
            switch (answer)
            {
                case string answerString:
                    output = new string[] { answerString };
                    break;

                case object[,] cellArray:
                    int nItems = cellArray.GetLength(0);
                    output = new string[nItems];
                    for (int iItem = 0; iItem < nItems; iItem++)
                    {
                        output[iItem] = Convert.ToString(cellArray[iItem, 0]);
                    }
                    break;

                case null:
                default:
                    output = new string[] { MatlabApplicationExtensions.NotFound };
                    break;
            }

            return output;
        }

        public static string WhichFirstOnly(this MatlabApplication matlabApplication, string item)
        {
            string[] locations = matlabApplication.Which(item);

            string output = locations[0];
            return output;
        }
    }
}


namespace Public.Common.Lib.MATLAB.Commands
{
    using Public.Common.MATLAB;
    using Public.Common.MATLAB.Commands;


    public static class MatlabApplicationExtensions
    {
        public static void AddCommonLibraryPath(this MatlabApplication matlabApplication)
        {
            string commonLibraryPath = matlabApplication.GetCommonLibraryPath();

            matlabApplication.AddPath(commonLibraryPath);
        }

        public static string GetCommonLibraryPath(this MatlabApplication matlabApplication)
        {
            string command = $@"{Matlab.AnswerVariableName} = getCommonLibraryPath;";
            matlabApplication.Execute(command);

            string output = matlabApplication.GetString(Matlab.AnswerVariableName);
            return output;
        }
    }
}