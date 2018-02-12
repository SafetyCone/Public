using System;

using ML = MLApp;

using Public.Common.Lib;
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

        public static T[] GetRowArray<T>(object obj)
        {
            T[] output;
            if (obj is T[,] tArray)
            {
                int nElements = tArray.GetLength(0);
                output = new T[nElements];
                for (int iElement = 0; iElement < nElements; iElement++)
                {
                    output[iElement] = tArray[0, iElement]; // Go across the row.
                }
            }
            else if (obj is object[,] objArray)
            {
                int nElements = objArray.GetLength(0);
                output = new T[nElements];
                for (int iElement = 0; iElement < nElements; iElement++)
                {
                    output[iElement] = (T)objArray[0, iElement]; // Go down the column.
                }
            }
            else
            {
                output = null;
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
                if (!this.LeaveOpen)
                {
                    this.MlApplication.Quit();
                }
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
        public bool LeaveOpen { get; set; }


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

        public object GetObject(string variableName, string workspaceName = MatlabApplication.BaseWorkspaceName)
        {
            this.MlApplication.GetWorkspaceData(variableName, workspaceName, out object output);

            return output;
        }

        public T GetData<T>(string variableName, string workspaceName = MatlabApplication.BaseWorkspaceName)
        {
            object obj = this.GetData(variableName, workspaceName);

            T output = (T)obj;
            return output;
        }

        public T GetScalar<T>(string variableName, string workspaceName = MatlabApplication.BaseWorkspaceName)
        {
            object obj = this.GetData(variableName, workspaceName);

            T output = (T)obj;
            return output;
        }

        public int[] GetColumnArrayInt(string variableName, string workspaceName = MatlabApplication.BaseWorkspaceName)
        {
            object obj = this.GetData(variableName, workspaceName);

            int[] output;
            if (obj is double[,] doubleArray)
            {
                int nElements = doubleArray.GetLength(0);
                output = new int[nElements];
                for (int iElement = 0; iElement < nElements; iElement++)
                {
                    output[iElement] = Convert.ToInt32(doubleArray[iElement, 0]); // Go down the column.
                }
            }
            else
            {
                output = null;
            }

            return output;
        }

        public int[] GetRowArrayInt(string variableName, string workspaceName = MatlabApplication.BaseWorkspaceName)
        {
            object obj = this.GetData(variableName, workspaceName);

            int[] output;
            if (obj is double[,] doubleArray)
            {
                int nElements = doubleArray.GetLength(1);
                output = new int[nElements];
                for (int iElement = 0; iElement < nElements; iElement++)
                {
                    output[iElement] = Convert.ToInt32(doubleArray[0, iElement]); // Go across the row.
                }
            }
            else
            {
                output = null;
            }

            return output;
        }

        public T[] GetColumnArray<T>(string variableName, string workspaceName = MatlabApplication.BaseWorkspaceName)
        {
            object obj = this.GetData(variableName, workspaceName);

            T[] output;
            if (obj is T[,] tArray)
            {
                int nElements = tArray.GetLength(0);
                output = new T[nElements];
                for (int iElement = 0; iElement < nElements; iElement++)
                {
                    output[iElement] = tArray[iElement, 0]; // Go down the column.
                }
            }
            else if (obj is object[,] objArray)
            {
                int nElements = objArray.GetLength(0);
                output = new T[nElements];
                for (int iElement = 0; iElement < nElements; iElement++)
                {
                    output[iElement] = (T)objArray[iElement, 0]; // Go down the column.
                }
            }
            else
            {
                output = null;
            }

            return output;
        }

        public T[] GetRowArray<T>(string variableName, string workspaceName = MatlabApplication.BaseWorkspaceName)
        {
            object obj = this.GetData(variableName, workspaceName);

            T[] output = MatlabApplication.GetRowArray<T>(obj);
            return output;
        }

        public T[] GetArray<T>(string variableName, string workspaceName = MatlabApplication.BaseWorkspaceName)
        {
            T[] output = this.GetColumnArray<T>(variableName, workspaceName);
            return output;
        }

        public int[] GetArrayInt(string variableName, string workspaceName = MatlabApplication.BaseWorkspaceName)
        {
            double[] arrayAsDoubles = this.GetArray<double>(variableName, workspaceName);

            int[] output = arrayAsDoubles.ConvertToInt();
            return output;
        }

        public dynamic GetDynamic(string variableName, string workspaceName = MatlabApplication.BaseWorkspaceName)
        {
            dynamic output = this.MlApplication.GetVariable(variableName, workspaceName);
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

        public string[] GetStringArray(string variableName, string workspaceName = MatlabApplication.BaseWorkspaceName)
        {
            string[] output = this.GetArray<string>(variableName, workspaceName);
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
    using System.Dynamic;


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

        public static void ChangeCurrentDirectory(this MatlabApplication matlabApplication, string directoryPath)
        {
            matlabApplication.CurrentDirectory(directoryPath);
        }

        public static void Clc(this MatlabApplication matlabApplication)
        {
            string command = @"clc;";
            matlabApplication.Execute(command);
        }

        public static void Clear(this MatlabApplication matlabApplication, string variableName)
        {
            string command = $@"clear {variableName}";
            matlabApplication.Execute(command);
        }

        public static void Clear(this MatlabApplication matlabApplication)
        {
            string command = @"clear;";
            matlabApplication.Execute(command);
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

        public static ExpandoObject GetStructure(this MatlabApplication matlabApplication, string structureName)
        {
            string tempVariableName = $@"{nameof(GetStructure)}TempVariable";
            using (Variable var = new Variable(matlabApplication, tempVariableName))
            {
                VariableInfo[] fieldInfos = MatlabApplicationExtensions.GetFieldInfos(matlabApplication, structureName);

                ExpandoObject output = new ExpandoObject();
                foreach (VariableInfo fieldInfo in fieldInfos)
                {
                    string variableName = $@"{structureName}.{fieldInfo.Name}";

                    string command = $@"{tempVariableName} = {variableName}";
                    matlabApplication.Execute(command);

                    VariableInfo variableInfo = new VariableInfo(tempVariableName, fieldInfo);

                    object value = MatlabApplicationExtensions.GetObjectValue(matlabApplication, variableInfo);

                    output.AddElement(fieldInfo.Name, value);
                }

                return output;
            }
        }

        private static object GetObjectValue(MatlabApplication matlabApplication, VariableInfo variableInfo)
        {
            string command;

            object value;
            switch (variableInfo.DataType)
            {
                case MatlabDataType.Cell:
                    command = $@"tempCellName = {variableInfo.Name}"; // Rename the structure for recursion. Note that this will interestingly switch the name back and forth at different levels of recursion, so only two names are needed.
                    using (Variable var = new Variable(matlabApplication, @"tempCellName", command))
                    {
                        value = MatlabApplicationExtensions.GetCellArray(matlabApplication, @"tempCellName");
                    }
                    break;

                case MatlabDataType.Char:
                case MatlabDataType.Double:
                case MatlabDataType.Logical:
                    value = matlabApplication.GetObject(variableInfo.Name);
                    break;

                case MatlabDataType.Struct:
                case MatlabDataType.UNKNOWN: // Generally a class, with property names that can be treated just like a struct.
                    command = $@"tempStructName = {variableInfo.Name}"; // Rename the structure for recursion. Note that this will interestingly switch the name back and forth at different levels of recursion, so only two names are needed.
                    using (Variable var = new Variable(matlabApplication, @"tempStructName", command))
                    {
                        value = MatlabApplicationExtensions.GetStructure(matlabApplication, @"tempStructName");
                    }
                    break;

                default:
                    value = null;
                    break;
            }

            return value;
        }

        private static object GetObjectValue(MatlabApplication matlabApplication, string variableName)
        {
            VariableInfo variableInfo = matlabApplication.GetVariableInfo(variableName);

            object output = MatlabApplicationExtensions.GetObjectValue(matlabApplication, variableInfo);
            return output;
        }

        public static VariableInfo GetVariableInfo(this MatlabApplication matlabApplication, string variableName)
        {
            string tempVariableName = $@"{nameof(GetVariableInfo)}TempVariable";
            using (Variable var = new Variable(matlabApplication, tempVariableName))
            {
                string command;

                // Get the type.
                command = $@"{tempVariableName} = class({variableName})";
                matlabApplication.Execute(command);

                string fieldType = matlabApplication.GetString(tempVariableName);
                MatlabDataType dataType = MatlabDataTypeExtensions.ToMatlabDataType(fieldType);

                // Get the size.
                command = $@"{tempVariableName} = numel({variableName})";
                matlabApplication.Execute(command);

                int numericalLength = matlabApplication.GetScalarInt(tempVariableName);

                bool isScalar;
                if (dataType != MatlabDataType.Char)
                {
                    isScalar = 1 == numericalLength;
                }
                else
                {
                    isScalar = true;
                }

                int[] size = null;
                if (!isScalar && dataType != MatlabDataType.Char) // Because a string is a char array.
                {
                    size = matlabApplication.Size(variableName); // Should be an array of ints.
                }

                VariableInfo output = new VariableInfo(variableName, dataType, isScalar, size);
                return output;
            }
        }

        private static VariableInfo[] GetFieldInfos(MatlabApplication matlabApplication, string structureName)
        {
            string tempVariableName = $@"{nameof(GetFieldInfos)}TempVariable";
            using (Variable var = new Variable(matlabApplication, tempVariableName))
            {
                string command;

                // Get the field names of the structure.
                command = $@"{tempVariableName} = fieldnames({structureName});";
                matlabApplication.Execute(command);
                string[] fieldNames = matlabApplication.GetStringArray(tempVariableName);

                int nFields = fieldNames.Length;
                VariableInfo[] fieldInfos = new VariableInfo[nFields];
                for (int iField = 0; iField < nFields; iField++)
                {
                    // Get the name.
                    string fieldName = fieldNames[iField];
                    string variableName = $@"{structureName}.{fieldName}";

                    VariableInfo variableInfo = matlabApplication.GetVariableInfo(variableName);
                    VariableInfo fieldInfo = new VariableInfo(fieldName, variableInfo);
                    fieldInfos[iField] = fieldInfo;
                }

                return fieldInfos;
            }
        }

        public static object GetCellArray(MatlabApplication matlabApplication, string variableName)
        {
            string tempVariableName = $@"{nameof(GetCellArray)}TempVariable";
            using (Variable var = new Variable(matlabApplication, tempVariableName))
            {
                string command;

                VariableInfo variableInfo = matlabApplication.GetVariableInfo(variableName);

                int nElements = variableInfo.Numel();
                object[] output = new object[nElements];
                for (int iElement = 0; iElement < nElements; iElement++) // Currently only works for 1 dimensional column vectors.
                {
                    int matlabIElement = iElement + 1;
                    command = $@"{tempVariableName} = {variableName}{{{matlabIElement.ToString()}}}";
                    matlabApplication.Execute(command);

                    object element = MatlabApplicationExtensions.GetObjectValue(matlabApplication, tempVariableName);
                    output[iElement] = element;
                }

                return output;
            }
        }

        public static int GetScalarInt(this MatlabApplication matlabApplication, string variableName)
        {
            object intAsDoubleAsObject = matlabApplication.GetObject(variableName);

            int output = Convert.ToInt32(intAsDoubleAsObject);
            return output;
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

        public static int[] Size(this MatlabApplication matlabApplication, string variableName)
        {
            string tempVariableName = @"SizeTempVariable";
            string creationCommand = $@"{tempVariableName} = size({variableName});";
            int[] output;
            using (Variable var = new Variable(matlabApplication, tempVariableName, creationCommand))
            {
                output = matlabApplication.GetRowArrayInt(tempVariableName);
            }

            return output;
        }

        public static void Startup(this MatlabApplication matlabApplication)
        {
            matlabApplication.Clear();
            matlabApplication.Clc();
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
    using System.IO;

    using Public.Common.MATLAB;
    using Public.Common.MATLAB.Commands;


    public static class MatlabApplicationExtensions
    {
        private static string PublicCommonDirectoryName { get; } = @"Public.Common";


        public static void AddPublicCommonLibraryPath(this MatlabApplication matlabApplication)
        {
            string commonLibraryPath = matlabApplication.GetPublicCommonLibraryPath();

            matlabApplication.AddPath(commonLibraryPath);
        }

        public static string GetPublicCommonLibraryPath(this MatlabApplication matlabApplication)
        {
            string libraryLocation = Utilities.LibraryDirectoryPath;

            string output = Path.Combine(libraryLocation, Matlab.MFilesDirectoryName, MatlabApplicationExtensions.PublicCommonDirectoryName);
            return output;
        }

        public static string[] FilePathsByExtension(this MatlabApplication matlabApplication, string directoryPath, string[] fileExtensions)
        {
            string[] output;

            using (var directoryPathVar = new Variable(matlabApplication, @"directoryPath", directoryPath))
            using (var fileExtensionsVar = new Variable(matlabApplication, @"fileExtensions", fileExtensions))
            {
                string command = $@"{Matlab.AnswerVariableName} = filePathsByExtensions(directoryPath, fileExtensions);";
                matlabApplication.Execute(command);

                output = matlabApplication.GetStringArray(Matlab.AnswerVariableName);   
            }

            return output;
        }
    }
}