using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

using Public.Common.Lib;
using Public.Common.Lib.Code;
using CodeUtilities = Public.Common.Lib.Code.Utilities;
using Public.Common.Lib.Code.Logical;
using LogicalProject = Public.Common.Lib.Code.Logical.Project;
using Public.Common.Lib.Code.Physical;
using PhysicalSolution = Public.Common.Lib.Code.Physical.Solution;
using Public.Common.Lib.Code.Physical.CSharp;
using PhysicalProject = Public.Common.Lib.Code.Physical.CSharp.Project;
using Public.Common.Lib.Code.Serialization;
using Public.Common.Lib.Extensions;
using Public.Common.WindowsShell;


namespace Public.Common.Avon.Lib
{
    public static class Utilities
    {
        #region Distribute Changes

        public static void DistributeChangesFromDefaultVsVersionSolution(string solutionDirectoryPath)
        {
            string defaultSolutionFilePath = Utilities.GetDefaultSolutionFilePath(solutionDirectoryPath);

            string[] solutionFilePaths = CodeUtilities.GetSolutionFilePaths(solutionDirectoryPath);
            foreach (string solutionFilePath in solutionFilePaths)
            {
                if (defaultSolutionFilePath != solutionFilePath)
                {
                    Utilities.DistributeChanges(defaultSolutionFilePath, solutionFilePath);
                }
            }
        }

        public static void DistributeChangesFromSpecifiedVsVersionSolution(string solutionDirectoryPath, VisualStudioVersion visualStudioVersion)
        {
            string specifiedSolutionFilePath;
            if (!CodeUtilities.GetSolutionFilePath(solutionDirectoryPath, visualStudioVersion, out specifiedSolutionFilePath))
            {
                string message = String.Format(@"Unable to find solution with Visual Studio version: {0}, in directory: {1}.", VisualStudioVersionExtensions.ToDefaultString(visualStudioVersion), solutionDirectoryPath);
                throw new FileNotFoundException(message);
            }

            string[] solutionFilePaths = CodeUtilities.GetSolutionFilePaths(solutionDirectoryPath);
            foreach (string solutionFilePath in solutionFilePaths)
            {
                if (specifiedSolutionFilePath != solutionFilePath)
                {
                    Utilities.DistributeChanges(specifiedSolutionFilePath, solutionFilePath);
                }
            }
        }

