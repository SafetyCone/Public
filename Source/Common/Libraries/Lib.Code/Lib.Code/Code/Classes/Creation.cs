using System;
using System.Collections.Generic;
using System.IO;

using Public.Common.Lib.Code.Logical;
using LogicalProject = Public.Common.Lib.Code.Logical.Project;
using LogicalSolution = Public.Common.Lib.Code.Logical.Solution;
using Public.Common.Lib.Code.Physical;
using PhysicalSolution = Public.Common.Lib.Code.Physical.Solution;
using Public.Common.Lib.Code.Physical.CSharp;
using PhysicalCSharpProject = Public.Common.Lib.Code.Physical.CSharp.Project;
using Public.Common.Lib.Code.Serialization;
using Public.Common.Lib.Code.Serialization.Extensions;
using Public.Common.Lib.Extensions;
using Public.Common.Lib.IO;
using Public.Common.Lib.IO.Serialization;
using Public.Common.Lib.IO.Serialization.Extensions;


namespace Public.Common.Lib.Code
{
    // Ok.
    public class Creation
    {
        public const string ConsoleProjectMoniker = @"Console";
        public const string ConsoleLibraryProjectMoniker = @"ConsoleLibrary";
        public const string LibraryProjectMoniker = @"Library";
        public const string LibraryConstructionConsoleProjectMoniker = @"ConstructionConsole";

        // Ok.
        public const string Lib = @"Lib";
        // Ok.
        public const string Construction = @"Construction";


        #region Static

        // OK.
        /// <summary>
        /// Creates (both constructs and serializes) a new solution based on the provided specification.
        /// </summary>
        public static void CreateSolution(NewSolutionSpecification specification)
        {
            SerializationList serializationList = new SerializationList();
            SerializationListCodeExtensions.AddDefaultSerializersByMoniker(serializationList);

            Creation.CreateSolution(serializationList, specification);

            serializationList.Serialize();
        }

        // Ok.
        /// <summary>
        /// Constructs a new solution and add all component files (solution, projects, and classes) to the serialization list.
        /// </summary>
        private static LogicalSolution CreateSolution(SerializationList serializationList, NewSolutionSpecification specification)
        {
            string solutionTypeDirectoryPath = Creation.DetermineSolutionTypeDirectoryPath(specification);

            LogicalSolution output = Creation.CreateSolution(serializationList, specification, solutionTypeDirectoryPath);
            return output;
        }

       // Ok.
       /// <summary>
       /// Constructs a new solution in the given solution type directory.
       /// </summary>
        private static LogicalSolution CreateSolution(SerializationList serializationList, NewSolutionSpecification specification, string solutionTypeDirectoryPath)
        {
            string solutionFilePath = Creation.DetermineSolutionFilePath(solutionTypeDirectoryPath, specification);

            LogicalSolution solution = Creation.CreateLogicalSolution(serializationList, specification, solutionTypeDirectoryPath);

            List<ProjectReference> projectReferences = Creation.GetProjectReferences(solutionFilePath, solution);

            PhysicalSolution physicalSolution = Creation.CreatePhysicalSolution(specification, projectReferences, solution);
            serializationList.AddSolution(solutionFilePath, physicalSolution);

            return solution;
        }

        // Ok.
        private static List<ProjectReference> GetProjectReferences(string solutionFilePath, LogicalSolution solution)
        {
            List<ProjectReference> output = new List<ProjectReference>();
            foreach(string projectPath in solution.ProjectsByPath.Keys)
            {
                LogicalProject project = solution.ProjectsByPath[projectPath];

                string name = project.Info.NamesInfo.Name;
                string relativePath = PathExtensions.GetRelativePath(solutionFilePath, projectPath);
                Guid guid = project.Info.GUID;

                ProjectReference reference = new ProjectReference(name, relativePath, guid);
                output.Add(reference);
            }

            return output;
        }

