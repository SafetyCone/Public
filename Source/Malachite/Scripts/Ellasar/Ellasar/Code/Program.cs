using System;
using System.IO;

using IoUtilities = Public.Common.Lib.IO.Utilities;
using ImageFileExtensions = Public.Common.Lib.Visuals.IO.FileExtensions;

using Ellasar.Lib;
using EllasarFileExtensions = Ellasar.Lib.FileExtensions;


namespace Ellasar
{
    class Program
    {
        static void Main(string[] args)
        {
            //Construction.SubMain();
            Program.SubMain();
        }

        private static void SubMain()
        {
            //Program.Reset();
            Program.PerformSfM();
        }

        private static void Reset()
        {
            string imagesDirectoryPath = @"E:\Organizations\Minex\Data\Images\Princeton\Campus\Small-Few";
            string modelName = @"Tower1";

            Reconstruction.ResetWorkDirectory(imagesDirectoryPath, modelName);
        }

        private static void PerformSfM()
        {
            string imagesDirectoryPath = @"E:\Organizations\Minex\Data\Images\Princeton\Campus\Small-Few";

            // Create the images list file.
            string imagesListFileName = @"Images List.txt";
            string imageFileListFilePath = Path.Combine(imagesDirectoryPath, imagesListFileName);
            Program.CreateImageFileListFile(imageFileListFilePath, imagesDirectoryPath);

            string workDirectoryName = @"Ellasar";
            string workDirectoryPath = Path.Combine(imagesDirectoryPath, workDirectoryName);

            string outputSparseNvmFileName = @"Model-Sparse.nvm";
            string outputSparseNvmFilePath = Path.Combine(imagesDirectoryPath, outputSparseNvmFileName);

            string outputDensePlyFileName = @"Model.ply";
            string outputDensePlyFilePath = Path.Combine(imagesDirectoryPath, outputDensePlyFileName);

            string outputDensePatchFileName = @"Model.patch";
            string outputDensePatchFilePath = Path.Combine(imagesDirectoryPath, outputDensePatchFileName);

            string outputMeshFileName = @"Mesh.ply";
            string outputMeshFilePath = Path.Combine(imagesDirectoryPath, outputMeshFileName);

            Reconstruction.Run(imageFileListFilePath, outputSparseNvmFilePath, outputDensePlyFilePath, outputDensePatchFilePath, outputMeshFilePath, workDirectoryPath);
        }

        private static void CreateImageFileListFile(string imageFileListFilePath, string imageDirectoryPath)
        {
            var imageFileExtensions = new string[] { ImageFileExtensions.JpgFileExtension };
            var imageFilePaths = IoUtilities.FilePathsByExtensions(imageDirectoryPath, imageFileExtensions);

            File.WriteAllLines(imageFileListFilePath, imageFilePaths);
        }
    }
}