        private static void DistributeChanges(string sourceSolutionFilePath, string destinationSolutionFilePath)
        {
            // Determine which projects must be added/removed from the destination.
            // This uses the project directory path since we will need to count different VS versioned project files (in the same directory) as the same file.
            PhysicalSolution sourceSolution = SolutionSerializer.Deserialize(sourceSolutionFilePath);

            ProjectSetCollection sourceProjectSetCollection = new ProjectSetCollection();
            Dictionary<string, Guid> sourceProjectGuidsByDirectoryPath = new Dictionary<string, Guid>();
            foreach (Guid projectGuid in sourceSolution.ProjectsByGuid.Keys)
            {
                SolutionProjectReference projectReference = sourceSolution.ProjectsByGuid[projectGuid];

                string projectFilePath = PathExtensions.GetPath(sourceSolutionFilePath, projectReference.RelativePath);
                string projectDirectoryPath = Path.GetDirectoryName(projectFilePath);
                sourceProjectGuidsByDirectoryPath.Add(projectDirectoryPath, projectGuid);

                VisualStudioVersionedFilePathSet projectVsVersionsSet = Utilities.GetProjectVisualStudioVersionedFilePathSet(projectFilePath);
                sourceProjectSetCollection.ProjectSetsByDirectoryPath.Add(projectVsVersionsSet.DirectoryPath, projectVsVersionsSet);
            }

            PhysicalSolution destinationSolution = SolutionSerializer.Deserialize(destinationSolutionFilePath);

            Dictionary<string, Guid> destinationProjectGuidsByDirectoryPath = new Dictionary<string, Guid>();
            foreach (Guid projectGuid in destinationSolution.ProjectsByGuid.Keys)
            {
                SolutionProjectReference projectReference = destinationSolution.ProjectsByGuid[projectGuid];

                string projectFilePath = PathExtensions.GetPath(destinationSolutionFilePath, projectReference.RelativePath);
                string projectDirectoryPath = Path.GetDirectoryName(projectFilePath);
                destinationProjectGuidsByDirectoryPath.Add(projectDirectoryPath, projectGuid);
            }

            SetDifference<string> projectFilePathDiffs = SetDifference<string>.Calculate(sourceProjectGuidsByDirectoryPath.Keys, destinationProjectGuidsByDirectoryPath.Keys);

            // These project file paths should be added to the destination solution.
            foreach (string projectDirectoryPath in projectFilePathDiffs.InSet1Only)
            {
                VisualStudioVersionedFilePathSet vsVersionSet = sourceProjectSetCollection.ProjectSetsByDirectoryPath[projectDirectoryPath];
                string vsVersionProjectFilePath = vsVersionSet.FilePathsByVisualStudioVersion[destinationSolution.VisualStudioVersion];

                ProjectReference projectReference = ProjectReference.GetFromProjectFilePath(vsVersionProjectFilePath);
                SolutionProjectReference solutionProjectReference = Utilities.GetSolutionProjectReference(destinationSolutionFilePath, projectReference);
                destinationSolution.ProjectsByGuid.Add(solutionProjectReference.GUID, solutionProjectReference);

                Guid destinationGuid = solutionProjectReference.GUID;
                destinationProjectGuidsByDirectoryPath.Add(projectDirectoryPath, destinationGuid);

                // Now add the build configuration info.
                Guid sourceGuid = sourceProjectGuidsByDirectoryPath[projectDirectoryPath];

                foreach (BuildConfiguration destinationBuildConfiguration in destinationSolution.ProjectBuildConfigurationsBySolutionBuildConfiguration.Keys)
                {
                    ProjectBuildConfigurationSet destinationBuildConfigSet = destinationSolution.ProjectBuildConfigurationsBySolutionBuildConfiguration[destinationBuildConfiguration];

                    BuildConfiguration sourceBuildConfiguration;
                    if (sourceSolution.ProjectBuildConfigurationsBySolutionBuildConfiguration.ContainsKey(destinationBuildConfiguration))
                    {
                        sourceBuildConfiguration = destinationBuildConfiguration;
                    }
                    else
                    {
                        sourceBuildConfiguration = Utilities.GetAppropriateSouceBuildConfiguration(destinationBuildConfiguration);
                    }
                    ProjectBuildConfigurationSet sourceBuildConfigSet = sourceSolution.ProjectBuildConfigurationsBySolutionBuildConfiguration[sourceBuildConfiguration];
                    ProjectBuildConfigurationInfo sourceBuildConfigInfo = sourceBuildConfigSet.ProjectBuildConfigurationsByProjectGuid[sourceGuid];

                    // Note, just use the default behavior for the VS version. This will miss any special translation that exists for.
                    ProjectBuildConfigurationInfo destinationBuildConfigInfo = Utilities.GetBuildConfigurationInfo(destinationSolution.VisualStudioVersion, sourceBuildConfigInfo, projectReference.OutputType, destinationBuildConfiguration);
                    destinationBuildConfigSet.ProjectBuildConfigurationsByProjectGuid.Add(destinationGuid, destinationBuildConfigInfo);
                }
            }

            // The project file paths should be removed from the destination.
            foreach (string projectDirectoryPath in projectFilePathDiffs.InSet2Only)
            {
                Guid guidToRemove = destinationProjectGuidsByDirectoryPath[projectDirectoryPath];
                destinationSolution.ProjectsByGuid.Remove(guidToRemove);

                destinationProjectGuidsByDirectoryPath.Remove(projectDirectoryPath);

                foreach (BuildConfiguration buildConfig in destinationSolution.ProjectBuildConfigurationsBySolutionBuildConfiguration.Keys)
                {
                    ProjectBuildConfigurationSet buildConfigSet = destinationSolution.ProjectBuildConfigurationsBySolutionBuildConfiguration[buildConfig];
                    buildConfigSet.ProjectBuildConfigurationsByProjectGuid.Remove(guidToRemove);
                }
            }

            // Serialize changes to the destination solution, if any.
            if (0 < projectFilePathDiffs.InSet1Only.Count || 0 < projectFilePathDiffs.InSet2Only.Count)
            {
                SolutionSerializer.Serialize(destinationSolutionFilePath, destinationSolution);
            }

            // Now that the project list matches between both solutions, we can move on to distributing changes from one VS version project to another.
            foreach (string projectDirectoryPath in sourceProjectGuidsByDirectoryPath.Keys)
            {
                Guid sourceProjectGuid = sourceProjectGuidsByDirectoryPath[projectDirectoryPath];
                Guid destinationProjectGuid = destinationProjectGuidsByDirectoryPath[projectDirectoryPath];

                SolutionProjectReference sourceProjectReference = sourceSolution.ProjectsByGuid[sourceProjectGuid];
                SolutionProjectReference destinationProjectReference = destinationSolution.ProjectsByGuid[destinationProjectGuid];

                string sourceProjectFilePath = PathExtensions.GetPath(sourceSolutionFilePath, sourceProjectReference.RelativePath);
                string destinationProjectFilePath = PathExtensions.GetPath(destinationSolutionFilePath, destinationProjectReference.RelativePath);

                PhysicalProject sourceProject = CSharpProjectSerializer.Deserialize(sourceProjectFilePath);
                PhysicalProject destinationProject = CSharpProjectSerializer.Deserialize(destinationProjectFilePath);

                Utilities.DistributeChanges(sourceProject, sourceProjectFilePath, destinationProject, destinationProjectFilePath, sourceProjectSetCollection);

                // Only serialize the destination.
                CSharpProjectSerializer.Serialize(destinationProjectFilePath, destinationProject);
            }
        }