        // Ok.
        /// <summary>
        /// Creates the solution, adding all sub-components to the serialization list.
        /// </summary>
        private static LogicalSolution CreateLogicalSolution(SerializationList serializationList, NewSolutionSpecification specification, string solutionTypeDirectoryPath)
        {
            LogicalSolution solution = new LogicalSolution();
            solution.Info = Creation.GetSolutionInfo(specification);

            string solutionDirectoryPath = Creation.CreateSolutionDirectoryPath(specification, solution.Info.NamesInfo);

            List<NewProjectSpecification> projectSpecifications = Creation.GetProjectSpecifications(solutionDirectoryPath, specification);
            foreach(NewProjectSpecification projectSpecification in projectSpecifications)
            {
                LogicalProject project = Creation.CreateProject(serializationList, solutionDirectoryPath, projectSpecification);
                string projectFilePath = Creation.GetProjectFilePath(solutionDirectoryPath, project.Info);
                solution.ProjectsByPath.Add(projectFilePath, project);

                PhysicalCSharpProject physicalProject = Creation.CreatePhysicalCSharpProject(serializationList, specification, project);
                serializationList.AddProject(projectFilePath, physicalProject);
            }

            return solution;
        }

        // Ok.
        /// <summary>
        /// Gets project specifications, including any references between projects (which means creating project specifications in a particular order).
        /// </summary>
        private static List<NewProjectSpecification> GetProjectSpecifications(string solutionDirectoryPath, NewSolutionSpecification solutionSpecification)
        {
            List<NewProjectSpecification> output = new List<NewProjectSpecification>();

            NewProjectSpecification librarySpecification = new NewProjectSpecification(solutionSpecification, ProjectType.Library);
            ProjectInfo libraryProjectInfo = Creation.GetProjectInfo(librarySpecification);
            string libraryFileProjectPath = Creation.GetProjectFilePath(solutionDirectoryPath, libraryProjectInfo);

            NewProjectSpecification console = new NewProjectSpecification(solutionSpecification, ProjectType.Console);
            console.ReferencedProjectsByPath.Add(libraryFileProjectPath, libraryProjectInfo);

            // Add the console project first to see if the startup project can be set.
            output.Add(console);
            output.Add(librarySpecification);

            return output;
        }

        // Ok.
        private static SolutionInfo GetSolutionInfo(NewSolutionSpecification specification)
        {
            SolutionInfo output = new SolutionInfo();
            output.Type = specification.SolutionType;
            output.NamesInfo.Name = specification.SolutionName;
            output.NamesInfo.DirectoryName = specification.SolutionName;
            output.NamesInfo.FileName = Creation.DetermineSolutionFileName(specification);

            return output;
        }

        // Ok.
        /// <summary>
        /// Creates (constructs and serializes) a new project, including all code files.
        /// </summary>
        private static LogicalProject CreateProject(SerializationList serializationList, string solutionDirectoryPath, NewProjectSpecification specification)
        {
            LogicalProject project = new LogicalProject();
            project.Info = Creation.GetProjectInfo(specification);

            string projectDirectoryPath = Creation.GetProjectDirectoryPath(solutionDirectoryPath, project.Info);

            List<ProjectItem> projectItems = new List<ProjectItem>();
            Creation.AddProjectItemsForProject(
                serializationList,
                projectItems,
                projectDirectoryPath,
                project.Info,
                specification.SolutionType,
                specification.ReferencedProjectsByPath,
                specification.VisualStudioVersion);

            foreach (ProjectItem item in projectItems)
            {
                project.ProjectItemsByRelativePath.Add(item.IncludePath, item);
            }

            return project;
        }

        // Ok.
        private static PhysicalCSharpProject CreatePhysicalCSharpProject(SerializationList serializationList, NewSolutionSpecification solutionSpecification, LogicalProject logicalProject)
        {
            PhysicalCSharpProject output = new PhysicalCSharpProject(logicalProject);

            output.VisualStudioVersion = solutionSpecification.VisualStudioVersion;
            output.TargetFrameworkVersion = Creation.GetDefaultNetFrameworkVersion(solutionSpecification.VisualStudioVersion);
            output.OutputType = logicalProject.Info.Type.ToDefaultOutputType();
            output.ActiveConfiguration = Creation.GetDefaultActiveConfiguration(logicalProject, solutionSpecification.VisualStudioVersion);

            Creation.AddBuildConfigurationInfos(output, solutionSpecification.VisualStudioVersion);
            // No need to worry about imports here, just the default so far.

            return output;
        }

        // Ok.
        private static string GetProjectDirectoryPath(string solutionDirectoryPath, ProjectInfo info)
        {
            string output = Path.Combine(solutionDirectoryPath, info.NamesInfo.DirectoryName);
            return output;
        }

