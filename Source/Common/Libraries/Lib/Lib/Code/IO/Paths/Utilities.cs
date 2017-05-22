using System;


namespace Public.Common.Lib.IO.Paths
{
    public static class Utilities
    {
        public static string MyDocumentsDirectoryPath
        {
            get
            {
                string output = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                return output;
            }
        }
    }
}
