using System;
using System.IO;
using System.Reflection;


namespace Public.Common.Lib.IO.Paths
{
    public static class Utilities
    {
        /// <summary>
        /// Allow changing the default directory path used for all caches.
        /// </summary>
        /// <remarks>
        /// NOT thread-safe.
        /// </remarks>
        public static string DefaultCachesDirectoryPath { get; set; } = @"E:\temp\Caches";


        public static string MyDocumentsDirectoryPath
        {
            get
            {
                var output = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                return output;
            }
        }

        /// <summary>
        /// Gets the path location of the executable file as specified by the first command line argument.
        /// </summary>
        /// <remarks>
        /// The executable file when debugging in Visual Studio is .exe.vshost, not just .exe.
        /// </remarks>
        public static string ExecutableLocationCommandLineArgument
        {
            get
            {
                var output = Environment.GetCommandLineArgs()[0];
                return output;
            }
        }
        /// <summary>
        /// Gets the path location of the executable file as specified by the entry assembly's location.
        /// </summary>
        public static string ExecutableLocationEntryAssembly
        {
            get
            {
                var output = Assembly.GetEntryAssembly().Location;
                return output;
            }
        }
        /// <summary>
        /// Gets the rooted path of the executable via the default route.
        /// </summary>
        /// <remarks>
        /// There are multiple ways to get the location of the executable, and depending on context (unit test, debugging in Visual Studio, or production) different locations are returned.
        /// I chose the command line argument as the default since this is the way the program is actually run by the operating system.
        /// </remarks>
        public static string ExecutableRootedPath
        {
            get
            {
                var output = Utilities.ExecutableLocationCommandLineArgument;
                return output;
            }
        }
        /// <summary>
        /// Gets the directory location of the executable via the default executable rooted path.
        /// </summary>
        public static string ExecutableDirectoryPath
        {
            get
            {
                var executableFileRootedPath = Utilities.ExecutableRootedPath;

                var output = Path.GetDirectoryName(executableFileRootedPath);
                return output;
            }
        }
        /// <summary>
        /// Gets the directory location of the executable via the default executable rooted path.
        /// </summary>
        public static DirectoryInfo ExecutableDirectory
        {
            get
            {
                var executableDirectoryPath = Utilities.ExecutableDirectoryPath;

                var output = new DirectoryInfo(executableDirectoryPath);
                return output;
            }
        }
    }
}