        // Ok.
        private static string GetProjectFilePath(string solutionDirectoryPath, ProjectInfo info)
        {
            string projectDirectoryPath = Creation.GetProjectDirectoryPath(solutionDirectoryPath, info);

            string output = Path.Combine(projectDirectoryPath, info.NamesInfo.FileName);
            return output;
        }

        private static void AddProjectItemsForProject(
            SerializationList serializationList,
            List<ProjectItem> projectItems,
            string projectDirectoryPath,
            ProjectInfo info,
            SolutionType solutionType,
            Dictionary<string, ProjectInfo> referencedProjectsByPath,
            VisualStudioVersion visualStudioVersion)
        {
            Creation.AddReferenceProjectItems(projectItems, info.Language);
            Creation.AddCompilationProjectItems(serializationList, projectItems, projectDirectoryPath, info);
            Creation.AddProjectReferenceCompilationItems(projectItems, projectDirectoryPath, referencedProjectsByPath);
            Creation.AddContentProjectItems(serializationList, projectItems, projectDirectoryPath, info, solutionType);
            Creation.AddFolderProjectItems(serializationList, projectItems, projectDirectoryPath);
            Creation.AddNoneProjectItems(serializationList, projectItems, projectDirectoryPath, visualStudioVersion);
        }

        private static void AddNoneProjectItems(SerializationList serializationList, List<ProjectItem> projectItems, string projectDirectoryPath, VisualStudioVersion visualStudioVersion)
        {
            ProjectItem appConfigItem = Creation.GetAppConfigProjectItem(serializationList, projectDirectoryPath, visualStudioVersion);
            projectItems.Add(appConfigItem);
        }

        private static ProjectItem GetAppConfigProjectItem(SerializationList serializationList, string projectDirectoryPath, VisualStudioVersion visualStudioVersion)
        {
            TextFile appConfig = new TextFile();
            appConfig.Lines.Add(@"<?xml version=""1.0"" encoding=""utf-8"" ?>");
            appConfig.Lines.Add(@"<configuration>");
            appConfig.Lines.Add(@"    <startup>");
            appConfig.Lines.Add(@"        <supportedRuntime version = ""v4.0"" sku = "".NETFramework,Version=v4.5.2"" />");
            appConfig.Lines.Add(@"    </startup>");
            appConfig.Lines.Add(@"</configuration>");

            string fileRelativePath;
            if (VisualStudioVersion.VS2010 == visualStudioVersion)
            {
                fileRelativePath = @"app.config";
            }
            else
            {
                fileRelativePath = @"App.config";
            }

            string filePath = Path.Combine(projectDirectoryPath, fileRelativePath);
            serializationList.AddTextFile(filePath, appConfig);

            ProjectItem output = new ContentProjectItem(fileRelativePath);
            return output;
        }

        private static void AddFolderProjectItems(SerializationList serializationList, List<ProjectItem> projectItems, string projectDirectoryPath)
        {
            string tempFilePath = Path.Combine(projectDirectoryPath, @"temp");
            string codeDirectoryPath = Path.Combine(projectDirectoryPath, @"Code");
            string relativePath = PathExtensions.GetRelativePath(tempFilePath, codeDirectoryPath);

            serializationList.AddCreateDirectory(relativePath);

            projectItems.Add(new FolderProjectItem(relativePath));
        }

        private static void AddContentProjectItems(SerializationList serializationList, List<ProjectItem> projectItems, string projectDirectoryPath, ProjectInfo info, SolutionType solutionType)
        {
            switch(solutionType)
            {
                case SolutionType.Library:
                    if(ProjectType.Library == info.Type)
                    {
                        ProjectItem projectPlan = Creation.GetProjectPlanProjectItem(serializationList, projectDirectoryPath);
                    }
                    break;

                default:
                    if (ProjectType.Library != info.Type)
                    {
                        ProjectItem projectPlan = Creation.GetProjectPlanProjectItem(serializationList, projectDirectoryPath);
                    }
                    break;
            }
        }

        private static ProjectItem GetProjectPlanProjectItem(SerializationList serializationList, string projectDirectoryPath)
        {
            TextFile projectPlan = new TextFile(); 

            string fileRelativePath = @"Project Plan.txt";
            string filePath = Path.Combine(projectDirectoryPath, fileRelativePath);
            serializationList.AddTextFile(filePath, projectPlan);

            ProjectItem output = new ContentProjectItem(fileRelativePath);
            return output;
        }

