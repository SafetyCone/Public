using System;
using System.Collections.Generic;
using System.IO;

using Public.Common.Lib.IO;
using IoUtilities = Public.Common.Lib.IO.Utilities;
using Public.Common.Lib.Organizational;
using Public.Common.Lib.OS;

using Public.Malachite.Lib.Organizational;

using EllasarFileExtensions = Ellasar.Lib.FileExtensions;


namespace Ellasar.Lib
{
    public class Reconstruction
    {
        #region Static

        public static readonly string DefaultModelName = @"Model1";


        public static void ResetWorkDirectory(string workDirectoryPath)
        {
            Reconstruction.ResetWorkDirectory(workDirectoryPath, Reconstruction.DefaultModelName);
        }

        public static void ResetWorkDirectory(string workDirectoryPath, string modelName)
        {
            // Delete .sift and .mat files.
            var fileExtensions = new string[] { EllasarFileExtensions.MatchFileExtension, EllasarFileExtensions.SiftFileExtension };
            IoUtilities.DeleteFilePathsByExtensions(workDirectoryPath, fileExtensions);

            // Delete "{Model Name}*} files.
            var prefixes = new string[] { modelName };
            IoUtilities.DeleteFilePathsByPrefixes(workDirectoryPath, prefixes);

            // Delete "{Model Name}*" directories.
            IoUtilities.DeleteSubDirectoryPathsByPrefixes(workDirectoryPath, prefixes);
        }

        public static void Run(string imageFileListFilePath, string outputSparseNvmFilePath, string outputDensePlyFilePath, string outputDensePatchFilePath, string outputMeshFilePath, string workDirectoryPath)
        {
            string[] imageFilePaths = File.ReadAllLines(imageFileListFilePath);

            Reconstruction.Run(imageFilePaths, outputSparseNvmFilePath, outputDensePlyFilePath, outputDensePatchFilePath, outputMeshFilePath, workDirectoryPath);
        }

        public static void Run(IEnumerable<string> imageFilePaths, string outputSparseNvmFilePath, string outputDensePlyFilePath, string outputDensePatchFilePath, string outputMeshFilePath, string workDirectoryPath)
        {
            // Create the temporary directory in which all scripting will occur.
            Directory.CreateDirectory(workDirectoryPath);

            // Copy each file in the image file path list to the temporary directory.
            foreach(var imageFilePath in imageFilePaths)
            {
                string destinationFilePath = IoUtilities.GetFilePathIfCopiedToDirectory(imageFilePath, workDirectoryPath);
                if(!File.Exists(destinationFilePath)) // Only recopy images if 
                {
                    File.Copy(imageFilePath, destinationFilePath);
                }
            }

            // Run the SfM process by scripting Visual SfM, PMVS2, and MeshLab.
            SfmOutputFilePaths outputFilePaths = Reconstruction.PerformSfM(workDirectoryPath, Reconstruction.DefaultModelName);

            // Copy the output files from where the scripting process placed them, and where they are desired.
            File.Copy(outputFilePaths.SparseNvmFilePath, outputSparseNvmFilePath, true);
            File.Copy(outputFilePaths.DenseModelPlyFilePath, outputDensePlyFilePath, true);
            File.Copy(outputFilePaths.DenseModelPatchFilePath, outputDensePatchFilePath, true);
            File.Copy(outputFilePaths.MeshFilePath, outputMeshFilePath, true);
        }

        public static SfmOutputFilePaths PerformSfM(string imagesDirectoryPath, string modelName)
        {
            var properties = Configuration.GetProjectProperties(PublicRepository.RepositoryDirectoryName, MalachiteDomain.DomainDirectoryName, Constants.ProjectDirectoryName);

            // Run sparse reconstruction and CMVS.
            string sparseNvmFilePath = Reconstruction.RunSparseAndCmvs(properties, imagesDirectoryPath, modelName);

            // Run PMVS.
            string modelDirectoryPath = Reconstruction.GetModelDirectoryPath(imagesDirectoryPath, modelName); // Already created by the sparse and CMVS process.

            Reconstruction.RunPMVS(properties, imagesDirectoryPath, modelName, modelDirectoryPath);

            string denseModelPlyFilePath = Reconstruction.GetOutputDensePlyFilePath(modelDirectoryPath);
            string denseModelPatchFilePath = Reconstruction.GetOutputDensePatchFilePath(modelDirectoryPath);

            // Run the mesh.
            string meshFilePath = Reconstruction.GetOutputMeshFilePath(imagesDirectoryPath, modelName);

            Reconstruction.RunMeshing(properties, denseModelPlyFilePath, meshFilePath);

            // Outputs:
            // 1. The sparse NVM file path.
            // 2. The dense model ply file path.
            // 3. The dense patch file path.
            // 4. The mesh file path.
            SfmOutputFilePaths output = new SfmOutputFilePaths()
            {
                DenseModelPatchFilePath = denseModelPatchFilePath,
                DenseModelPlyFilePath = denseModelPlyFilePath,
                MeshFilePath = meshFilePath,
                SparseNvmFilePath = sparseNvmFilePath,
            };
            return output;
        }

