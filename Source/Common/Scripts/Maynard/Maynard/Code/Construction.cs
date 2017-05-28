using System;
using System.IO;

using Public.Common.Augustus.Lib;
using ProjectFileNameInfo = Public.Common.Lib.Code.ProjectFileNameInfo;
using Project = Public.Common.Lib.Code.Physical.CSharp.Project;
using ProjectSerializer = Public.Common.Lib.Code.Serialization.CSharpProjectSerializer;
using VsVersionExtensions = Public.Common.Lib.Code.Physical.VisualStudioVersionExtensions;
using Public.Common.Lib.Extensions;
using Public.Common.Lib.IO;


namespace Public.Common.Maynard
{
    public static class Construction
    {
        public static void SubMain(string[] args)
        {
            //Construction.ProcessAllBinaries();
            Construction.ProcessBinary();
        }

        public static void ProcessAllBinaries(string[] args)
        {
            IOutputStream outputStream = new ConsoleOutputStream();

            Configuration config;
            if (Configuration.TryParseArguments(out config, outputStream, args))
            {
                string[] buildItemSpecifications = File.ReadAllLines(config.ProjectBuildListFilePath);
                foreach (string buildItemSpecification in buildItemSpecifications)
                {
                    try
                    {
                        // Build the project.
                        BuildItem item = BuildItem.Parse(buildItemSpecification);
                        bool success = Builder.Run(item, new ConsoleOutputStream(), new ConsoleOutputStream());

                        string projectFilePath = item.FilePath;
                        if (success)
                        {
                            // Determine the location of the project's binaries.
                            Project project = ProjectSerializer.Deserialize(projectFilePath);
                            string projectBinDirectoryRelativePath = project.BuildConfigurationInfos[project.ActiveConfiguration].BinOutputPath;
                            string projectDirectoryPath = Path.GetDirectoryName(projectFilePath);
                            string projectBinDirectoryPath = Path.Combine(projectDirectoryPath, projectBinDirectoryRelativePath);

                            // Determine the proposed binaries directory path.
                            string datedBinariesDirectory = Path.Combine(config.OutputDirectoryPath, DateTime.Today.ToYYYYMMDDStr());

                            ProjectFileNameInfo nameInfo = ProjectFileNameInfo.Parse(projectFilePath);
                            string projectDirectoryName = nameInfo.FileNameBase;
                            string vsVersionProjectDirectoryName = String.Format(@"{0}.{1}", nameInfo.FileNameBase, VsVersionExtensions.ToDefaultString(nameInfo.VisualStudioVersion));
                            string projectOutputDirectoryPath = Path.Combine(datedBinariesDirectory, projectDirectoryName, vsVersionProjectDirectoryName);

                            // Create the binaries directory path.
                            if (!Directory.Exists(projectOutputDirectoryPath))
                            {
                                Directory.CreateDirectory(projectOutputDirectoryPath);
                            }

                            // Copy all files from source to destination.
                            DirectoryExtensions.Copy(projectBinDirectoryPath, projectOutputDirectoryPath);
                        }
                        else
                        {
                            string message = String.Format(@"Unable to build project: {0}", projectFilePath);
                            throw new InvalidOperationException(message);
                        }
                    }
                    catch (Exception ex)
                    {
                        string message = String.Format(@"ERROR: {0}", ex.Message);
                        outputStream.WriteLine(message);
                    }
                }
            }
        }

        public static void ProcessBinary()
        {
            string projectFilePath = @"C:\Organizations\Minex\Repositories\Public\Source\Common\Scripts\Augustus\Augustus\Augustus.VS2010.csproj";
            string binariesDirectory = @"C:\Organizations\Minex\Binaries";

            // Build the project.
            string buildItemSpecification = String.Format(@"{0}|Windows|x86", projectFilePath);
            bool success = Builder.Run(buildItemSpecification, new ConsoleOutputStream(), new ConsoleOutputStream());

            if (success)
            {
                // Determine the location of the project's binaries.
                Project project = ProjectSerializer.Deserialize(projectFilePath);
                string projectBinDirectoryRelativePath = project.BuildConfigurationInfos[project.ActiveConfiguration].BinOutputPath;
                string projectDirectoryPath = Path.GetDirectoryName(projectFilePath);
                string projectBinDirectoryPath = Path.Combine(projectDirectoryPath, projectBinDirectoryRelativePath);

                // Determine the proposed binaries directory path.
                string datedBinariesDirectory = Path.Combine(binariesDirectory, DateTime.Today.ToYYYYMMDDStr());

                ProjectFileNameInfo nameInfo = ProjectFileNameInfo.Parse(projectFilePath);
                string projectDirectoryName = nameInfo.FileNameBase;
                string vsVersionProjectDirectoryName = String.Format(@"{0}.{1}", nameInfo.FileNameBase, VsVersionExtensions.ToDefaultString(nameInfo.VisualStudioVersion));
                string projectOutputDirectoryPath = Path.Combine(datedBinariesDirectory, projectDirectoryName, vsVersionProjectDirectoryName);

                // Create the binaries directory path.
                if (!Directory.Exists(projectOutputDirectoryPath))
                {
                    Directory.CreateDirectory(projectOutputDirectoryPath);
                }

                // Copy all files from source to destination.
                DirectoryExtensions.Copy(projectBinDirectoryPath, projectOutputDirectoryPath);
            }
            else
            {
                string message = String.Format(@"Unable to build project: {0}", projectFilePath);
                throw new Exception(message); // TODO, better exception type.
            }
        }
    }
}