        private static void AddProjectReferenceCompilationItems(List<ProjectItem> projectItems, string projectDirectoryPath, Dictionary<string, ProjectInfo> referencedProjectsByPath)
        {
            string tempFilePath = Path.Combine(projectDirectoryPath, @"temp");
            foreach(string projectPath in referencedProjectsByPath.Keys)
            {
                ProjectInfo referencedProject = referencedProjectsByPath[projectPath];

                string relativePath = PathExtensions.GetRelativePath(tempFilePath, projectPath);
                ProjectReferenceProjectItem reference = new ProjectReferenceProjectItem(relativePath, referencedProject.GUID, referencedProject.NamesInfo.Name);
                projectItems.Add(reference);
            }
        }

        private static void AddCompilationProjectItems(SerializationList serializationList, List<ProjectItem> projectItems, string projectDirectoryPath, ProjectInfo info)
        {
            switch (info.Type)
            {
                case ProjectType.Console:
                    {
                        ProjectItem program = Creation.GetConsoleProgramProjectItem(serializationList, projectDirectoryPath, info.NamesInfo.RootNamespaceName);
                        projectItems.Add(program);
                        ProjectItem assemblyInfo = Creation.GetAssemblyInfoProjectItem(serializationList, projectDirectoryPath, info.NamesInfo.Name, info.GUID);
                        projectItems.Add(assemblyInfo);
                    }
                    break;

                case ProjectType.Library:
                    {
                        ProjectItem assemblyInfo = Creation.GetAssemblyInfoProjectItem(serializationList, projectDirectoryPath, info.NamesInfo.Name, info.GUID);
                        projectItems.Add(assemblyInfo);
                    }
                    break;

                default:
                    break; // Do nothing.
            }
        }

        private static ProjectItem GetAssemblyInfoProjectItem(SerializationList serializationList, string projectDirectoryPath, string projectName, Guid guid)
        {
            EmptyType assemblyInfo = CreateAssemblyInfo.GetAssemblyInfo(projectName, guid);
            CodeFile assemblyInfoFile = CodeFile.ProcessEmptyType(assemblyInfo);

            string fileRelativePath = @"Properties\AssemblyInfo.cs";
            string filePath = Path.Combine(projectDirectoryPath, fileRelativePath);
            serializationList.AddCodeFile(filePath, assemblyInfoFile);

            ProjectItem output = new CompileProjectItem(fileRelativePath);
            return output;
        }

        private static ProjectItem GetConsoleProgramProjectItem(SerializationList serializationList, string projectDirectoryPath, string projectRootNamespaceName)
        {
            Class program = CreateConsoleProgramClass.CreateProgram(projectRootNamespaceName);
            CodeFile programFile = CodeFile.ProcessClass(program);

            string fileRelativePath = @"Code\Program.cs";
            string filePath = Path.Combine(projectDirectoryPath, fileRelativePath);
            serializationList.AddCodeFile(filePath, programFile);

            ProjectItem output = new CompileProjectItem(fileRelativePath);
            return output;
        }

        private static void AddReferenceProjectItems(List<ProjectItem> projectItems, Language language)
        {
            switch(language)
            {
                case Language.CSharp:
                    string[] assemblyNames = new string[]
                    {
                        @"System",
                        @"System.Core",
                        @"System.Xml.Linq",
                        @"System.Data.DataSetExtensions",
                        @"Microsoft.CSharp",
                        @"System.Data",
                        @"System.Net.Http",
                        @"System.Xml",
                    };

                    foreach(string assemblyName in assemblyNames)
                    {
                        projectItems.Add(new ReferenceProjectItem(assemblyName));
                    }
                    break;

                default:
                    throw new UnexpectedEnumerationValueException<Language>(language);
            }
        }

        // Ok.
        private static ProjectInfo GetProjectInfo(NewProjectSpecification specification)
        {
            ProjectInfo output = new ProjectInfo();
            Creation.DetermineNamesInfoForProject(output.NamesInfo, specification);
            output.GUID = Guid.NewGuid();
            output.Language = specification.Language;
            output.Type = specification.ProjectType;

            return output;
        }

        #region Physical

        private static string CreateSolutionFilePath(NewSolutionSpecification specification, PhysicalSolution solution)
        {
            string solutionDirectoryPath = Creation.CreateSolutionDirectoryPath(specification, solution.Info.NamesInfo);

            string solutionFileName = String.Format(@"{0}.{1}", solution.Info.NamesInfo.FileName, SolutionSerializer.SolutionFileExtension);

            string output = Path.Combine(solutionDirectoryPath, solutionFileName);
            return output;
        }