        /// <summary>
        /// Runs the Visual SfM sfm+cmvs command.
        /// </summary>
        /// <remarks>
        /// Side effects:
        /// * Creates the {Model Name}.nvm.cmvs directory.
        /// * Creates the "00" model index directory.
        /// * Creates a variety of files in the model index directory.
        /// </remarks>
        public static string RunSparseAndCmvs(IDictionary<string, string> properties, string imagesDirectoryPath, string modelName)
        {
            string visualSfmExecutableFilePath = properties[Properties.VisualSfMExecutableFilePathPropertyName];

            string imageSourcePath = imagesDirectoryPath; // Currently only a directory seems to work. Use an image file list file should be figured out.

            string outputNvmFilePath = Reconstruction.GetOutputNvmFilePath(imagesDirectoryPath, modelName);

            string arguments = $@"sfm+cmvs {imageSourcePath} {outputNvmFilePath}";

            ProcessStarter.RunProcess(visualSfmExecutableFilePath, arguments);

            return outputNvmFilePath;
        }

        public static void RunPMVS(IDictionary<string, string> properties, string imagesDirectoryPath, string modelName, string modelDirectoryPath)
        {
            // Example command:
            // pmvs2 E:\Organizations\Minex\Data\Images\Princeton\Campus\Small\VisualSfM\Tower1.nvm.cmvs\00\ option-0000 {output file here!}

            // Documentation: https://www.di.ens.fr/pmvs/documentation.html

            string pmvs2ExecutablePath = properties[Properties.Pmvs2ExecutableFilePathPropertyName];

            string arguments = $@"{modelDirectoryPath}\ {Constants.OptionFileName} PATCH PSET"; // Needs the trailing back-slash on the directory path.

            ProcessStarter.RunProcess(pmvs2ExecutablePath, arguments);
        }

        public static void RunMeshing(IDictionary<string, string> properties, string inputPlyFilePath, string outputPlyFilePath)
        {
            string meshlabServerExecutableFilePath = properties[Properties.MeshlabServerExecutableFilePathPropertyName];

            string scriptFilePath = properties[Properties.BallPivotingMeshlabScriptFilePathPropertyName];

            string arguments = $@"-i {inputPlyFilePath} -o {outputPlyFilePath} -m vc -s {scriptFilePath}";

            ProcessStarter.RunProcess(meshlabServerExecutableFilePath, arguments);
        }

        public static string GetOutputNvmFilePath(string imagesDirectoryPath, string modelName)
        {
            string outputNvmFileName = $@"{modelName}.nvm";
            string outputNvmFilePath = Path.Combine(imagesDirectoryPath, outputNvmFileName);
            return outputNvmFilePath;
        }

        public static string GetModelDirectoryPath(string imagesDirectoryPath, string modelName)
        {
            // Create the directory for the dense SfM reconstruction.
            string denseSfMDirectoryName = $@"{modelName}.nvm.cmvs";
            string denseSfMDirectoryPath = Path.Combine(imagesDirectoryPath, denseSfMDirectoryName);

            // Create the directory for the model.
            string modelDirectoryName = @"00";
            string modelDirectoryPath = Path.Combine(denseSfMDirectoryPath, modelDirectoryName);

            return modelDirectoryPath;
        }

        public static string GetOutputDensePatchFilePath(string modelDirectoryPath)
        {
            string patchFileName = $@"{Constants.OptionFileName}.patch";
            string output = Path.Combine(modelDirectoryPath, VisualSfmDirectoryStructure.ModelsDirectoryName, patchFileName);
            return output;
        }

        public static string GetOutputDensePlyFilePath(string modelDirectoryPath)
        {
            string patchFileName = $@"{Constants.OptionFileName}.ply";
            string output = Path.Combine(modelDirectoryPath, VisualSfmDirectoryStructure.ModelsDirectoryName, patchFileName);
            return output;
        }

        public static string GetOutputMeshFilePath(string imagesDirectoryPath, string modelName)
        {
            string meshFileName = $@"{modelName}-Mesh.ply";
            string meshFilePath = Path.Combine(imagesDirectoryPath, meshFileName);
            return meshFilePath;
        }

        #endregion
    }
}
