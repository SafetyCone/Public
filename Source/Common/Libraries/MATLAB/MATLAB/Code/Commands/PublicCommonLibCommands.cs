using System.IO;


using Public.Common.MATLAB;
using MatlabUtilities = Public.Common.MATLAB.Utilities;


namespace Public.Common.Lib.MATLAB
{
    /// <summary>
    /// Contains functionality for commands that are part of the Public.Common library of MATLAB commands.
    /// </summary>
    public static class PublicCommonLibCommands
    {
        private static string PublicCommonDirectoryName { get; } = @"Public.Common";


        public static void AddPublicCommonLibraryPath(this MatlabApplication matlabApplication)
        {
            string commonLibraryPath = matlabApplication.GetPublicCommonLibraryPath();

            matlabApplication.AddPath(commonLibraryPath);
        }

        public static string GetPublicCommonLibraryPath(this MatlabApplication matlabApplication)
        {
            string libraryLocation = MatlabUtilities.LibraryDirectoryPath;

            string output = Path.Combine(libraryLocation, Matlab.MFilesDirectoryName, PublicCommonLibCommands.PublicCommonDirectoryName);
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
