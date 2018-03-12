using System;
using System.IO;

using PathUtilities = Public.Common.Lib.IO.Paths.Utilities;


namespace Public.Common.MATLAB
{
    public static class Matlab
    {
        public const string BaseWorkspaceName = @"base";


        public static string AnswerVariableName { get; } = @"ans";
        public static string DeleteFunctionName { get; } = @"delete";
        public static string DirectoryName { get; } = Matlab.Name;
        public static string ErrorIndicator { get; } = @"???";
        public static int MaximumVariableNameLength { get; } = 64;
        public static string MFilesDirectoryName { get; } = @"M-Files";
        public static string Name { get; } = @"MATLAB";
        internal static string LoadSaveFixedStructureName { get; } = @"var3ced8cc79b284b559afc483c0e5890c5";
        internal static string LoadSaveFixedFieldName { get; } = @"value";
        public static string UserDirectoryPath
        {
            get
            {
                string output = Path.Combine(PathUtilities.MyDocumentsDirectoryPath, Matlab.DirectoryName);
                return output;
            }
        }

        // Type names.
        public static string CellTypeName { get; } = @"cell";
        public static string CharTypeName { get; } = @"char";
        public static string DoubleTypeName { get; } = @"double";
        public static string LogicalTypeName { get; } = @"logical";
        public static string StructTypeName { get; } = @"struct";
        public static string UnknownTypeName { get; } = @"UNKNOWN";


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
            if (null == Matlab.zInstance)
            {
                Matlab.zInstance = new MatlabApplication();
            }
        }

        public static void Stop()
        {
            if (null != Matlab.zInstance)
            {
                Matlab.zInstance.Dispose();

                Matlab.zInstance = null;
            }
        }

        public static string GetCreateEmptyStructureCommand(string name)
        {
            string command = $@"{name} = struct;";
            return command;
        }

        public static string GetTemporaryVariableName()
        {
            string output = @"var" + Guid.NewGuid().ToString().Replace(@"-", String.Empty); // OK, less than the max variable name length of MATLAB. Make sure to start variable names with a letter.
            return output;
        }

        public static string GetTemporaryVariableName(string baseVariableName)
        {
            string suffix = Matlab.GetTemporaryVariableName();

            string combination = baseVariableName + suffix;
            string output;
            if (Matlab.MaximumVariableNameLength > combination.Length)
            {
                output = combination;
            }
            else
            {
                output = combination.Substring(0, Matlab.MaximumVariableNameLength - 1);
            }

            return output;
        }
    }
}