        public static void DistributeChanges(PhysicalProject sourceProject, string sourceProjectFilePath, PhysicalProject destinationProject, string destinationProjectFilePath, ProjectSetCollection projectSetCollection)
        {
            destinationProject.ProjectItemsByRelativePath.Clear();

            foreach (string relativePath in sourceProject.ProjectItemsByRelativePath.Keys)
            {
                ProjectItem item = sourceProject.ProjectItemsByRelativePath[relativePath];
                ProjectItem clone = item.Clone();

                if (clone is ProjectReferenceProjectItem)
                {
                    ProjectReferenceProjectItem cloneAsProjectItem = clone as ProjectReferenceProjectItem;

                    string referenceProjectFilePath = PathExtensions.GetPath(sourceProjectFilePath, cloneAsProjectItem.IncludePath);
                    VisualStudioVersionedFilePathSet projectVsVersionsSet = Utilities.GetProjectVisualStudioVersionedFilePathSet(referenceProjectFilePath);
                    if (!projectSetCollection.ProjectSetsByDirectoryPath.ContainsKey(projectVsVersionsSet.DirectoryPath))
                    {
                        projectSetCollection.ProjectSetsByDirectoryPath.Add(projectVsVersionsSet.DirectoryPath, projectVsVersionsSet);
                    }

                    Utilities.AdjustProjectReference(cloneAsProjectItem, destinationProjectFilePath, destinationProject.VisualStudioVersion, projectVsVersionsSet);
                }

                destinationProject.ProjectItemsByRelativePath.Add(relativePath, clone);
            }
        }

        public static void AdjustProjectReference(ProjectReferenceProjectItem reference, string destinationProjectFilePath, VisualStudioVersion destinationVisualStudioVersion, VisualStudioVersionedFilePathSet referenceProjectVsVersionedFileSet)
        {
            string referencedProjectFilePath = referenceProjectVsVersionedFileSet.FilePathsByVisualStudioVersion[destinationVisualStudioVersion];

            string relativePath = PathExtensions.GetRelativePath(destinationProjectFilePath, referencedProjectFilePath);

            ProjectReference referenceProject = ProjectReference.GetFromProjectFilePath(referencedProjectFilePath);

            reference.GUID = referenceProject.GUID;
            reference.IncludePath = relativePath;
            reference.Name = referenceProject.Name;
        }

