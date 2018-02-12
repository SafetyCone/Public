using System.IO;
using System.Reflection;


namespace Public.Common.MATLAB
{
    public static class Utilities
    {
        public static string LibraryDirectoryPath
        {
            get
            {
                string assemblyFilePath = Assembly.GetExecutingAssembly().Location;

                string output = Path.GetDirectoryName(assemblyFilePath);
                return output;
            }
        }
    }
}