        // Ok.
        public static PhysicalSolution CreatePhysicalSolution(
            NewSolutionSpecification specification,
            List<ProjectReference> projects,
            LogicalSolution logicalSolution)
        {
            PhysicalSolution output = new PhysicalSolution();
            output.Info = logicalSolution.Info;
            output.VisualStudioVersion = specification.VisualStudioVersion;

            foreach (ProjectReference project in projects)
            {
                output.ProjectsByGuid.Add(project.GUID, project);
            }

            Creation.AddDefaultProjectBuildConfigurations(output, logicalSolution);

            return output;
        }

        // Ok.
        private static void AddDefaultProjectBuildConfigurations(PhysicalSolution physicalSolution, LogicalSolution logicalSolution)
        {
            switch (physicalSolution.VisualStudioVersion)
            {
                case VisualStudioVersion.VS2010:
                    Creation.AddDefaultProjectBuildConfigurationsVs2010(physicalSolution, logicalSolution);
                    break;

                default:
                    Creation.AddDefaultProjectBuildConfigurationsNonVs2010(physicalSolution, logicalSolution);
                    break;
            }
        }

        // Ok.
        private static void AddDefaultProjectBuildConfigurationsVs2010(PhysicalSolution physicalSolution, LogicalSolution logicalSolution)
        {
            List<BuildConfiguration> buildConfigs = new List<BuildConfiguration>();
            buildConfigs.Add(new BuildConfiguration(Configuration.Debug, Platform.AnyCPU));
            buildConfigs.Add(new BuildConfiguration(Configuration.Debug, Platform.MixedPlatforms));
            buildConfigs.Add(new BuildConfiguration(Configuration.Debug, Platform.x86));
            buildConfigs.Add(new BuildConfiguration(Configuration.Release, Platform.AnyCPU));
            buildConfigs.Add(new BuildConfiguration(Configuration.Release, Platform.MixedPlatforms));
            buildConfigs.Add(new BuildConfiguration(Configuration.Release, Platform.x86));

            foreach (BuildConfiguration buildConfig in buildConfigs)
            {
                ProjectBuildConfigurationSet configSet = new ProjectBuildConfigurationSet(buildConfig);
                physicalSolution.ProjectBuildConfigurationsBySolutionBuildConfiguration.Add(configSet.BuildConfiguration, configSet);

                foreach (Guid projectID in physicalSolution.ProjectsByGuid.Keys)
                {
                    ProjectReference projectRef = physicalSolution.ProjectsByGuid[projectID];
                    ProjectType projectType = logicalSolution.ProjectsByPath[projectRef.Name].Info.Type;

                    bool build = true;
                    if ((ProjectType.Console == projectType && Platform.AnyCPU == buildConfig.Platform) || (ProjectType.Library == projectType && Platform.x86 == buildConfig.Platform))
                    {
                        build = false;
                    }
                    ProjectBuildConfigurationInfo buildInfo = new ProjectBuildConfigurationInfo(build, buildConfig);

                    configSet.ProjectBuildConfigurationsByProjectGuid.Add(projectID, buildInfo);
                }
            }
        }

        // Ok.
        private static void AddDefaultProjectBuildConfigurationsNonVs2010(PhysicalSolution physicalSolution, LogicalSolution logicalSolution)
        {
            List<BuildConfiguration> buildConfigs = new List<BuildConfiguration>();
            buildConfigs.Add(new BuildConfiguration(Configuration.Debug, Platform.AnyCPU));
            buildConfigs.Add(new BuildConfiguration(Configuration.Release, Platform.AnyCPU));

            // Hierarchy should be project then build config, but here we assume the correct serialization order will be done by the serializer.
            foreach (BuildConfiguration buildConfig in buildConfigs)
            {
                ProjectBuildConfigurationSet configSet = new ProjectBuildConfigurationSet(buildConfig);
                physicalSolution.ProjectBuildConfigurationsBySolutionBuildConfiguration.Add(configSet.BuildConfiguration, configSet);

                foreach (Guid projectID in physicalSolution.ProjectsByGuid.Keys)
                {
                    ProjectBuildConfigurationInfo buildInfo = new ProjectBuildConfigurationInfo(true, buildConfig);

                    configSet.ProjectBuildConfigurationsByProjectGuid.Add(projectID, buildInfo);
                }
            }
        }