        /// <summary>
        /// Get the default for the visual studio version.
        /// </summary>
        private static ProjectBuildConfigurationInfo GetBuildConfigurationInfo(VisualStudioVersion visualStudioVersion, ProjectBuildConfigurationInfo sourceBuildConfigInfo, ProjectOutputType projectType, BuildConfiguration solutionBuildConfiguration)
        {
            ProjectBuildConfigurationInfo output;
            if (VisualStudioVersion.VS2010 == visualStudioVersion)
            {
                Platform platform;
                if (ProjectOutputType.Library == projectType)
                {
                    platform = Platform.AnyCPU;
                }
                else
                {
                    if (Platform.AnyCPU == solutionBuildConfiguration.Platform)
                    {
                        platform = Platform.AnyCPU;
                    }
                    else
                    {
                        platform = Platform.x86;
                    }
                }
                BuildConfiguration activeConfig = new BuildConfiguration(solutionBuildConfiguration.Configuration, platform);

                output = new ProjectBuildConfigurationInfo(sourceBuildConfigInfo.Build, activeConfig);

                if (ProjectOutputType.Library == projectType && (Platform.x86 == solutionBuildConfiguration.Platform || Platform.x64 == solutionBuildConfiguration.Platform))
                {
                    output.Build = false;
                }
            }
            else
            {
                // For all other VS versions things are simple, just return a clone of the source build configuration info.
                output = new ProjectBuildConfigurationInfo(sourceBuildConfigInfo);
            }

            return output;
        }

        /// <summary>
        /// When translating from VS2013+ solution to a VS2010 solution, the VS2010 has many more (and more complex) build configurations.
        /// </summary>
        private static BuildConfiguration GetAppropriateSouceBuildConfiguration(BuildConfiguration destinationBuildConfiguration)
        {
            // Just produce any CPU.
            BuildConfiguration output = new BuildConfiguration(destinationBuildConfiguration.Configuration, Platform.AnyCPU);
            return output;
        }

        private static VisualStudioVersionedFilePathSet GetProjectVisualStudioVersionedFilePathSet(string projectFilePath)
        {
            string projectDirectoryPath = Path.GetDirectoryName(projectFilePath); // NOTE, assumes that each project set will have its own directory, i.e. the ony time two project files will be in the same directory is when they are just different VS versions of each other.

            VisualStudioVersionedFilePathSet output = new VisualStudioVersionedFilePathSet(projectDirectoryPath);

            string projectFileExtension = Path.GetExtension(projectFilePath);
            string projectFileSearchPattern = @"*" + projectFileExtension;
            string[] projectFilePaths = Directory.GetFiles(projectDirectoryPath, projectFileSearchPattern, SearchOption.TopDirectoryOnly);

            int numProjectFilePaths = projectFilePaths.Length; // There should be at least one project file path since we were given a project file path.
            if (0 == numProjectFilePaths)
            {
                string message = String.Format(@"No project files found in directory: {0}", projectDirectoryPath);
                throw new InvalidOperationException(message);
            }

            if (1 == numProjectFilePaths)
            {
                string curProjectFilePath = projectFilePaths[0]; // Should be the same as the input project file path, but not checked.
                if (CodeUtilities.IsVisualStudioVersioned(curProjectFilePath))
                {
                    ProjectFileNameInfo curProjectNameInfo = ProjectFileNameInfo.Parse(curProjectFilePath, VisualStudioVersion.VS2015); // Dummy VS version, we know it is versioned.
                    output.FilePathsByVisualStudioVersion.Add(curProjectNameInfo.VisualStudioVersion, curProjectFilePath);
                }
                else
                {
                    VisualStudioVersion[] allVsVersions = VisualStudioVersionExtensions.GetAllVisualStudioVersions();
                    foreach (VisualStudioVersion vsVersion in allVsVersions)
                    {
                        output.FilePathsByVisualStudioVersion.Add(vsVersion, curProjectFilePath);
                    }
                }
            }

            foreach (string curProjectFilePath in projectFilePaths)
            {
                if (CodeUtilities.IsVisualStudioVersioned(curProjectFilePath))
                {
                    ProjectFileNameInfo curProjectNameInfo = ProjectFileNameInfo.Parse(curProjectFilePath, VisualStudioVersion.VS2015); // Again, dummy VS version, we know it is versioned.
                    output.FilePathsByVisualStudioVersion.Add(curProjectNameInfo.VisualStudioVersion, curProjectFilePath);
                }
                else
                {
                    string message = String.Format(@"Project file not visual studio versioned: {0}", curProjectFilePath);
                    throw new InvalidOperationException(message);
                }
            }

            return output;
        }

