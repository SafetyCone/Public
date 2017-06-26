using System;
using System.IO;


namespace PythonFromNet.Lib
{
    public class MiniBatchFilePathManager
    {
        public const string DefaultBaseDirectory = @"C:\temp\Mini-Batches";
        public const string DefaultEpochFileMask = @"Epoch {0}.dat";
        public const int DefaultCurrentEpochNumber = 0;


        #region Static

        public static string CreateEpochFilePath(string baseDirectoryPath, string epochFileMask, int currentEpochNumber)
        {
            string fileName = String.Format(epochFileMask, currentEpochNumber);

            string output = Path.Combine(baseDirectoryPath, fileName);
            return output;
        }

        #endregion


        public string BaseDirectoryPath { get; set; }
        public string EpochFileMask { get; set; }
        public int CurrentEpochNumber { get; set; }


        public MiniBatchFilePathManager(string baseDirectoryPath, string epochFileMask, int currentEpochNumber)
        {
            this.BaseDirectoryPath = baseDirectoryPath;
            if(!Directory.Exists(this.BaseDirectoryPath))
            {
                Directory.CreateDirectory(this.BaseDirectoryPath);
            }

            this.EpochFileMask = epochFileMask;
            this.CurrentEpochNumber = currentEpochNumber;
        }

        public MiniBatchFilePathManager(string baseDirectoryPath)
            : this(baseDirectoryPath, MiniBatchFilePathManager.DefaultEpochFileMask, MiniBatchFilePathManager.DefaultCurrentEpochNumber)
        {
        }

        public MiniBatchFilePathManager()
            : this(MiniBatchFilePathManager.DefaultBaseDirectory)
        {
        }

        public string GetEpochFilePath()
        {
            string output = MiniBatchFilePathManager.CreateEpochFilePath(this.BaseDirectoryPath, this.EpochFileMask, this.CurrentEpochNumber);
            return output;
        }

        public string GetNextEpochFilePath()
        {
            string output = this.GetEpochFilePath();

            this.CurrentEpochNumber++;

            return output;
        }

        public string GetEpochFilePath(int epochNumber)
        {
            string output = MiniBatchFilePathManager.CreateEpochFilePath(this.BaseDirectoryPath, this.EpochFileMask, epochNumber);
            return output;
        }
    }
}