        public static ProjectReference GetReference(PhysicalCSharpProject physicalProject)
        {
            string name = physicalProject.Info.NamesInfo.Name;
            string relativePath = Path.Combine(physicalProject.Info.NamesInfo.DirectoryName, physicalProject.Info.NamesInfo.FileName); // By this point, the project file name is complete with VsVersion and extension.
            Guid guid = physicalProject.Info.GUID;

            ProjectReference output = new ProjectReference(name, relativePath, guid);
            return output;
        }

        private static string CreateProjectFilePath(NewSolutionSpecification solutionSpecification, LogicalSolution logicalSolution, PhysicalCSharpProject physicalProject)
        {
            string solutionDirectoryPath = Creation.CreateSolutionDirectoryPath(solutionSpecification, logicalSolution.Info.NamesInfo);

            string output = Path.Combine(solutionDirectoryPath, physicalProject.Info.NamesInfo.DirectoryName, physicalProject.Info.NamesInfo.FileName);
            return output;
        }

        // Ok.
        private static string CreateSolutionDirectoryPath(NewSolutionSpecification solutionSpecification, SolutionNamesInfo namesInfo)
        {
            OrganizationalInfo orgInfo = solutionSpecification.OrganizationalInfo;
            OrganizationalPaths orgPaths = new OrganizationalPaths(solutionSpecification.OrganizationsDirectoryPath, orgInfo.Organization, orgInfo.Repository, orgInfo.Domain, solutionSpecification.SolutionType.ToDefaultPluralString());

            string output = Path.Combine(orgPaths.SolutionTypeDirectoryPath, namesInfo.DirectoryName);
            return output;
        }

        private static void AddBuildConfigurationInfos(PhysicalCSharpProject physicalProject, VisualStudioVersion visualStudioVersion)
        {
            BuildConfigurationInfo debug = BuildConfigurationInfo.GetDebugDefault(visualStudioVersion);
            BuildConfigurationInfo release = BuildConfigurationInfo.GetReleaseDefault(visualStudioVersion);

            if (VisualStudioVersion.VS2010 == visualStudioVersion && ProjectType.Library != physicalProject.Info.Type)
            {
                debug.BuildConfiguration.Platform = Platform.x86;
                release.BuildConfiguration.Platform = Platform.x86;
            }

            physicalProject.BuildConfigurationInfos.Add(debug.BuildConfiguration, debug);
            physicalProject.BuildConfigurationInfos.Add(release.BuildConfiguration, release);
        }

        private static BuildConfiguration GetDefaultActiveConfiguration(LogicalProject logicalProject, VisualStudioVersion visualStudioVersion)
        {
            BuildConfiguration output;
            if (ProjectType.Library == logicalProject.Info.Type)
            {
                output = new BuildConfiguration(Configuration.Debug, Platform.AnyCPU);
            }
            else
            {
                switch (visualStudioVersion)
                {
                    case VisualStudioVersion.VS2010:
                        output = new BuildConfiguration(Configuration.Debug, Platform.x86);
                        break;

                    default:
                        output = new BuildConfiguration(Configuration.Debug, Platform.AnyCPU);
                        break;
                }
            }

            return output;
        }

        private static NetFrameworkVersion GetDefaultNetFrameworkVersion(VisualStudioVersion visualStudioVersion)
        {
            NetFrameworkVersion output;
            switch (visualStudioVersion)
            {
                case VisualStudioVersion.VS2010:
                    output = NetFrameworkVersion.NetFramework40;
                    break;

                case VisualStudioVersion.VS2013:
                    output = NetFrameworkVersion.NetFramework45;
                    break;

                case VisualStudioVersion.VS2015:
                case VisualStudioVersion.VS2017:
                    output = NetFrameworkVersion.NetFramework452;
                    break;

                default:
                    throw new UnexpectedEnumerationValueException<VisualStudioVersion>(visualStudioVersion);
            }

            return output;
        }

        #endregion

        #region Logical

