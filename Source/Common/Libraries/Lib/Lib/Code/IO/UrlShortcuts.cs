using System;
using System.IO;


namespace Public.Common.Lib.IO
{
    /// <summary>
    /// Allows creating URL files pointing to another file.
    /// </summary>
    public static class UrlShortcuts
    {
        public const string InternetShortcutLine = @"[InternetShortcut]";
        public const string UrlPrefix = @"URL=";
        public const string FilePrefix = @"file:///";


        public static void CreateFileShortcut(string directoryPath, string urlFileNameBase, string targetFilePath)
        {
            string urlFilePath = UrlShortcuts.CreateUrlShortCutFilePath(directoryPath, urlFileNameBase);

            using (StreamWriter writer = new StreamWriter(urlFilePath))
            {
                writer.WriteLine(UrlShortcuts.InternetShortcutLine);
                writer.WriteLine(UrlShortcuts.UrlPrefix + UrlShortcuts.FilePrefix + targetFilePath);
            }
        }

        public static void CreateFileShortcut(string urlShortcutFilePath, string targetFilePath, string iconFilePath, int iconIndex)
        {
            using (StreamWriter writer = new StreamWriter(urlShortcutFilePath))
            {
                writer.WriteLine(UrlShortcuts.InternetShortcutLine);
                writer.WriteLine(UrlShortcuts.UrlPrefix + UrlShortcuts.FilePrefix + targetFilePath);
                writer.WriteLine(@"IconFile={0}", iconFilePath);
                writer.WriteLine(@"IconIndex={0}", iconIndex);
            }
        }

        public static string CreateUrlShortCutFilePath(string directoryPath, string urlFileNameBase)
        {
            string urlFileName = String.Format(@"{0}.url", urlFileNameBase);

            string output = Path.Combine(directoryPath, urlFileName);
            return output;
        }
    }
}
