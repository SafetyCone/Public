using System.IO;

using PathUtilities = Public.Common.Lib.IO.Paths.Utilities;


namespace Public.Common.MATLAB
{
    public static class Matlab
    {
        public static string AnswerVariableName { get; } = @"ans";
        public static string Name { get; } = @"MATLAB";
        public static string DirectoryName { get; } = Matlab.Name;
        public static string MFilesDirectoryName { get; } = @"M-Files";
        public static string UserDirectoryPath
        {
            get
            {
                string output = Path.Combine(PathUtilities.MyDocumentsDirectoryPath, Matlab.DirectoryName);
                return output;
            }
        }
        public static string CellTypeName { get; } = @"cell";
        public static string CharTypeName { get; } = @"char";
        public static string DoubleTypeName { get; } = @"double";
        public static string LogicalTypeName { get; } = @"logical";
        public static string StructTypeName { get; } = @"struct";
        public static string UnknownTypeName { get; } = @"UNKNOWN";
    }
}