        // Ok.
        private static void DetermineNamesInfoForProject(ProjectNamesInfo namesInfo, NewProjectSpecification specification)
        {
            string repository = specification.OrganizationalInfo.Repository;
            string domain = specification.OrganizationalInfo.Domain;
            string projectName = specification.ProjectName;
            string fileExtension = ProjectFileLanguageExtensions.ToDefaultString(specification.Language);

            switch (specification.SolutionType)
            {
                case SolutionType.Library:
                    switch (specification.ProjectType)
                    {
                        case ProjectType.Console: // The construction console project.
                            {
                                string libConstruction = String.Format(@"{0}.{1}", projectName, Creation.Construction);
                                string repDomLib = String.Format(@"{0}.{1}.{2}", repository, domain, libConstruction);
                                namesInfo.Name = repDomLib;
                                namesInfo.DirectoryName = Creation.Construction;
                                namesInfo.FileName = String.Format(@"{0}.{1}.{2}", repDomLib, VisualStudioVersionExtensions.ToDefaultString(specification.VisualStudioVersion), fileExtension);
                                namesInfo.RootNamespaceName = repDomLib;
                                namesInfo.AssemblyName = repDomLib;
                            }
                            break;

                        case ProjectType.Library:
                            {
                                string repDomLib = String.Format(@"{0}.{1}.{2}", repository, domain, projectName);
                                namesInfo.Name = repDomLib;
                                namesInfo.DirectoryName = projectName;
                                namesInfo.FileName = String.Format(@"{0}.{1}.{2}", repDomLib, VisualStudioVersionExtensions.ToDefaultString(specification.VisualStudioVersion), fileExtension);
                                namesInfo.RootNamespaceName = repDomLib;
                                namesInfo.AssemblyName = repDomLib;
                            }
                            break;

                        default:
                            throw new UnexpectedEnumerationValueException<ProjectType>(specification.ProjectType);
                    }
                    break;

                default:
                    switch (specification.ProjectType)
                    {
                        case ProjectType.Library: // The support library.
                            string nameWithLib = String.Format(@"{0}.{1}", specification.ProjectName, Creation.Lib);
                            namesInfo.Name = nameWithLib;
                            namesInfo.DirectoryName = Creation.Lib;
                            namesInfo.FileName = String.Format(@"{0}.{1}.{2}", nameWithLib, VisualStudioVersionExtensions.ToDefaultString(specification.VisualStudioVersion), fileExtension);
                            namesInfo.RootNamespaceName = String.Format(@"{0}.{1}.{2}", specification.OrganizationalInfo.Repository, specification.OrganizationalInfo.Domain, nameWithLib);
                            namesInfo.AssemblyName = nameWithLib;
                            break;

                        case ProjectType.Console:
                            namesInfo.Name = specification.ProjectName;
                            namesInfo.DirectoryName = specification.ProjectName;
                            namesInfo.FileName = String.Format(@"{0}.{1}.{2}", specification.ProjectName, VisualStudioVersionExtensions.ToDefaultString(specification.VisualStudioVersion), fileExtension);
                            namesInfo.RootNamespaceName = String.Format(@"{0}.{1}.{2}", specification.OrganizationalInfo.Repository, specification.OrganizationalInfo.Domain, specification.ProjectName);
                            namesInfo.AssemblyName = specification.ProjectName;
                            break;

                        default:
                            throw new UnexpectedEnumerationValueException<ProjectType>(specification.ProjectType);
                    }
                    break;
            }
        }

        private static Dictionary<string, NewProjectSpecification> CreateProjectSpecificationsByMoniker(NewSolutionSpecification solutionSpecification)
        {
            Dictionary<string, NewProjectSpecification> output = new Dictionary<string, NewProjectSpecification>();
            switch (solutionSpecification.SolutionType)
            {
                case SolutionType.Library:
                    NewProjectSpecification constructionConsole = new NewProjectSpecification(solutionSpecification, ProjectType.Console);
                    output.Add(Creation.LibraryConstructionConsoleProjectMoniker, constructionConsole);
                    NewProjectSpecification library = new NewProjectSpecification(solutionSpecification, ProjectType.Library);
                    output.Add(Creation.LibraryProjectMoniker, library);
                    break;

                default:
                    // All others, including Applications, Experiments, Scripts and Web Sites.
                    switch (solutionSpecification.ProjectType)
                    {
                        case ProjectType.Library:
                            throw new InvalidOperationException(@"Cannot create a library project type for a non-library solution type.");

                        case ProjectType.Console:
                            NewProjectSpecification console = new NewProjectSpecification(solutionSpecification, ProjectType.Console);
                            output.Add(Creation.ConsoleProjectMoniker, console);
                            NewProjectSpecification supportLibrary = new NewProjectSpecification(solutionSpecification, ProjectType.Library);
                            output.Add(Creation.ConsoleLibraryProjectMoniker, supportLibrary);
                            break;

                        default:
                            throw new UnexpectedEnumerationValueException<ProjectType>(solutionSpecification.ProjectType);
                    }
                    break;
            }

            return output;
        }

