using System;
using System.Collections.Generic;
using System.IO;


namespace PythonFromNet.Lib
{
    public class FileSeriesManager
    {
        public const string DefaultBaseDirectoryPath = @"C:\temp\File Series Manager";


        #region Static

        public static string BaseDirectoryPath { get; set; }
        public static Dictionary<string, string> DirectoryPathsBySeriesID { get; private set; }


        static FileSeriesManager()
        {
            FileSeriesManager.BaseDirectoryPath = FileSeriesManager.DefaultBaseDirectoryPath;

            FileSeriesManager.DirectoryPathsBySeriesID = new Dictionary<string, string>();
        }

        /// <returns>The directory path of the series directory.</returns>
        public static string AddSeries(string seriesID, string directoryName)
        {
            string directoryPath = Path.Combine(FileSeriesManager.BaseDirectoryPath, directoryName);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            FileSeriesManager.DirectoryPathsBySeriesID.Add(seriesID, directoryPath);

            return directoryPath;
        }

        /// <returns>The directory path of the series directory.</returns>
        public static string AddSeries(string seriesID)
        {
            string output = FileSeriesManager.AddSeries(seriesID, seriesID);
            return output;
        }

        public static string GetSeriesDirectoryPath(string seriesID)
        {
            if(!FileSeriesManager.DirectoryPathsBySeriesID.ContainsKey(seriesID))
            {
                FileSeriesManager.AddSeries(seriesID);
            }

            string output = FileSeriesManager.DirectoryPathsBySeriesID[seriesID];
            return output;
        }

        #endregion
    }
}
