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
    /// <summary>
    /// A class containing all code for the creation of a new solution.
    /// </summary>
    public class Creation
    {
        public const string Lib = @"Lib";
        public const string Construction = @"Construction";


        #region Static

        #region Create Solution Set

        public static void CreateSolutionSet(string initialSolutionFilePath, IEnumerable<VisualStudioVersion> desiredVsVersions)
        {
            string solutionDirectoryPath = Path.GetDirectoryName(initialSolutionFilePath);
            string solutionFileName = Path.GetFileName(initialSolutionFilePath);
            SolutionFileNameInfo fileNameInfo = SolutionFileNameInfo.Parse(initialSolutionFilePath);

            // Deserialize the solution file to a physical solution object.
            PhysicalSolution initialSolution = SolutionSerializer.Deserialize(initialSolutionFilePath);

            // Determine the paths to each project referenced by the solution object.
            List<string> projectFilePaths = new List<string>();
            foreach(ProjectReference initialProjectReference in initialSolution.ProjectsByGuid.Values)
            {
                string projectFileUnresolvedPath = Path.Combine(solutionDirectoryPath, initialProjectReference.RelativePath);
                string projectFilePath = PathExtensions.GetResolvedPath(projectFileUnresolvedPath);
                projectFilePaths.Add(projectFilePath);
            }

            // Deserialize each project file to a physical project object.
            Dictionary<string, PhysicalCSharpProject> initialProjectsByPath = new Dictionary<string, PhysicalCSharpProject>();
            foreach(string projectFilePath in projectFilePaths)
            {
                PhysicalCSharpProject curInitialProject = CSharpProjectSerializer.Deserialize(projectFilePath);
                initialProjectsByPath.Add(projectFilePath, curInitialProject);
            }

            // Determine the paths of all desired VS version solution files.
            List<Tuple<string, VisualStudioVersion>> solutionFilePathVsVersionPairs = new List<Tuple<string, VisualStudioVersion>>();
            foreach(VisualStudioVersion desiredVsVersion in desiredVsVersions)
            {
                SolutionFileNameInfo curVersion = new SolutionFileNameInfo(fileNameInfo);
                curVersion.VisualStudioVersion = desiredVsVersion;

                string desiredSolutionFileName = SolutionFileNameInfo.Format(curVersion);
                string desiredSolutionFilePath = Path.Combine(solutionDirectoryPath, desiredSolutionFileName);
                solutionFilePathVsVersionPairs.Add(new Tuple<string, VisualStudioVersion>(desiredSolutionFilePath, desiredVsVersion));
            }

            // Check to see if the files exist.
            //  If they do then we are on to the situation of distributing information from files of one version of VS to files of another version of VS.
            //  If they don't, then create these solution files.
            foreach(Tuple<string, VisualStudioVersion> solutionFilePathVsVersionPair in solutionFilePathVsVersionPairs)
            {
                VisualStudioVersion desiredVsVersion = solutionFilePathVsVersionPair.Item2;

                string filePath = solutionFilePathVsVersionPair.Item1;
                if (!File.Exists(filePath))
                {
                    PhysicalSolution curVsVersionSolution = new PhysicalSolution(initialSolution);
                    curVsVersionSolution.VisualStudioVersion = desiredVsVersion;

                    // Now modify all project file paths to have the proper VS Version token.
                    foreach(ProjectReference reference in curVsVersionSolution.ProjectsByGuid.Values)
                    {
                        string relativePath = reference.RelativePath;
                        string relativePathDirectory = Path.GetDirectoryName(relativePath);
                        string relativeFileName = Path.GetFileName(relativePath);

                        ProjectFileNameInfo fileInfo = ProjectFileNameInfo.Parse(relativeFileName, initialSolution.VisualStudioVersion);
                        fileInfo.VisualStudioVersion = desiredVsVersion;

                        string newRelativeFileName = ProjectFileNameInfo.Format(fileInfo);
                        reference.RelativePath = Path.Combine(relativePathDirectory, newRelativeFileName);
                    }

                    SolutionSerializer.Serialize(filePath, curVsVersionSolution);
                }
            }

            // Foreach project, determine the paths of all desired VS version project files.
            Dictionary<string, List<Tuple<string, VisualStudioVersion>>> projectFilePathVsVersionPairsByInitialProjectPath = new Dictionary<string, List<Tuple<string, VisualStudioVersion>>>();
            foreach (string path in initialProjectsByPath.Keys)
            {
                List<Tuple<string, VisualStudioVersion>> projectFilePathVsVersionPairs = new List<Tuple<string, VisualStudioVersion>>();
                projectFilePathVsVersionPairsByInitialProjectPath.Add(path, projectFilePathVsVersionPairs);

                string projectDirectoryPath = Path.GetDirectoryName(path);

                ProjectFileNameInfo projectFileNameInfo = ProjectFileNameInfo.Parse(path, initialSolution.VisualStudioVersion);
                foreach(VisualStudioVersion desiredVsVersion in desiredVsVersions)
                {
                    ProjectFileNameInfo curVersion = new ProjectFileNameInfo(projectFileNameInfo);
                    curVersion.VisualStudioVersion = desiredVsVersion;

                    string desiredProjectFileName = ProjectFileNameInfo.Format(curVersion);
                    string desiredProjectFilePath = Path.Combine(projectDirectoryPath, desiredProjectFileName);

                    projectFilePathVsVersionPairs.Add(new Tuple<string, VisualStudioVersion>(desiredProjectFilePath, desiredVsVersion));
                }
            }

            // Check to see if the files exist.
            //  If they do, then we have the same situation as distributing information from files of one version of VS to files of another version of VS.
            //  If they don't , then create these new project files.
            foreach(string initialProjectPath in projectFilePathVsVersionPairsByInitialProjectPath.Keys)
            {
                PhysicalCSharpProject initialProject = initialProjectsByPath[initialProjectPath];
                List<Tuple<string, VisualStudioVersion>> projectFilePathVsVersionPairs = projectFilePathVsVersionPairsByInitialProjectPath[initialProjectPath];

                foreach(Tuple<string, VisualStudioVersion> projectFilePathVsVersionPair in projectFilePathVsVersionPairs)
                {
                    string desiredProjectPath = projectFilePathVsVersionPair.Item1;
                    VisualStudioVersion desiredVsVersion = projectFilePathVsVersionPair.Item2;

                    PhysicalCSharpProject curVsVersionProject = new PhysicalCSharpProject(initialProject);
                    curVsVersionProject.VisualStudioVersion = desiredVsVersion;

                    // Any changes to the project items required for Visual Studio project translation should be done here.
                    // TODO, app.config or App.config? SHOULD be the same file, or perhaps versioned by Visual Studio version.

                    CSharpProjectSerializer.Serialize(desiredProjectPath, curVsVersionProject);
                }
            }
        }

        public static void CreateSolutionSet(NewSolutionSetSpecification solutionSetSpecification)
        {
            foreach(VisualStudioVersion vsVersion in solutionSetSpecification.VisualStudioVersions)
            {
                NewSolutionSpecification solutionSpecification = new NewSolutionSpecification(solutionSetSpecification.BaseSolutionSpecification);
                solutionSpecification.VisualStudioVersion = vsVersion;

                Creation.CreateSolution(solutionSpecification);
            }
        }

        #endregion

        #region Create Solution

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

        /// <summary>
        /// Constructs a new solution and add all component files (solution, projects, and classes) to the serialization list.
        /// </summary>
        private static LogicalSolution CreateSolution(SerializationList serializationList, NewSolutionSpecification specification)
        {
            string solutionTypeDirectoryPath = Creation.DetermineSolutionTypeDirectoryPath(specification);

            LogicalSolution output = Creation.CreateSolution(serializationList, specification, solutionTypeDirectoryPath);
            return output;
        }

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

        /// <summary>
        /// Creates the solution, adding all sub-components to the serialization list.
        /// </summary>
        private static LogicalSolution CreateLogicalSolution(SerializationList serializationList, NewSolutionSpecification specification, string solutionTypeDirectoryPath)
        {
            LogicalSolution solution = new LogicalSolution();
            solution.Info = Creation.GetSolutionInfo(specification);

            string solutionDirectoryPath = Creation.CreateSolutionDirectoryPath(specification, solution.Info.NamesInfo);

            List<Tuple<NewProjectSpecification, ProjectInfo>> projectSpecifications = Creation.GetProjectSpecifications(solutionDirectoryPath, specification);
            foreach(Tuple<NewProjectSpecification, ProjectInfo> projectSpecification in projectSpecifications)
            {
                LogicalProject project = Creation.CreateProject(serializationList, solutionDirectoryPath, projectSpecification.Item1, projectSpecification.Item2);
                string projectFilePath = Creation.GetProjectFilePath(solutionDirectoryPath, project.Info);
                solution.ProjectsByPath.Add(projectFilePath, project);

                PhysicalCSharpProject physicalProject = Creation.CreatePhysicalCSharpProject(serializationList, specification, project);
                serializationList.AddProject(projectFilePath, physicalProject);
            }

            return solution;
        }

        /// <summary>
        /// Gets project specifications, including any references between projects (which means creating project specifications in a particular order).
        /// </summary>
        private static List<Tuple<NewProjectSpecification, ProjectInfo>> GetProjectSpecifications(string solutionDirectoryPath, NewSolutionSpecification solutionSpecification)
        {
            List<Tuple<NewProjectSpecification, ProjectInfo>> output = new List<Tuple<NewProjectSpecification, ProjectInfo>>();

            NewProjectSpecification librarySpecification = new NewProjectSpecification(solutionSpecification, ProjectType.Library);
            ProjectInfo libraryProjectInfo = Creation.GetProjectInfo(librarySpecification);
            string libraryFileProjectPath = Creation.GetProjectFilePath(solutionDirectoryPath, libraryProjectInfo);

            NewProjectSpecification consoleSpecification = new NewProjectSpecification(solutionSpecification, ProjectType.Console);
            ProjectInfo consolProjectInfo = Creation.GetProjectInfo(consoleSpecification);
            consoleSpecification.ReferencedProjectsByPath.Add(libraryFileProjectPath, libraryProjectInfo);

            // Add the console project first to see if the startup project can be set.
            output.Add(new Tuple<NewProjectSpecification, ProjectInfo>(consoleSpecification, consolProjectInfo));
            output.Add(new Tuple<NewProjectSpecification, ProjectInfo>(librarySpecification, libraryProjectInfo));

            return output;
        }

        private static SolutionInfo GetSolutionInfo(NewSolutionSpecification specification)
        {
            SolutionInfo output = new SolutionInfo();
            output.Type = specification.SolutionType;
            output.NamesInfo.Name = specification.SolutionName;
            output.NamesInfo.DirectoryName = specification.SolutionName;
            output.NamesInfo.FileName = Creation.DetermineSolutionFileName(specification);

            return output;
        }

        /// <summary>
        /// Creates (constructs and serializes) a new project, including all code files.
        /// </summary>
        private static LogicalProject CreateProject(SerializationList serializationList, string solutionDirectoryPath, NewProjectSpecification specification, ProjectInfo info)
        {
            LogicalProject project = new LogicalProject();
            project.Info = info;

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

        private static string GetProjectDirectoryPath(string solutionDirectoryPath, ProjectInfo info)
        {
            string output = Path.Combine(solutionDirectoryPath, info.NamesInfo.DirectoryName);
            return output;
        }

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
            Creation.AddNoneProjectItems(serializationList, projectItems, projectDirectoryPath, info, visualStudioVersion);
        }

        private static void AddNoneProjectItems(SerializationList serializationList, List<ProjectItem> projectItems, string projectDirectoryPath, ProjectInfo projectInfo, VisualStudioVersion visualStudioVersion)
        {
            if (ProjectType.Library != projectInfo.Type)
            {
                ProjectItem appConfigItem = Creation.GetAppConfigProjectItem(serializationList, projectDirectoryPath, visualStudioVersion);
                projectItems.Add(appConfigItem);
            }
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

            ProjectItem output = new NoneProjectItem(fileRelativePath);
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
                        projectItems.Add(projectPlan);
                    }
                    break;

                default:
                    if (ProjectType.Library != info.Type)
                    {
                        ProjectItem projectPlan = Creation.GetProjectPlanProjectItem(serializationList, projectDirectoryPath);
                        projectItems.Add(projectPlan);
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
            EmptyType assemblyInfo = Creation.GetAssemblyInfo(projectName, guid);
            CodeFile assemblyInfoFile = CodeFile.ProcessEmptyType(assemblyInfo);

            string fileRelativePath = @"Properties\AssemblyInfo.cs";
            string filePath = Path.Combine(projectDirectoryPath, fileRelativePath);
            serializationList.AddCodeFile(filePath, assemblyInfoFile);

            ProjectItem output = new CompileProjectItem(fileRelativePath);
            return output;
        }

        public static EmptyType GetAssemblyInfo(string title, Guid guid)
        {
            EmptyType output = new EmptyType(@"AssemblyInfo");
            output.NamespacesUsed.Add(@"System.Reflection");
            output.NamespacesUsed.Add(@"System.Runtime.CompilerServices");
            output.NamespacesUsed.Add(@"System.Runtime.InteropServices");

            output.Lines.Add(
@"// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly."
                );
            output.Lines.Add(String.Format(@"[assembly: AssemblyTitle(""{0}"")]", title));
            output.Lines.Add(
@"[assembly: AssemblyDescription("""")]
[assembly: AssemblyConfiguration("""")]
[assembly: AssemblyCompany("""")]
[assembly: AssemblyProduct("""")]
[assembly: AssemblyCopyright(""Copyright ©  2017"")]
[assembly: AssemblyTrademark("""")]
[assembly: AssemblyCulture("""")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM"
                );
            output.Lines.Add(String.Format(@"[assembly: Guid(""{0}"")]", guid.ToString()));
            output.Lines.Add(
@"
// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion(""1.0.* "")]
[assembly: AssemblyVersion(""1.0.0.0"")]
[assembly: AssemblyFileVersion(""1.0.0.0"")]"
                );

            return output;
        }

        private static ProjectItem GetConsoleProgramProjectItem(SerializationList serializationList, string projectDirectoryPath, string projectRootNamespaceName)
        {
            Class program = Creation.CreateProgram(projectRootNamespaceName);
            CodeFile programFile = CodeFile.ProcessClass(program);

            string fileRelativePath = @"Code\Program.cs";
            string filePath = Path.Combine(projectDirectoryPath, fileRelativePath);
            serializationList.AddCodeFile(filePath, programFile);

            ProjectItem output = new CompileProjectItem(fileRelativePath);
            return output;
        }

        public static Class CreateProgram(string namespaceName)
        {
            Class program = new Class(Logical.Types.ProgramTypeName, namespaceName, Accessibility.Private);

            Method main = Creation.CreateMain();
            program.Methods.Add(main);

            return program;
        }

        private static Method CreateMain()
        {
            MethodArgument args = new MethodArgument(Logical.Types.StringArrayTypeName, @"args");

            Method main = Method.NewStaticMethod(Logical.Methods.MainMethodName, Logical.Types.VoidTypeName, Accessibility.Private, new MethodArgument[] { args });
            return main;
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

        private static ProjectInfo GetProjectInfo(NewProjectSpecification specification)
        {
            ProjectInfo output = new ProjectInfo();
            Creation.DetermineNamesInfoForProject(output.NamesInfo, specification);
            output.GUID = Guid.NewGuid();
            output.Language = specification.Language;
            output.Type = specification.ProjectType;

            return output;
        }

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

                Dictionary<Guid, LogicalProject> logicalProjectsByGuid = new Dictionary<Guid, LogicalProject>();
                foreach(string path in logicalSolution.ProjectsByPath.Keys)
                {
                    LogicalProject logicalProject = logicalSolution.ProjectsByPath[path];
                    logicalProjectsByGuid.Add(logicalProject.Info.GUID, logicalProject);
                }

                foreach (Guid projectID in physicalSolution.ProjectsByGuid.Keys)
                {
                    ProjectType projectType = logicalProjectsByGuid[projectID].Info.Type;

                    bool build = true;
                    if ((ProjectType.Console == projectType && Platform.AnyCPU == buildConfig.Platform) || (ProjectType.Library == projectType && Platform.x86 == buildConfig.Platform))
                    {
                        build = false;
                    }

                    ProjectBuildConfigurationInfo buildInfo;
                    if (ProjectType.Library == projectType)
                    {
                        buildInfo = new ProjectBuildConfigurationInfo(build, new BuildConfiguration(buildConfig.Configuration, Platform.AnyCPU));
                    }
                    else
                    {
                        if(Platform.MixedPlatforms == buildConfig.Platform)
                        {
                            buildInfo = new ProjectBuildConfigurationInfo(build, new BuildConfiguration(buildConfig.Configuration, Platform.x86));
                        }
                        else
                        {
                            buildInfo = new ProjectBuildConfigurationInfo(build, buildConfig);
                        }
                    }

                    configSet.ProjectBuildConfigurationsByProjectGuid.Add(projectID, buildInfo);
                }
            }
        }

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

        private static void DetermineNamesInfoForProject(ProjectNamesInfo namesInfo, NewProjectSpecification specification)
        {
            string repository = specification.OrganizationalInfo.Repository;
            string domain = specification.OrganizationalInfo.Domain;
            string projectName = specification.ProjectName;
            string vsVersion = VisualStudioVersionExtensions.ToDefaultString(specification.VisualStudioVersion);
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
                                namesInfo.Name = String.Format(@"{0}.{1}", repDomLib, vsVersion);
                                namesInfo.DirectoryName = Creation.Construction;
                                namesInfo.FileName = String.Format(@"{0}.{1}.{2}", repDomLib, vsVersion, fileExtension);
                                namesInfo.RootNamespaceName = repDomLib;
                                namesInfo.AssemblyName = repDomLib;
                            }
                            break;

                        case ProjectType.Library:
                            {
                                string repDomLib = String.Format(@"{0}.{1}.{2}", repository, domain, projectName);
                                namesInfo.Name = String.Format(@"{0}.{1}", repDomLib, vsVersion);
                                namesInfo.DirectoryName = projectName;
                                namesInfo.FileName = String.Format(@"{0}.{1}.{2}", repDomLib, vsVersion, fileExtension);
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
                            namesInfo.Name = String.Format(@"{0}.{1}", nameWithLib, vsVersion);
                            namesInfo.DirectoryName = Creation.Lib;
                            namesInfo.FileName = String.Format(@"{0}.{1}.{2}", nameWithLib, vsVersion, fileExtension);
                            namesInfo.RootNamespaceName = String.Format(@"{0}.{1}.{2}", specification.OrganizationalInfo.Repository, specification.OrganizationalInfo.Domain, nameWithLib);
                            namesInfo.AssemblyName = nameWithLib;
                            break;

                        case ProjectType.Console:
                            namesInfo.Name = String.Format(@"{0}.{1}", specification.ProjectName, vsVersion);
                            namesInfo.DirectoryName = specification.ProjectName;
                            namesInfo.FileName = String.Format(@"{0}.{1}.{2}", specification.ProjectName, vsVersion, fileExtension);
                            namesInfo.RootNamespaceName = String.Format(@"{0}.{1}.{2}", specification.OrganizationalInfo.Repository, specification.OrganizationalInfo.Domain, specification.ProjectName);
                            namesInfo.AssemblyName = specification.ProjectName;
                            break;

                        default:
                            throw new UnexpectedEnumerationValueException<ProjectType>(specification.ProjectType);
                    }
                    break;
            }
        }

        public static string DetermineSolutionFilePath(string solutionTypeDirectoryPath, SolutionInfo info)
        {
            string output = Path.Combine(solutionTypeDirectoryPath, info.NamesInfo.DirectoryName, info.NamesInfo.DirectoryName);
            return output;
        }

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

        public static string DetermineSolutionDirectoryPath(string solutionTypeDirectoryPath, NewSolutionSpecification specification)
        {
            string output = Path.Combine(solutionTypeDirectoryPath, specification.SolutionName);
            return output;
        }

        /// <summary>
        /// The solution file name includes the VS version and the file extension.
        /// </summary>
        public static string DetermineSolutionFileName(NewSolutionSpecification specification)
        {
            string output = Creation.DetermineSolutionFileName(specification.SolutionName, specification.SolutionType, specification.OrganizationalInfo.Domain, specification.OrganizationalInfo.Repository, specification.VisualStudioVersion);
            return output;
        }

        public static string DetermineSolutionFileName(string solutionName, SolutionType solutionType, string domain, string repository, VisualStudioVersion visualStudioVersion)
        {
            string logicalSolutionName = Creation.DetermineLogicalSolutionFileName(solutionName, solutionType, domain, repository);
            string vsVersion = VisualStudioVersionExtensions.ToDefaultString(visualStudioVersion);

            string output = String.Format(@"{0}.{1}.{2}", logicalSolutionName, vsVersion, SolutionFileNameInfo.SolutionFileExtension);
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