        private static SolutionProjectReference GetSolutionProjectReference(string solutionFilePath, ProjectReference projectReference)
        {
            string relativePath = PathExtensions.GetRelativePath(solutionFilePath, projectReference.Path);

            SolutionProjectReference output = new SolutionProjectReference(projectReference.Name, relativePath, projectReference.GUID);
            return output;
        }

        #endregion

        #region Ensure VS-Versioned Bin And Obj Project Properties

        public static void EnsureVsVersionedBinAndObjProperties(string solutionDirectoryPath, HashSet<string> projectDirectoryPathsAllowedToChange)
        {
            string[] solutionFilePaths = CodeUtilities.GetSolutionFilePaths(solutionDirectoryPath);
            foreach (string solutionFilePath in solutionFilePaths)
            {
                PhysicalSolution solution = SolutionSerializer.Deserialize(solutionFilePath);

                List<string> projectFilePaths = CodeUtilities.GetProjectFilePaths(solutionFilePath, solution);
                foreach (string projectFilePath in projectFilePaths)
                {
                    string projectDirectoryPath = Path.GetDirectoryName(projectFilePath);
                    if (projectDirectoryPathsAllowedToChange.Contains(projectDirectoryPath))
                    {
                        Utilities.EnsureVsVersionedBinAndObjProperties(projectFilePath);
                    }
                }
            }
        }

        private static void EnsureVsVersionedBinAndObjProperties(string projectFilePath)
        {
            ProjectFileNameInfo projectInfo = ProjectFileNameInfo.Parse(projectFilePath, VisualStudioVersion.VS2015); // Dummy VS version.

            // Load project XML.
            XmlDocument doc = new XmlDocument();
            doc.LoadNoNamespaces(projectFilePath);

            // Debug.
            //XmlNode debugBuildNode = doc.SelectSingleNode(@"/Project/PropertyGroup[contains(@Condition,'Debug|AnyCPU')]");
            XmlNode debugBuildNode = doc.SelectSingleNode(@"/Project/PropertyGroup[contains(@Condition,'Debug')]");
            XmlNode debugBinNode = debugBuildNode.SelectSingleNode(CSharpProjectSerializer.OutputPathNodeName);
            debugBinNode.InnerText = BuildConfigurationInfo.GetBinaryOutputDirectoryName(projectInfo.VisualStudioVersion, Configuration.Debug);

            XmlNode debugObjNode = debugBuildNode.SelectSingleNode(CSharpProjectSerializer.BaseIntermediateOutputPathNodeName);
            if (null == debugObjNode)
            {
                debugObjNode = XmlHelper.AddChildElement(debugBuildNode, CSharpProjectSerializer.BaseIntermediateOutputPathNodeName, debugBinNode);
            }
            debugObjNode.InnerText = BuildConfigurationInfo.GetObjectIntermediateDirectoryName(projectInfo.VisualStudioVersion);

            // Release.
            XmlNode releaseBuildNode = doc.SelectSingleNode(@"Project/PropertyGroup[contains(@Condition,'Release')]");
            XmlNode releaseBinNode = releaseBuildNode.SelectSingleNode(CSharpProjectSerializer.OutputPathNodeName);
            releaseBinNode.InnerText = BuildConfigurationInfo.GetBinaryOutputDirectoryName(projectInfo.VisualStudioVersion, Configuration.Release);

            XmlNode releaseObjNode = releaseBuildNode.SelectSingleNode(CSharpProjectSerializer.BaseIntermediateOutputPathNodeName);
            if (null == releaseObjNode)
            {
                releaseObjNode = XmlHelper.AddChildElement(releaseBuildNode, CSharpProjectSerializer.BaseIntermediateOutputPathNodeName, releaseBinNode);
            }
            releaseObjNode.InnerText = BuildConfigurationInfo.GetObjectIntermediateDirectoryName(projectInfo.VisualStudioVersion);

            // Save project XML.
            XmlHelper.FixXmlDocumentNamespaceForSave(doc, CSharpProjectSerializer.MsBuild2003XmlNamespaceName);
            doc.Save(projectFilePath);
        }

