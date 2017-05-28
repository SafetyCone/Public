using System;
using System.IO;

using Public.Common.Augustus.Lib;
using ProjectFileNameInfo = Public.Common.Lib.Code.ProjectFileNameInfo;
using Project = Public.Common.Lib.Code.Physical.CSharp.Project;
using ProjectSerializer = Public.Common.Lib.Code.Serialization.CSharpProjectSerializer;
using VsVersionExtensions = Public.Common.Lib.Code.Physical.VisualStudioVersionExtensions;
using Public.Common.Lib.Extensions;
using Public.Common.Lib.IO;
using Public.Common.Lib.IO.Extensions;


namespace Public.Common.Maynard
{
    class Program
    {
        private static void Main(string[] args)
        {
            Program.SubMain(args);
            //Testing.SubMain(args);
            //Construction.SubMain(args);
        }

        private static void SubMain(string[] args)
        {
            IOutputStream outputStream = new ConsoleOutputStream();

            Configuration config;
            if (Configuration.TryParseArguments(out config, outputStream, args))
            {
                // Determine the proposed binaries directory path.
                string datedBinariesDirectoryPath = Path.Combine(config.OutputDirectoryPath, DateTime.Today.ToYYYYMMDDStr());
                string currentDirectoryPath = Path.Combine(config.OutputDirectoryPath, Constants.CurrentDirectoryName);

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

                            // Create the binaries directory path.
                            ProjectFileNameInfo nameInfo = ProjectFileNameInfo.Parse(projectFilePath);
                            string projectDirectoryName = nameInfo.FileNameBase;
                            string vsVersionProjectDirectoryName = String.Format(@"{0}.{1}", nameInfo.FileNameBase, VsVersionExtensions.ToDefaultString(nameInfo.VisualStudioVersion));
                            string projectOutputDirectoryPath = Path.Combine(datedBinariesDirectoryPath, projectDirectoryName, vsVersionProjectDirectoryName);

                            if (!Directory.Exists(projectOutputDirectoryPath))
                            {
                                Directory.CreateDirectory(projectOutputDirectoryPath);
                            }

                            // Copy all files from source to destination.
                            DirectoryExtensions.Copy(projectBinDirectoryPath, projectOutputDirectoryPath);

                            // Copy all files to the current directory.
                            string projectCurrentDirectoryPath = Path.Combine(currentDirectoryPath, projectDirectoryName, vsVersionProjectDirectoryName);
                            DirectoryExtensions.Copy(projectBinDirectoryPath, projectCurrentDirectoryPath);
                        }
                        else
                        {
                            string message = String.Format(@"Unable to build project: {0}", projectFilePath);
                            throw new InvalidOperationException(message);
                        }
                    }
                    catch (Exception ex)
                    {
                        string line = String.Format(@"ERROR: {0}", buildItemSpecification);
                        outputStream.WriteLine(line);
                        string message = String.Format(@"ERROR: {0}", ex.Message);
                        outputStream.WriteLineAndBlankLine(message);
                    }
                }
            }
        }
    }
}
