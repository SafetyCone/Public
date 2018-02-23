using System;
using System.Collections.Generic;
using System.IO;
using System.Management;

using Public.Common.Lib.IO;
using IoUtilities = Public.Common.Lib.IO.Utilities;
using Public.Common.Lib.Organizational;
using Public.Common.Lib.OS;

using Public.Malachite.Lib.Organizational;

using Ellasar.Lib;
using EllasarProperties = Ellasar.Lib.Properties;


namespace Ellasar
{
    public static class Construction
    {
        public static void SubMain()
        {
            //Construction.Scratch();
        }

        //// use `/ 1048576` to get ram in MB
        //// and `/ (1048576 * 1024)` or `/ 1048576 / 1024` to get ram in GB
        //// https://stackoverflow.com/questions/105031/how-do-you-get-total-amount-of-ram-the-computer-has
        //// Requires adding a reference to System.Management.
        //private static string GetRAMsize()
        //{
        //    //Console.WriteLine(BytesToMb(Convert.ToInt64(ManagementQuery("SELECT TotalPhysicalMemory FROM Win32_ComputerSystem", "TotalPhysicalMemory", "root\\CIMV2"))));

        //    ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
        //    ManagementObjectCollection moc = mc.GetInstances();
        //    string output = @"RAMsize";
        //    foreach (ManagementObject item in moc)
        //    {
        //        output = Convert.ToString(Math.Round(Convert.ToDouble(item.Properties["TotalPhysicalMemory"].Value) / 1048576, 0)) + " MB";
        //    }

        //    return output;
        //}

        private static string GetInputPlyFilePathFromNvmFilePath(string nvmFilePath)
        {
            string directoryPath = Path.GetDirectoryName(nvmFilePath);

            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(nvmFilePath);

            string plyFileName = $@"{fileNameWithoutExtension}.0.ply";
            string plyFilePath = Path.Combine(directoryPath, plyFileName);
            return plyFilePath;
        }

        private static string RunSparse(IDictionary<string, string> properties, string imagesDirectoryPath, string modelName)
        {
            string visualSfmExecutableFilePath = properties[EllasarProperties.VisualSfMExecutableFilePathPropertyName];

            string imageSourcePath = imagesDirectoryPath; // Currently only a directory seems to work. Use an image file list file should be figured out.

            string outputNvmFilePath = Construction.GetOutputSparseNvmFilePath(imagesDirectoryPath, modelName);

            string arguments = $@"sfm {imageSourcePath} {outputNvmFilePath}";

            ProcessStarter.RunProcess(visualSfmExecutableFilePath, arguments);

            return outputNvmFilePath;
        }

        private static string GetOutputSparseNvmFilePath(string imagesDirectoryPath, string modelName)
        {
            string outputNvmFileName = $@"{modelName}-Sparse.nvm";
            string outputNvmFilePath = Path.Combine(imagesDirectoryPath, outputNvmFileName);
            return outputNvmFilePath;
        }

        private static string GetOutputDenseNvmFilePath(string imagesDirectoryPath, string modelName)
        {
            string outputNvmFileName = $@"{modelName}-Dense.nvm";
            string outputNvmFilePath = Path.Combine(imagesDirectoryPath, outputNvmFileName);
            return outputNvmFilePath;
        }

        private static string RunSparseAndDense(IDictionary<string, string> properties, string imagesDirectoryPath, string modelName)
        {
            string visualSfmExecutableFilePath = properties[EllasarProperties.VisualSfMExecutableFilePathPropertyName];

            string imageSourcePath = imagesDirectoryPath; // Currently only a directory seems to work. Use an image file list file should be figured out.

            string outputNvmFileName = $@"{modelName}.nvm";
            string outputNvmFilePath = Path.Combine(imagesDirectoryPath, outputNvmFileName);

            string arguments = $@"sfm+pmvs {imageSourcePath} {outputNvmFilePath}";

            ProcessStarter.RunProcess(visualSfmExecutableFilePath, arguments);

            return outputNvmFilePath;
        }

        private static void RunSparseAndDense()
        {
            // Get the VisualSfM directory path.
            var properties = Configuration.GetProjectProperties(PublicRepository.RepositoryDirectoryName, MalachiteDomain.DomainDirectoryName, Constants.ProjectDirectoryName);

            string visualSfmExecutableFilePath = properties[EllasarProperties.VisualSfMExecutableFilePathPropertyName];
            string imagesDirectoryPath = @"E:\Organizations\Minex\Data\Images\Princeton\Campus\Small\VisualSfM";
            //string imageFileListFilePath = @"C:\temp\ImageList.txt";
            string modelName = @"Tower1";
            string modelFileName = modelName;
            string outputNvmFilePath = $@"E:\Organizations\Minex\Data\Images\Princeton\Campus\Small\VisualSfM\{modelFileName}.nvm";

            string cd = Environment.CurrentDirectory;

            string imageSourceFilePath = imagesDirectoryPath;
            //string imageSourceFilePath = imageFileListFilePath;
            string arguments = $@"sfm+pmvs {imageSourceFilePath} {outputNvmFilePath}";

            //ProcessStarter.StartProcess(visualSfmExecutablePath, arguments);

            //ProcessStartInfo info = new ProcessStartInfo(visualSfmExecutablePath, arguments);
            //info.UseShellExecute = true;
            ////info.RedirectStandardError = true;
            ////info.
            //Process.Start(info);

            ProcessStarter.RunProcess(visualSfmExecutableFilePath, arguments);
        }

        private static void RunMeshLabServer()
        {
            string meshlabServerExecutableFilePath = @"C:\Program Files\VCG\MeshLab\meshlabserver.exe";
            string inputPlyFilePath = @"E:\Organizations\Minex\Data\Images\Princeton\Campus\Small\VisualSfM\Tower1.0.ply";
            string outputFilePathPath = @"C:\temp\temp.ply";
            string scriptFilePath = @"C:\temp\BallPivoting.mlx";

            string arguments = $@"-i {inputPlyFilePath} -o {outputFilePathPath} -m vc -s {scriptFilePath}";

            ProcessStarter.RunProcess(meshlabServerExecutableFilePath, arguments);
        }

        private static void Scratch()
        {
            //int systemPageSize = Environment.SystemPageSize;
            //long workingSet = Environment.WorkingSet;
            //string ramSize = Program.GetRAMsize();
            //int processorCount = Environment.ProcessorCount;

            //Program.GetSfmExecutableDirectoryPath();
            //Program.ResetDirectory();
            //Program.RunSparseAndDense();
            //Program.RunMeshLabServer();

            //string imageFileListFilePath = @"C:\temp\ImageList.txt";
            //string imagesDirectoryPath = @"E:\Organizations\Minex\Data\Images\Princeton\Campus\Small\VisualSfM";
            //Program.CreateImageFileListFile(imagesDirectoryPath, imageFileListFilePath);

            //string imagesDirectoryPath = @"E:\Organizations\Minex\Data\Images\Princeton\Campus\Small\VisualSfM2";
            //string modelName = @"Tower1";

            //Program.ResetDirectory(imagesDirectoryPath, modelName);
            ////Program.SfmToMesh(imagesDirectoryPath, modelName);
            //Program.RunEachStepBad(imagesDirectoryPath, modelName);
        }
    }
}