        #endregion

        #region Create Solution Set With Default

        /// <summary>
        /// Create a solution set of all Visual Studio versions, but with a shortcut pointing to one specific version as the default.
        /// </summary>
        public static void CreateSolutionSetWithDefault(NewSolutionSetSpecification solutionSetSpecifcation, VisualStudioVersion defaultVisualStudioVersion)
        {
            Creation.CreateSolutionSet(solutionSetSpecifcation);

            string solutionDirectoryPath = Creation.DetermineSolutionDirectoryPath(solutionSetSpecifcation.BaseSolutionSpecification);
            Utilities.SetDefaultVisualStudioVersion(solutionDirectoryPath, defaultVisualStudioVersion);
        }

        /// <remarks>
        /// Examine the shortcut to the default Visual Studio solution file created with the solution set, and get thus get the path of the default solution.
        /// </remarks>
        public static VisualStudioVersion DetermineDefaultSolutionVisualStudioVersion(string defaultSolutionShortcutFilePath)
        {
            string defaultSolutionFilePath = WindowsShellRuntimeWrapper.GetShortcutTargetPath(defaultSolutionShortcutFilePath);

            SolutionFileNameInfo solutionFileNameInfo = SolutionFileNameInfo.Parse(defaultSolutionFilePath);
            return solutionFileNameInfo.VisualStudioVersion;
        }

        /// <remarks>
        /// This method assumes there are multiple VS version labeled solution files in the solution directory.
        /// </remarks>
        public static void SetDefaultVisualStudioVersion(string solutionDirectoryPath, VisualStudioVersion defaultVisualStudioVersion)
        {
            string[] solutionFilePaths = Directory.GetFiles(solutionDirectoryPath, @"*.sln", SearchOption.TopDirectoryOnly);
            foreach (string solutionFilePath in solutionFilePaths)
            {
                SolutionFileNameInfo fileNameInfo = SolutionFileNameInfo.Parse(solutionFilePath);
                if (defaultVisualStudioVersion == fileNameInfo.VisualStudioVersion)
                {
                    // Make the shortcut.
                    string shortCutFileName = String.Format(@"{0}.{1}", fileNameInfo.FileNameBase, SolutionFileNameInfo.SolutionFileExtension);
                    string shortCutFilePath = Path.Combine(solutionDirectoryPath, shortCutFileName);

                    WindowsShellRuntimeWrapper.CreateShortcut(shortCutFilePath, solutionFilePath);
                }
            }
        }

        /// <summary>
        /// Get the default solution file path.
        /// </summary>
        public static string GetDefaultSolutionFilePath(string solutionsDirectoryPath)
        {
            string defaultSolutionShortcutFilePath = Utilities.GetDefaultSolutionShortcutFilePath(solutionsDirectoryPath);

            string output = WindowsShellRuntimeWrapper.GetShortcutLinkTargetPath(defaultSolutionShortcutFilePath);
            return output;
        }

        /// <summary>
        /// Get the path of the default solution file link.
        /// </summary>
        public static string GetDefaultSolutionShortcutFilePath(string solutionsDirectoryPath)
        {
            string[] solutionLinkFiles = Directory.GetFiles(solutionsDirectoryPath, @"*.sln.lnk");
            if (1 != solutionLinkFiles.Length)
            {
                throw new InvalidOperationException(@"Unable to determine default solution from link target due to the presence of multiple links in the solution directory.");
            }

            string output = solutionLinkFiles[0];
            return output;
        }

        #endregion
    }
}