        public static string DetermineSolutionFilePath(string solutionTypeDirectoryPath, SolutionInfo info)
        {
            string output = Path.Combine(solutionTypeDirectoryPath, info.NamesInfo.DirectoryName, info.NamesInfo.DirectoryName);
            return output;
        }

        // Ok.
        /// <summary>
        /// Gets the full solution file path.
        /// </summary>
        public static string DetermineSolutionFilePath(string solutionTypeDirectoryPath, NewSolutionSpecification specification)
        {
            string solutionDirectoryPath = Creation.DetermineSolutionDirectoryPath(solutionTypeDirectoryPath, specification);
            string solutionFileName = Creation.DetermineSolutionFileName(specification);

            string output = Path.Combine(solutionDirectoryPath, solutionFileName);
            return output;
        }

        // Ok.
        public static string DetermineSolutionTypeDirectoryPath(NewSolutionSpecification specification)
        {
            OrganizationalPaths paths = new OrganizationalPaths(
                specification.OrganizationsDirectoryPath,
                specification.OrganizationalInfo.Organization,
                specification.OrganizationalInfo.Repository,
                specification.OrganizationalInfo.Domain,
                specification.SolutionType.ToDefaultPluralString());

            string output = paths.SolutionTypeDirectoryPath;
            return output;
        }

        // Ok.
        public static string DetermineSolutionDirectoryPath(string solutionTypeDirectoryPath, NewSolutionSpecification specification)
        {
            string output = Path.Combine(solutionTypeDirectoryPath, specification.SolutionName);
            return output;
        }

        // Ok.
        /// <summary>
        /// The solution file name includes the VS version and the file extension.
        /// </summary>
        public static string DetermineSolutionFileName(NewSolutionSpecification specification)
        {
            string output = Creation.DetermineSolutionFileName(specification.SolutionName, specification.SolutionType, specification.OrganizationalInfo.Domain, specification.OrganizationalInfo.Repository, specification.VisualStudioVersion);
            return output;
        }

        // Ok.
        public static string DetermineSolutionFileName(string solutionName, SolutionType solutionType, string domain, string repository, VisualStudioVersion visualStudioVersion)
        {
            string logicalSolutionName = Creation.DetermineLogicalSolutionFileName(solutionName, solutionType, domain, repository);
            string vsVersion = VisualStudioVersionExtensions.ToDefaultString(visualStudioVersion);

            string output = String.Format(@"{0}.{1}.{2}", logicalSolutionName, vsVersion, SolutionSerializer.SolutionFileExtension);
            return output;
        }

        /// <summary>
        /// The logical solution name does not include the VS version token, nor the file extension.
        /// </summary>
        public static string DetermineLogicalSolutionFileName(NewSolutionSpecification specification)
        {
            string output = Creation.DetermineLogicalSolutionFileName(specification.SolutionName, specification.SolutionType, specification.OrganizationalInfo.Domain, specification.OrganizationalInfo.Repository);
            return output;
        }

        // Ok.
        public static string DetermineLogicalSolutionFileName(string solutionName, SolutionType solutionType, string domain, string repository)
        {
            // In all names, no Visual Studio token yet, that will be added during logical to physial translation. No file extension either, that will be added during serialization to a file.
            string output;
            if (SolutionType.Library == solutionType)
            {
                // Library solutions have more complicated naming schemes.
                if (CommonDomain.DomainName == domain)
                {
                    if (Creation.Lib == solutionName)
                    {
                        // THE library of THE domain of A repository.
                        output = String.Format(@"{0}.Construction", repository);
                    }
                    else
                    {
                        // A .Lib-namespace expanding library for THE domain of A repository, OR A library for THE domain of A repository.
                        output = String.Format(@"{0}.{1}.Construction", repository, solutionName);
                    }
                }
                else
                {
                    // THE library of A domain of A repository, OR A .Lib-namespace expanding library for A domain of A repository, OR A library for A domain of A repository.
                    output = String.Format(@"{0}.{1}.Construction", domain, solutionName);
                }
            }
            else
            {
                output = solutionName;
            }

            return output;
        }

        #endregion

        #endregion
    }
}
