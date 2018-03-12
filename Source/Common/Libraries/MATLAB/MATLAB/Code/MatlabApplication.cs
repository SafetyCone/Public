using System;
using System.Collections.Generic;
using System.Dynamic;

using ML = MLApp;

using Public.Common.Lib.Extensions;
using Public.Common.Lib.Math;


namespace Public.Common.MATLAB
{
    /// <summary>
    /// Represents a MATLAB application.
    /// </summary>
    /// <remarks>
    /// Contains function for basic start/stop and marshalling basic data types to/from MATLAB.
    /// </remarks>
    public class MatlabApplication : IDisposable
    {
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

        private static int[] GetIntegerArray(double[,] values, int count, IEnumerable<MatrixLocation> locations)
        {
            int[] output = new int[count];

            int iIndex = 0;
            foreach (var location in locations)
            {
                output[iIndex] = Convert.ToInt32(values[location.Row, location.Column]);
                iIndex++;
            }

            return output;
        }

        public static T[] GetRowArray<T>(object obj)
        {
            T[] output;
            if (obj is T[,] tArray)
            {
                int nElements = tArray.GetLength(1);
                output = new T[nElements];
                for (int iElement = 0; iElement < nElements; iElement++)
                {
                    output[iElement] = tArray[0, iElement]; // Go across the row.
                }
            }
            else if (obj is object[,] objArray)
            {
                int nElements = objArray.GetLength(1);
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


        /// <summary>
        /// Allow access to the MLApp from within this assembly.
        /// </summary>
        internal ML.MLApp MlApplication { get; set; }
        public bool Visible
        {
            get
            {
                bool output = 1 == this.MlApplication.Visible;
                return output;
            }
            set
            {
                int visibleInt = value ? 1 : 0;
                this.MlApplication.Visible = visibleInt;
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
        /// <summary>
        /// Allow specifying that the MATLAB application should be left open by the disposal logic (this speeds startup since the application can be reused).
        /// </summary>
        public bool LeaveOpen { get; set; }


        public MatlabApplication()
            : this(true)
        {
        }

        public MatlabApplication(bool visible)
            : this(visible, false)
        {
        }

        public MatlabApplication(bool visible, bool maximized)
            : this(visible, maximized, false)
        {
        }

        public MatlabApplication(bool visible, bool maximized, bool leaveOpen)
        {
            this.MlApplication = new ML.MLApp();

            this.Visible = visible;
            this.WindowMaximized = maximized;
        }

        public string Execute(string command, bool throwOnError = true)
        {
            string output = this.MlApplication.Execute(command);
            if (null != output && String.Empty != output && output.Substring(0, Matlab.ErrorIndicator.Length) == Matlab.ErrorIndicator && throwOnError)
            {
                throw new MatlabException(output);
            }

            return output;
        }

        public List<string> Execute(IEnumerable<string> commands, bool throwOnError = true)
        {
            var output = new List<string>();
            foreach(var command in commands)
            {
                string result = this.Execute(command, throwOnError);
                output.Add(result);
            }

            return output;
        }

        #region Marshalling        

        public double GetDouble(string variableName, string workspaceName = Matlab.BaseWorkspaceName)
        {
            double output = this.GetObject<double>(variableName, workspaceName);
            return output;
        }

        public void PutDouble(string variableName, double value, string workspaceName = Matlab.BaseWorkspaceName)
        {
            this.PutObject(variableName, value, workspaceName);
        }

        public double[] GetDoubleArray(string variableName, string workspaceName = Matlab.BaseWorkspaceName)
        {
            double[] output = this.GetObject<double[]>(variableName, workspaceName);
            return output;
        }

        public void PutDoubleArray(string variableName, double[] value, string workspaceName = Matlab.BaseWorkspaceName)
        {
            this.PutObject(variableName, value, workspaceName);
        }

        public double[,] GetDoubleArray2D(string variableName, string workspaceName = Matlab.BaseWorkspaceName)
        {
            double[,] output = this.GetObject<double[,]>(variableName, workspaceName);
            return output;
        }

        public void PutDoubleArray2D(string variableName, double[,] value, string workspaceName = Matlab.BaseWorkspaceName)
        {
            this.PutObject(variableName, value, workspaceName);
        }

        public double[,,] GetDoubleArray3D(string variableName, string workspaceName = Matlab.BaseWorkspaceName)
        {
            double[,,] output = this.GetObject<double[,,]>(variableName, workspaceName);
            return output;
        }

        public void PutDoubleArray3D(string variableName, double[,,] value, string workspaceName = Matlab.BaseWorkspaceName)
        {
            this.PutRealArray3D(variableName, value, workspaceName);
        }

        public int GetInteger(string variableName, string workspaceName = Matlab.BaseWorkspaceName)
        {
            object doubleAsObject = this.GetObject(variableName, workspaceName);

            int output = Convert.ToInt32(doubleAsObject); // Use convert since it's likely the 'integer' was a double in MATLAB since double is the preferred MATLAB numeric type. This means a cast from object to integer will fail at runtime (not compiletime) since the object is in fact a double and doubles cannot be casted to integers.
            return output;
        }

        public void PutInteger(string variableName, int value, string workspaceName = Matlab.BaseWorkspaceName)
        {
            // Convert to a double since that matches the preferred MATLAB numeric data type (executing the command 'num = 1; class(num)' will output 'double'.
            double doubleValue = Convert.ToDouble(value);

            this.PutObject(variableName, doubleValue, workspaceName);
        }

        public int[] GetIntegerArrayColumn(string variableName, string workspaceName = Matlab.BaseWorkspaceName)
        {
            double[,] values = this.GetObject<double[,]>(variableName, workspaceName);

            int count = values.GetLength(0);
            var locationGenerator = new ColumnMatrixLocationGenerator(0);
            int[] output = MatlabApplication.GetIntegerArray(values, count, locationGenerator.GetLocations(count));
            return output;
        }

        public int[] GetIntegerArrayRow(string variableName, string workspaceName = Matlab.BaseWorkspaceName)
        {
            double[,] values = this.GetObject<double[,]>(variableName, workspaceName);

            int count = values.GetLength(1);
            var locationGenerator = new RowMatrixLocationGenerator(0);
            int[] output = MatlabApplication.GetIntegerArray(values, count, locationGenerator.GetLocations(count));
            return output;
        }
        
        public int[] GetIntegerArray(string variableName, string workspaceName = Matlab.BaseWorkspaceName)
        {
            int[] output = this.GetIntegerArrayColumn(variableName, workspaceName);
            return output;
        }

        public void PutIntegerArray(string variableName, int[] value, string workspaceName = Matlab.BaseWorkspaceName)
        {
            double[] doubles = value.ConvertTo<int, double>(Convert.ToDouble);

            this.PutDoubleArray(variableName, doubles, workspaceName);
        }

        public object GetObject(string variableName, string workspaceName = Matlab.BaseWorkspaceName)
        {
            this.MlApplication.GetWorkspaceData(variableName, workspaceName, out object output);

            return output;
        }

        public T GetObject<T>(string variableName, string workspaceName = Matlab.BaseWorkspaceName)
        {
            object obj = this.GetData(variableName, workspaceName);

            T output = (T)obj;
            return output;
        }

        public void PutObject(string variableName, object value, string workspaceName = Matlab.BaseWorkspaceName)
        {
            this.MlApplication.PutWorkspaceData(variableName, workspaceName, value);
        }

        public string GetString(string variableName, string workspaceName = Matlab.BaseWorkspaceName)
        {
            string output = this.MlApplication.GetCharArray(variableName, workspaceName);
            return output;
        }

        public void PutString(string variableName, string value, string workspaceName = Matlab.BaseWorkspaceName)
        {
            this.MlApplication.PutCharArray(variableName, workspaceName, value);
        }

        #endregion

        public T GetData<T>(string variableName, string workspaceName = Matlab.BaseWorkspaceName)
        {
            object obj = this.GetData(variableName, workspaceName);

            T output = (T)obj;
            return output;
        }

        public void PutData(string variableName, object data, string workspaceName = Matlab.BaseWorkspaceName)
        {
            this.MlApplication.PutWorkspaceData(variableName, workspaceName, data);
        }

        public object GetData(string variableName, string workspaceName = Matlab.BaseWorkspaceName)
        {
            this.MlApplication.GetWorkspaceData(variableName, workspaceName, out object output);

            return output;
        }

        public T[] GetColumnArray<T>(string variableName, string workspaceName = Matlab.BaseWorkspaceName)
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

        public T[] GetRowArray<T>(string variableName, string workspaceName = Matlab.BaseWorkspaceName)
        {
            object obj = this.GetData(variableName, workspaceName);

            T[] output = MatlabApplication.GetRowArray<T>(obj);
            return output;
        }

        public T[] GetArray<T>(string variableName, string workspaceName = Matlab.BaseWorkspaceName)
        {
            T[] output = this.GetColumnArray<T>(variableName, workspaceName);
            return output;
        }

        public int[] GetArrayInt(string variableName, string workspaceName = Matlab.BaseWorkspaceName)
        {
            double[] arrayAsDoubles = this.GetArray<double>(variableName, workspaceName);

            int[] output = arrayAsDoubles.ConvertToInt();
            return output;
        }

        public dynamic GetDynamic(string variableName, string workspaceName = Matlab.BaseWorkspaceName)
        {
            dynamic output = this.MlApplication.GetVariable(variableName, workspaceName);
            return output;
        }

        public string[] GetStringArray(string variableName, string workspaceName = Matlab.BaseWorkspaceName)
        {
            string[] output = this.GetArray<string>(variableName, workspaceName);
            return output;
        }

        public double[] GetRealVector(string variableName, string workspaceName = Matlab.BaseWorkspaceName)
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

        public void PutRealVector(string variableName, double[] values, string workspaceName = Matlab.BaseWorkspaceName)
        {
            double[] imaginaryComplement = MatlabApplication.GetComplementZeros(values);

            this.MlApplication.PutFullMatrix(variableName, workspaceName, values, imaginaryComplement);
        }

        public void PutRealArray(string variableName, Array values, string workspaceName = Matlab.BaseWorkspaceName)
        {
            int numDimensions = values.Rank;
            switch (numDimensions)
            {
                case 1:
                case 2:
                    this.MlApplication.PutWorkspaceData(variableName, workspaceName, values);
                    break;

                case 3:
                    this.PutRealArray3D(variableName, values, workspaceName);
                    break;

                default:
                    string message = String.Format(@"Unsupported number of dimensions: {0}. Only dimensions of 1, 2, and 3 are supported.", numDimensions);
                    throw new NotImplementedException(message);
            }
        }

        private void PutRealArray3D(string variableName, Array values, string workspaceName)
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

            string dataTempVariableName = Matlab.GetTemporaryVariableName(variableName);
            this.MlApplication.PutWorkspaceData(dataTempVariableName, workspaceName, data);

            int[] lengths = values.GetLengths();

            string tempDimensionsVariableName = Matlab.GetTemporaryVariableName(@"dimensions");
            this.MlApplication.PutWorkspaceData(tempDimensionsVariableName, workspaceName, lengths);

            string command;
            command = String.Format(@"{0} = reshape({1}, {2})", variableName, dataTempVariableName, tempDimensionsVariableName);
            this.MlApplication.Execute(command);
            command = String.Format(@"clear {0}", dataTempVariableName);
            this.MlApplication.Execute(command);
            command = String.Format(@"clear {0}", tempDimensionsVariableName);
            this.MlApplication.Execute(command);
        }

        public double[,] GetRealMatrix(string variableName, string workspaceName = Matlab.BaseWorkspaceName)
        {
            this.MlApplication.GetWorkspaceData(variableName, workspaceName, out object vectorAsObject);

            double[,] vectorAsMatrix = vectorAsObject as double[,];
            if (null == vectorAsMatrix)
            {
                throw new InvalidOperationException(@"Attempt to get vector from MATLAB failed.");
            }

            return vectorAsMatrix;
        }

        public void PutRealMatrix(string variableName, double[,] values, string workspaceName = Matlab.BaseWorkspaceName)
        {
            double[,] imaginaryComplement = MatlabApplication.GetComplementZeros(values);

            this.MlApplication.PutFullMatrix(variableName, workspaceName, values, imaginaryComplement);
        }
    }


    public static class MatlabApplicationExtensions
    {
        public static int GetScalarInt(this MatlabApplication matlabApplication, string variableName)
        {
            object intAsDoubleAsObject = matlabApplication.GetObject(variableName);

            int output = Convert.ToInt32(intAsDoubleAsObject);
            return output;
        }

        /// <summary>
        /// Marshall a structure from MATLAB to C#.
        /// </summary>
        public static ExpandoObject GetStructure(this MatlabApplication matlabApplication, string structureName)
        {
            using (Variable temp = new Variable(matlabApplication, Matlab.GetTemporaryVariableName()))
            {
                VariableInfo[] fieldInfos = MatlabApplicationExtensions.GetFieldInfos(matlabApplication, structureName);

                ExpandoObject output = new ExpandoObject();
                foreach (VariableInfo fieldInfo in fieldInfos)
                {
                    string variableName = $@"{structureName}.{fieldInfo.Name}";

                    string command = $@"{temp.Name} = {variableName}";
                    matlabApplication.Execute(command);

                    VariableInfo variableInfo = new VariableInfo(temp.Name, fieldInfo);

                    object value = MatlabApplicationExtensions.GetObjectValue(matlabApplication, variableInfo);

                    output.AddElement(fieldInfo.Name, value);
                }

                return output;
            }
        }

        /// <summary>
        /// Gets an object of a specific C# type based on it declared MATLAB type.
        /// </summary>
        private static object GetObjectValue(MatlabApplication matlabApplication, VariableInfo variableInfo)
        {
            string command;

            object value;
            switch (variableInfo.DataType)
            {
                case MatlabDataType.Cell:
                    using (Variable temp = new Variable(matlabApplication, Matlab.GetTemporaryVariableName()))
                    {
                        command = $@"{temp.Name} = {variableInfo.Name}"; // Rename the structure for recursion. Note that this will interestingly switch the name back and forth at different levels of recursion, so only two names are needed.
                        matlabApplication.Execute(command);
                        value = MatlabApplicationExtensions.GetCellArray(matlabApplication, temp.Name);
                    }
                    break;

                case MatlabDataType.Char:
                case MatlabDataType.Double:
                case MatlabDataType.Logical:
                    value = matlabApplication.GetObject(variableInfo.Name);
                    break;

                case MatlabDataType.Struct:
                case MatlabDataType.UNKNOWN: // Generally a class, with property names that can be treated just like a struct.
                    using (Variable temp = new Variable(matlabApplication, Matlab.GetTemporaryVariableName()))
                    {
                        command = $@"{temp.Name} = {variableInfo.Name}"; // Rename the structure for recursion. Note that this will interestingly switch the name back and forth at different levels of recursion, so only two names are needed.
                        matlabApplication.Execute(command);
                        value = MatlabApplicationExtensions.GetStructure(matlabApplication, temp.Name);
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

        /// <summary>
        /// Get variable informational metadata.
        /// </summary>
        public static VariableInfo GetVariableInfo(this MatlabApplication matlabApplication, string variableName)
        {
            using (Variable temp = new Variable(matlabApplication, Matlab.GetTemporaryVariableName()))
            {
                string command;

                // Get the type.
                command = $@"{temp.Name} = class({variableName})";
                matlabApplication.Execute(command);

                string fieldType = matlabApplication.GetString(temp.Name);
                MatlabDataType dataType = MatlabDataTypeExtensions.ToMatlabDataType(fieldType);

                // Get the size.
                command = $@"{temp.Name} = numel({variableName})";
                matlabApplication.Execute(command);

                int numericalLength = matlabApplication.GetScalarInt(temp.Name);

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

        /// <summary>
        /// Get information about the fields of a structure.
        /// </summary>
        public static VariableInfo[] GetFieldInfos(this MatlabApplication matlabApplication, string structureName)
        {
            using (Variable temp = new Variable(matlabApplication, Matlab.GetTemporaryVariableName()))
            {
                string command;

                // Get the field names of the structure.
                command = $@"{temp.Name} = fieldnames({structureName});";
                matlabApplication.Execute(command);
                string[] fieldNames = matlabApplication.GetStringArray(temp.Name);

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

        /// <summary>
        /// Marshall a MATLAB cell array (array where each element can be a different type) to C#.
        /// </summary>
        public static object[] GetCellArray(this MatlabApplication matlabApplication, string variableName)
        {
            using (Variable temp = new Variable(matlabApplication, Matlab.GetTemporaryVariableName()))
            {
                string command;

                VariableInfo variableInfo = matlabApplication.GetVariableInfo(variableName);

                int nElements = variableInfo.Numel();
                object[] output = new object[nElements];
                for (int iElement = 0; iElement < nElements; iElement++) // Currently only works for 1 dimensional column vectors.
                {
                    int matlabIElement = iElement + 1;
                    command = $@"{temp.Name} = {variableName}{{{matlabIElement.ToString()}}}";
                    matlabApplication.Execute(command);

                    object element = MatlabApplicationExtensions.GetObjectValue(matlabApplication, temp.Name);
                    output[iElement] = element;
                }

                return output;
            }
        }

        public static void PutCellArray(this MatlabApplication matlabApplication, string variableName, object[] elements)
        {
            MatlabApplicationExtensions.PutCellArray(matlabApplication, variableName, elements, 0);
        }

        private static void PutCellArray(MatlabApplication matlabApplication, string variableName, object[] elements, int recursionLevel)
        {
            int nElements = elements.Length;
            matlabApplication.Cell(variableName, nElements);

            using (var temp = new Variable(matlabApplication, Matlab.GetTemporaryVariableName()))
            {
                for (int iElement = 0; iElement < nElements; iElement++)
                {
                    object element = elements[iElement];
                    matlabApplication.PutObjectByTypeDispatch(temp.Name, element);

                    int iElementMatlab = iElement + 1;
                    string command = $@"{variableName}{{{iElementMatlab.ToString()}}} = {temp.Name};";
                    matlabApplication.Execute(command);
                }
            }
        }

        /// <summary>
        /// Marshall a structure from C# to MATLAB.
        /// </summary>
        public static void PutStructure(this MatlabApplication matlabApplication, string variableName, ExpandoObject expandoObject)
        {
            matlabApplication.CreateEmptryStructure(variableName); // Create an empty structure variable to nuke any pre-existing data with the same name.

            MatlabApplicationExtensions.PutStructure(matlabApplication, variableName, expandoObject, 0);
        }

        private static void PutStructure(MatlabApplication matlabApplication, string variableName, ExpandoObject expandoObject, int recursionLevel)
        {
            var expandoDict = expandoObject as IDictionary<string, object>;

            // Use a disposable temporary variable to transfer fields.
            using (var temp = new Variable(matlabApplication, Matlab.GetTemporaryVariableName()))
            {
                foreach (var fieldName in expandoDict.Keys)
                {
                    object fieldValue = expandoDict[fieldName];
                    matlabApplication.PutObjectByTypeDispatch(temp.Name, fieldValue, recursionLevel);

                    string command = $@"{variableName}.{fieldName} = {temp.Name}";
                    matlabApplication.Execute(command);
                }
            }
        }

        //public object GetObject(string variableName, string workspaceName = Matlab.BaseWorkspaceName)
        //{
        //    this.MlApplication.GetWorkspaceData(variableName, workspaceName, out object output);

        //    return output;
        //}

        //public T GetObject<T>(string variableName, string workspaceName = Matlab.BaseWorkspaceName)
        //{
        //    object obj = this.GetData(variableName, workspaceName);

        //    T output = (T)obj;
        //    return output;
        //}

        public static void PutObjectByTypeDispatch(this MatlabApplication matlabApplication, string variableName, object value, int recursionLevel = 0)
        {
            switch (value)
            {
                case string s:
                    matlabApplication.PutString(variableName, s);
                    break;

                case int i:
                    matlabApplication.PutInteger(variableName, i);
                    break;

                case int[] intArray:
                    matlabApplication.PutIntegerArray(variableName, intArray);
                    break;

                case double d:
                    matlabApplication.PutDouble(variableName, d);
                    break;

                case double[,] d2d:
                    matlabApplication.PutDoubleArray2D(variableName, d2d);
                    break;

                case double[,,] d3d:
                    matlabApplication.PutDoubleArray3D(variableName, d3d);
                    break;

                case ExpandoObject expando:
                    using (var structureVariable = new Variable(matlabApplication, Matlab.GetTemporaryVariableName()))
                    {
                        MatlabApplicationExtensions.PutStructure(matlabApplication, structureVariable.Name, expando, recursionLevel++);

                        string command = $@"{variableName} = {structureVariable.Name};";
                        matlabApplication.Execute(command);
                    }
                    break;

                case object[] cellArray:
                    using (var cellArrayVariable = new Variable(matlabApplication, Matlab.GetTemporaryVariableName()))
                    {
                        MatlabApplicationExtensions.PutCellArray(matlabApplication, cellArrayVariable.Name, cellArray, recursionLevel++);

                        string command = $@"{variableName} = {cellArrayVariable.Name};";
                        matlabApplication.Execute(command);
                    }
                    break;
                
                // bool
                default:
                    // Just try putting the object in directly, see what happens, and allow errors to propagate upwards.
                    matlabApplication.PutObject(variableName, value);
                    break;
            }
        }
    }
}