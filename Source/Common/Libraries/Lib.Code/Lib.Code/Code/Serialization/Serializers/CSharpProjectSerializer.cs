﻿using System;
using SysConvert = System.Convert;
using System.Collections.Generic;
using System.IO;
using System.Xml;

using Public.Common.Lib.Code.Physical;
using Public.Common.Lib.Code.Physical.CSharp;
using Public.Common.Lib.Extensions;
using Public.Common.Lib.IO.Serialization;


namespace Public.Common.Lib.Code.Serialization
{
    // Notes:
    // The tools version will always mapped with the visual studio version. For VS2010, the product version and schema version will be set in serialization.
    // The app designer folder will always be the default.
    // The file alignment will always be the default.
    // Autogenerate binding redirects will always be true, and is only set during serialization.
    /// <summary>
    /// Allows deserialization/serialization of a Visual Studio C# project to a path. Designed for use in a serialization list.
    /// </summary>
    public class CSharpProjectSerializer : SerializerBase<CSharpProjectSerializationUnit>
    {
        private const string MicrosoftCommonPropsImportKey = @"$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props";
        private const string MicrosoftCSharpTargetsImportKet = @"$(MSBuildToolsPath)\Microsoft.CSharp.targets";
        private const string DefaultAppDesignerFolder = @"Properties";
        private const int DefaultFileAlignment = 512;
        public const string BaseIntermediateOutputPathNodeName = @"BaseIntermediateOutputPath";
        public const string OutputPathNodeName = @"OutputPath";
        public const string MsBuild2003XmlNamespaceName = @"http://schemas.microsoft.com/developer/msbuild/2003";


        #region Static

        public static string GetProjectFilePath(string solutionDirectoryPath, Project project)
        {
            string projectFileExtension = ProjectFileLanguageExtensions.ToDefaultString(project.Info.Language);
            string fileName = String.Format(@"{0}.{1}", project.Info.NamesInfo.Name, projectFileExtension);

            string output = Path.Combine(solutionDirectoryPath, fileName);
            return output;
        }

        public static void Serialize(string filePath, Project project)
        {
            XmlDocument document = CSharpProjectSerializer.CreateDocument();

            XmlElement projectNode = CSharpProjectSerializer.CreateProjectNode(document, project);
            CSharpProjectSerializer.FillProjectNode(projectNode, project);
            CSharpProjectSerializer.ModifyProjectNodeForVsVersion(projectNode, project);

            document.Save(filePath);
        }

        private static void ModifyProjectNodeForVsVersion(XmlElement projectNode, Project project)
        {
            switch (project.VisualStudioVersion)
            {
                case VisualStudioVersion.VS2010:
                    {
                        XmlNode platformNode = projectNode.SelectSingleNode("Import[@Project='$(MSBuildExtensionsPath)\\$(MSBuildToolsVersion)\\Microsoft.Common.props']");
                        projectNode.RemoveChild(platformNode);
                    }
                    break;

                case VisualStudioVersion.VS2017:
                    {
                        projectNode.Attributes.RemoveNamedItem("DefaultTargets");
                    }
                    break;

                default:
                    break; // Do nothing.
            }
        }

        private static void FillProjectNode(XmlElement projectNode, Project project)
        {
            CSharpProjectSerializer.CreateMicrosoftCommonPropsImport(projectNode, project);

            XmlElement projectPropertyGroupNode = CSharpProjectSerializer.CreateProjectPropertyGroup(projectNode, project);
            CSharpProjectSerializer.ModifyProjectPropertyGroupForVsVersion(projectPropertyGroupNode, project);

            CSharpProjectSerializer.CreateBuildConfigurationInfoNodes(projectNode, project);

            ItemGroups groups = new ItemGroups(project.ProjectItemsByRelativePath);
            CSharpProjectSerializer.CreateReferenceItemGroup(projectNode, groups.References);
            CSharpProjectSerializer.CreateCOMReferenceItemGroup(projectNode, groups.COMReferences, project.VisualStudioVersion);
            CSharpProjectSerializer.CreateCompileItemGroup(projectNode, groups.Compiles);
            CSharpProjectSerializer.CreateEmbeddedResourceItemGroup(projectNode, groups.Embeddeds);
            CSharpProjectSerializer.CreateProjectReferenceItemGroup(projectNode, groups.ProjectReferences);
            CSharpProjectSerializer.CreateContentItemGroup(projectNode, groups.Contents);
            CSharpProjectSerializer.CreateFolderItemGroup(projectNode, groups.Folders);
            CSharpProjectSerializer.CreateNoneItemGroup(projectNode, groups.Nones);

            CSharpProjectSerializer.CreateMicrosoftCSharpTargetsImport(projectNode, project);

            CSharpProjectSerializer.AddTrailingImports(projectNode, project);

            CSharpProjectSerializer.AddMiscellaneousEndComment(projectNode, project);
        }

        private static void AddMiscellaneousEndComment(XmlElement projectNode, Project project)
        {
            if (VisualStudioVersion.VS2017 == project.VisualStudioVersion)
            {
                return;
            }

            string commentStr =
@" To modify your build process, add your task inside one of the targets below and uncomment it. 
       Other similar extension points exist, see Microsoft.Common.targets.
  <Target Name=""BeforeBuild"">
  </Target>
  <Target Name=""AfterBuild"">
  </Target>
  ";
            XmlComment comment = projectNode.OwnerDocument.CreateComment(commentStr);
            projectNode.AppendChild(comment);
        }

        private static void AddTrailingImports(XmlElement projectNode, Project project)
        {
            foreach (string importPath in project.Imports.Keys)
            {
                if(importPath != CSharpProjectSerializer.MicrosoftCommonPropsImportKey && importPath != CSharpProjectSerializer.MicrosoftCSharpTargetsImportKet)
                {
                    Import import = project.Imports[importPath];
                    CSharpProjectSerializer.CreateImportNode(projectNode, import);
                }
            }
        }

        private static void CreateMicrosoftCSharpTargetsImport(XmlElement projectNode, Project project)
        {
            Import import;
            if (project.Imports.ContainsKey(CSharpProjectSerializer.MicrosoftCSharpTargetsImportKet))
            {
                import = project.Imports[CSharpProjectSerializer.MicrosoftCSharpTargetsImportKet];
            }
            else
            {
                import = Import.MicrosoftCSharpTargets;
            }

            CSharpProjectSerializer.CreateImportNode(projectNode, import);
        }

        private static void CreateFolderItemGroup(XmlNode projectNode, List<FolderProjectItem> folders)
        {
            if(0 < folders.Count)
            {
                XmlElement itemGroupNode = projectNode.OwnerDocument.CreateElement("ItemGroup");
                projectNode.AppendChild(itemGroupNode);

                foreach (FolderProjectItem folder in folders)
                {
                    XmlElement folderNode = XmlHelper.CreateElement(itemGroupNode, "Folder", new Tuple<string, string>[] { new Tuple<string, string>("Include", folder.IncludePath) });
                    itemGroupNode.AppendChild(folderNode);
                }
            }
        }

        private static void CreateContentItemGroup(XmlElement projectNode, List<ContentProjectItem> contents)
        {
            if(0 < contents.Count)
            {
                XmlElement itemGroupNode = projectNode.OwnerDocument.CreateElement("ItemGroup");
                projectNode.AppendChild(itemGroupNode);

                foreach (ContentProjectItem content in contents)
                {
                    CSharpProjectSerializer.CreateContentReference(itemGroupNode, content);
                }
            }
        }

        private static void CreateContentReference(XmlElement itemGroupNode, ContentProjectItem content)
        {
            XmlElement contentReferenceNode = XmlHelper.CreateElement(itemGroupNode, "Content", new Tuple<string, string>[] { new Tuple<string, string>("Include", content.IncludePath) });
            itemGroupNode.AppendChild(contentReferenceNode);

            if(CopyToOutputDirectory.Never != content.CopyToOutputDirectory)
            {
                XmlHelper.AddChildElement(contentReferenceNode, "CopyToOutputDirectory", content.CopyToOutputDirectory.ToDefaultString());
            }
        }

        private static void CreateProjectReferenceItemGroup(XmlElement projectNode, List<ProjectReferenceProjectItem> projectReferences)
        {
            if(0 < projectReferences.Count)
            {
                XmlElement itemGroupNode = projectNode.OwnerDocument.CreateElement("ItemGroup");
                projectNode.AppendChild(itemGroupNode);

                foreach(ProjectReferenceProjectItem projectReference in projectReferences)
                {
                    CSharpProjectSerializer.CreateProjectReference(itemGroupNode, projectReference);
                }
            }
        }

        private static void CreateProjectReference(XmlElement itemGroupNode, ProjectReferenceProjectItem projectReference)
        {
            XmlElement projectReferenceNode = XmlHelper.CreateElement(itemGroupNode, "ProjectReference", new Tuple<string, string>[] { new Tuple<string, string>("Include", projectReference.IncludePath) });
            itemGroupNode.AppendChild(projectReferenceNode);

            XmlHelper.AddChildElement(projectReferenceNode, "Project", String.Format(@"{{{0}}}", projectReference.GUID.ToString().ToUpperInvariant()));
            XmlHelper.AddChildElement(projectReferenceNode, "Name", projectReference.Name);
        }

        private static void CreateNoneItemGroup(XmlElement projectNode, List<NoneProjectItem> nones)
        {
            if (0 < nones.Count)
            {
                XmlElement itemGroupNode = projectNode.OwnerDocument.CreateElement("ItemGroup");
                projectNode.AppendChild(itemGroupNode);

                foreach (NoneProjectItem none in nones)
                {
                    XmlNode noneNode = XmlHelper.CreateElement(itemGroupNode, "None", new Tuple<string, string>[] { new Tuple<string, string>("Include", none.IncludePath) });
                    itemGroupNode.AppendChild(noneNode);

                    if(!String.IsNullOrEmpty(none.Generator))
                    {
                        XmlHelper.AddChildElement(noneNode, "Generator", none.Generator);
                    }

                    if (!String.IsNullOrEmpty(none.LastGenOutput))
                    {
                        XmlHelper.AddChildElement(noneNode, "LastGenOutput", none.LastGenOutput);
                    }

                    if (CopyToOutputDirectory.Blank != none.CopyToOutputDirectory)
                    {
                        XmlHelper.AddChildElement(noneNode, "CopyToOutputDirectory", none.CopyToOutputDirectory.ToDefaultString());
                    }
                }
            }
        }

        private static void CreateEmbeddedResourceItemGroup(XmlElement projectNode, List<EmbededResourceProjectItem> embeddeds)
        {
            XmlElement itemGroupNode = projectNode.OwnerDocument.CreateElement("ItemGroup");
            projectNode.AppendChild(itemGroupNode);

            foreach (EmbededResourceProjectItem embedded in embeddeds)
            {
                XmlNode embeddedNode = XmlHelper.CreateElement(itemGroupNode, "EmbeddedResource", new Tuple<string, string>[] { new Tuple<string, string>("Include", embedded.IncludePath) });
                itemGroupNode.AppendChild(embeddedNode);

                if (String.IsNullOrEmpty(embedded.Generator))
                {
                    XmlHelper.AddChildElement(embeddedNode, "Generator", embedded.Generator);
                }

                if (String.IsNullOrEmpty(embedded.LastGenOutput))
                {
                    XmlHelper.AddChildElement(embeddedNode, "LastGenOutput", embedded.LastGenOutput);
                }

                if (String.IsNullOrEmpty(embedded.SubType))
                {
                    XmlHelper.AddChildElement(embeddedNode, "SubType", embedded.SubType);
                }
            }
        }

        private static void CreateCompileItemGroup(XmlElement projectNode, List<CompileProjectItem> compiles)
        {
            XmlElement itemGroupNode = projectNode.OwnerDocument.CreateElement("ItemGroup");
            projectNode.AppendChild(itemGroupNode);

            foreach (CompileProjectItem compile in compiles)
            {
                XmlNode compileNode = XmlHelper.CreateElement(itemGroupNode, "Compile", new Tuple<string, string>[] { new Tuple<string, string>("Include", compile.IncludePath) });
                itemGroupNode.AppendChild(compileNode);

                if(compile.AutoGen)
                {
                    XmlHelper.AddChildElement(compileNode, "AutoGen", compile.AutoGen.ToString());
                }

                if(!String.IsNullOrEmpty(compile.DependentUpon))
                {
                    XmlHelper.AddChildElement(compileNode, "DependentUpon", compile.DependentUpon);
                }

                if (compile.DesignTime)
                {
                    XmlHelper.AddChildElement(compileNode, "DesignTime", compile.DesignTime.ToString());
                }

                if (compile.DesignTimeSharedInput)
                {
                    XmlHelper.AddChildElement(compileNode, "DesignTimeSharedInput", compile.DesignTimeSharedInput.ToString());
                }
            }
        }

        private static void CreateCOMReferenceItemGroup(XmlElement projectNode, List<COMReferenceProjectItem> comReferences, VisualStudioVersion visualStudioVersion)
        {
            XmlElement itemGroupNode = projectNode.OwnerDocument.CreateElement("ItemGroup");
            projectNode.AppendChild(itemGroupNode);

            foreach (COMReferenceProjectItem comReference in comReferences)
            {
                XmlNode referenceNode = XmlHelper.CreateElement(itemGroupNode, "COMReference", new Tuple<string, string>[] { new Tuple<string, string>("Include", comReference.IncludePath) });
                itemGroupNode.AppendChild(referenceNode);

                string guidUpperStr = comReference.GUID.ToString().ToUpperInvariant();
                string guidStr;
                if (VisualStudioVersion.VS2010 == visualStudioVersion || VisualStudioVersion.VS2013 == visualStudioVersion)
                {
                    guidStr = String.Format(@"{{{0}}}", guidUpperStr);
                }
                else
                {
                    guidStr = guidUpperStr;
                }

                XmlHelper.AddChildElement(referenceNode, "Guid", guidStr);
                XmlHelper.AddChildElement(referenceNode, "VersionMajor", comReference.VersionMajor.ToString());
                XmlHelper.AddChildElement(referenceNode, "VersionMinor", comReference.VersionMinor.ToString());
                XmlHelper.AddChildElement(referenceNode, "Lcid", comReference.LCID.ToString());
                XmlHelper.AddChildElement(referenceNode, "WrapperTool", comReference.WrapperTool);
                XmlHelper.AddChildElement(referenceNode, "Isolated", comReference.Isolated.ToString());
                XmlHelper.AddChildElement(referenceNode, "EmbedInteropTypes", comReference.EmbedInteropTypes.ToString());
            }
        }

        private static void CreateReferenceItemGroup(XmlElement projectNode, List<ReferenceProjectItem> references)
        {
            XmlElement itemGroupNode = projectNode.OwnerDocument.CreateElement("ItemGroup");
            projectNode.AppendChild(itemGroupNode);

            foreach(ReferenceProjectItem assemblyReference in references)
            {
                XmlNode referenceNode = XmlHelper.CreateElement(itemGroupNode, "Reference", new Tuple<string, string>[] { new Tuple<string, string>("Include", assemblyReference.IncludePath) });
                itemGroupNode.AppendChild(referenceNode);

                if(assemblyReference.EmbedInteropTypes)
                {
                    XmlHelper.AddChildElement(referenceNode, "EmbedInteropTypes", assemblyReference.EmbedInteropTypes.ToString());
                }
            }
        }

        private static void CreateBuildConfigurationInfoNodes(XmlElement projectNode, Project project)
        {
            foreach(BuildConfiguration config in project.BuildConfigurationInfos.Keys)
            {
                BuildConfigurationInfo info = project.BuildConfigurationInfos[config];
                CSharpProjectSerializer.CreateBuildConfigurationInfoNode(projectNode, project, config, info);
            }
        }

        private static void CreateBuildConfigurationInfoNode(XmlElement projectNode, Project project, BuildConfiguration config, BuildConfigurationInfo info)
        {
            string conditionValue = String.Format(@" '$(Configuration)|$(Platform)' == '{0}|{1}' ", config.Configuration.ToDefaultString(), config.Platform.ToDefaultString());

            XmlElement groupNode = XmlHelper.CreateElement(projectNode, "PropertyGroup", new Tuple<string, string>[] { new Tuple<string, string>("Condition", conditionValue) });
            projectNode.AppendChild(groupNode);

            if (ProjectOutputType.Exe == project.OutputType)
            {
                XmlHelper.AddChildElement(groupNode, "PlatformTarget", info.BuildConfiguration.Platform.ToDefaultString());
            }
            if (info.DebugSymbols)
            {
                XmlHelper.AddChildElement(groupNode, "DebugSymbols", info.DebugSymbols.ToStringLower());
            }
            XmlHelper.AddChildElement(groupNode, "DebugType", info.DebugType.ToDefaultString());
            XmlHelper.AddChildElement(groupNode, "Optimize", info.Optimize.ToStringLower());
            XmlHelper.AddChildElement(groupNode, "OutputPath", info.BinOutputPath);
            if (!String.IsNullOrEmpty(info.ObjIntermediatePath))
            {
                XmlHelper.AddChildElement(groupNode, "BaseIntermediateOutputPath", info.ObjIntermediatePath);
            }
            XmlHelper.AddChildElement(groupNode, "DefineConstants", StringExtensions.Concatenate(info.DefinedConstants.ToArray(), @";"));
            XmlHelper.AddChildElement(groupNode, "ErrorReport", BuildConfigurationInfo.DefaultErrorReport);
            XmlHelper.AddChildElement(groupNode, "WarningLevel", BuildConfigurationInfo.DefaultWarningLevel.ToString());
            if (info.AllowUnsafeBlocks)
            {
                XmlHelper.AddChildElement(groupNode, "AllowUnsafeBlocks", info.AllowUnsafeBlocks.ToStringLower());
            }
        }

        private static void ModifyProjectPropertyGroupForVsVersion(XmlElement projectPropertyGroupNode, Project project)
        {
            switch(project.VisualStudioVersion)
            {
                case VisualStudioVersion.VS2010:
                    {
                        XmlNode platformNode = projectPropertyGroupNode.SelectSingleNode("Platform");
                        XmlElement productVersionNode = XmlHelper.CreateElement(projectPropertyGroupNode, "ProductVersion", @"8.0.30703");
                        projectPropertyGroupNode.InsertAfter(productVersionNode, platformNode);
                        XmlElement schemaVersionNode = XmlHelper.CreateElement(projectPropertyGroupNode, "SchemaVersion", @"2.0");
                        projectPropertyGroupNode.InsertAfter(schemaVersionNode, productVersionNode);
                    }
                    break;

                case VisualStudioVersion.VS2017:
                    {
                        if (ProjectOutputType.Exe == project.OutputType)
                        {
                            XmlNode appDesignerFolderNode = projectPropertyGroupNode.SelectSingleNode("AppDesignerFolder");
                            projectPropertyGroupNode.RemoveChild(appDesignerFolderNode);
                        }
                    }
                    break;


                default:
                    break; // Do nothing.
            }
        }

        private static XmlElement CreateProjectPropertyGroup(XmlElement projectNode, Project project)
        {
            XmlElement groupNode = projectNode.OwnerDocument.CreateElement("PropertyGroup");
            projectNode.AppendChild(groupNode);

            XmlHelper.AddChildElement(groupNode, "Configuration", project.ActiveConfiguration.Configuration.ToDefaultString(), new Tuple<string, string>[] { new Tuple<string, string>("Condition", @" '$(Configuration)' == '' ") });
            XmlHelper.AddChildElement(groupNode, "Platform", project.ActiveConfiguration.Platform.ToDefaultString(), new Tuple<string, string>[] { new Tuple<string, string>("Condition", @" '$(Platform)' == '' ") });
            XmlHelper.AddChildElement(groupNode, "ProjectGuid", String.Format(@"{{{0}}}", project.Info.GUID.ToString().ToUpperInvariant()));
            XmlHelper.AddChildElement(groupNode, "OutputType", project.OutputType.ToDefaultString());
            XmlHelper.AddChildElement(groupNode, "AppDesignerFolder", CSharpProjectSerializer.DefaultAppDesignerFolder);
            XmlHelper.AddChildElement(groupNode, "RootNamespace", project.Info.NamesInfo.RootNamespaceName);
            XmlHelper.AddChildElement(groupNode, "AssemblyName", project.Info.NamesInfo.AssemblyName);
            XmlHelper.AddChildElement(groupNode, "TargetFrameworkVersion", NetFrameworkVersionVFormatExtensions.ToDefaultString(project.TargetFrameworkVersion));
            XmlHelper.AddChildElement(groupNode, "FileAlignment", CSharpProjectSerializer.DefaultFileAlignment.ToString());

            int vsVersion = VisualStudioVersionToolsVersionExtensions.ToDefaultInt(project.VisualStudioVersion);
            if (VisualStudioVersionToolsVersionExtensions.VS2013 < vsVersion && ProjectOutputType.Exe == project.OutputType)
            {
                XmlHelper.AddChildElement(groupNode, "AutoGenerateBindingRedirects", Boolean.TrueString.ToLowerInvariant());
            }

            return groupNode;
        }

        private static void CreateMicrosoftCommonPropsImport(XmlElement projectNode, Project project)
        {
            Import import;
            if(project.Imports.ContainsKey(CSharpProjectSerializer.MicrosoftCommonPropsImportKey))
            {
                import = project.Imports[CSharpProjectSerializer.MicrosoftCommonPropsImportKey];
            }
            else
            {
                import = Import.MicrosoftCommonProps;
            }

            CSharpProjectSerializer.CreateImportNode(projectNode, import);
        }

        private static XmlElement CreateImportNode(XmlElement projectNode, Import import)
        {
            List<Tuple<string, string>> attributes = new List<Tuple<string, string>>();
            attributes.Add(new Tuple<string, string>("Project", import.ProjectPath));
            if (!String.IsNullOrEmpty(import.Condition))
            {
                attributes.Add(new Tuple<string, string>("Condition", import.Condition));
            }

            XmlElement importNode = XmlHelper.CreateElement(projectNode, "Import", attributes);
            projectNode.AppendChild(importNode);

            return importNode;
        }

        private static XmlElement CreateProjectNode(XmlDocument document, Project project)
        {
            int toolsVersionMajor = VisualStudioVersionToolsVersionExtensions.ToDefaultInt(project.VisualStudioVersion);
            Version toolsVersion = new Version(toolsVersionMajor, 0);
            string toolsVersionStr = toolsVersion.ToString();

            Tuple<string, string>[] attributes = new Tuple<string, string>[]
            {
                new Tuple<string, string>("ToolsVersion", toolsVersionStr),
                new Tuple<string, string>("DefaultTargets", "Build"),
                new Tuple<string, string>("xmlns", CSharpProjectSerializer.MsBuild2003XmlNamespaceName),
            };

            XmlElement projectNode = XmlHelper.CreateElement(document, "Project", attributes);
            document.AppendChild(projectNode);

            return projectNode;
        }

        private static XmlDocument CreateDocument()
        {
            XmlDocument output = new XmlDocument();

            XmlDeclaration declaration = output.CreateXmlDeclaration(@"1.0", @"utf-8", String.Empty);
            output.AppendChild(declaration);

            return output;
        }

        public static Project Deserialize(string filePath)
        {
            // Disable namespaces so node lookup is simple and intuitive.
            XmlDocument doc = new XmlDocument();
            doc.LoadNoNamespaces(filePath);

            Project output = new Project();
            output.Info.NamesInfo.Name = Path.GetFileNameWithoutExtension(filePath);
            output.Info.NamesInfo.FileName = Path.GetFileName(filePath);

            string fullFileDirectoryPath = Path.GetDirectoryName(filePath);
            output.Info.NamesInfo.DirectoryName = Path.GetFileName(fullFileDirectoryPath); // Treats directory as file.

            string fileExtension = PathExtensions.GetExtensionOnly(filePath);
            output.Info.Language = ProjectFileLanguageExtensions.FromDefault(fileExtension);

            foreach (XmlNode child in doc.ChildNodes)
            {
                if ("Project" == child.Name)
                {
                    CSharpProjectSerializer.DeserializeProjectXmlNode(output, child);

                    break;
                }
            }

            return output;
        }

        private static void DeserializeProjectXmlNode(Project project, XmlNode node)
        {
            string toolsVersionStr = node.Attributes["ToolsVersion"].Value;
            Version toolsVersion = new Version(toolsVersionStr);
            project.VisualStudioVersion = VisualStudioVersionToolsVersionExtensions.FromDefault(toolsVersion.Major);

            // No need to deserialize default targets and msbuild xmlns.

            foreach(XmlNode child in node.ChildNodes)
            {
                switch(child.Name)
                {
                    case @"Import":
                        CSharpProjectSerializer.DeserializeImportNode(project, child);
                        break;

                    case @"PropertyGroup":
                        CSharpProjectSerializer.DeserializePropertyGroupNode(project, child);
                        break;

                    case @"ItemGroup":
                        CSharpProjectSerializer.DeserializeItemGroup(project, child);
                        break;

                    default:
                        CSharpProjectSerializer.DeserializeOther(project, child);
                        break;
                }
            }
        }

        private static void DeserializeOther(Project project, XmlNode node)
        {
            if(XmlNodeType.Comment != node.NodeType)
            {
                throw new Exception(@"Unknown project child node found.");
            }
        }

        private static void DeserializeItemGroup(Project project, XmlNode node)
        {
            foreach(XmlNode child in node.ChildNodes)
            {
                switch(child.Name)
                {
                    case @"Reference":
                        CSharpProjectSerializer.DeserializeReference(project, child);
                        break;

                    case @"COMReference":
                        CSharpProjectSerializer.DeserializeCOMReference(project, child);
                        break;

                    case @"ProjectReference":
                        CSharpProjectSerializer.DeserializeProjectReference(project, child);
                        break;

                    case @"Compile":
                        CSharpProjectSerializer.DeserializeCompile(project, child);
                        break;

                    case @"Content":
                        CSharpProjectSerializer.DeserializeContent(project, child);
                        break;

                    case @"Folder":
                        CSharpProjectSerializer.DeserializeFolder(project, child);
                        break;

                    case @"EmbeddedResource":
                        CSharpProjectSerializer.DeserializeEmbeddedResource(project, child);
                        break;

                    case @"None":
                        CSharpProjectSerializer.DeserializeNone(project, child);
                        break;

                    default:
                        throw new Exception(@"Unknown item group node found.");
                }
            }
        }

        private static void DeserializeEmbeddedResource(Project project, XmlNode node)
        {
            string includePath = node.Attributes["Include"].Value;
            EmbededResourceProjectItem embedded = new EmbededResourceProjectItem(includePath);

            XmlNode generatorNode = node.SelectSingleNode("Generator");
            if (null != generatorNode)
            {
                embedded.Generator = generatorNode.InnerText;
            }

            XmlNode lastGenOutputNode = node.SelectSingleNode("LastGenOutput");
            if (null != lastGenOutputNode)
            {
                embedded.LastGenOutput = lastGenOutputNode.InnerText;
            }

            XmlNode subTypeNode = node.SelectSingleNode("SubType");
            if (null != subTypeNode)
            {
                embedded.SubType = subTypeNode.InnerText;
            }

            project.ProjectItemsByRelativePath.Add(embedded.IncludePath, embedded);
        }

        private static void DeserializeNone(Project project, XmlNode node)
        {
            string includePath = node.Attributes["Include"].Value;
            NoneProjectItem none = new NoneProjectItem(includePath);

            XmlNode generatorNode = node.SelectSingleNode("Generator");
            if(null != generatorNode)
            {
                none.Generator = generatorNode.InnerText;
            }

            XmlNode lastGenOutputNode = node.SelectSingleNode("LastGenOutput");
            if (null != lastGenOutputNode)
            {
                none.LastGenOutput = lastGenOutputNode.InnerText;
            }

            XmlNode copyToOutputDirectoryNode = node.SelectSingleNode("CopyToOutputDirectory");
            if (null != copyToOutputDirectoryNode)
            {
                none.CopyToOutputDirectory = CopyToOutputDirectoryExtensions.FromDefault(copyToOutputDirectoryNode.InnerText);
            }

            project.ProjectItemsByRelativePath.Add(none.IncludePath, none);
        }

        private static void DeserializeFolder(Project project, XmlNode node)
        {
            string relativePath = node.Attributes["Include"].Value;
            FolderProjectItem folder = new FolderProjectItem(relativePath);

            project.ProjectItemsByRelativePath.Add(folder.IncludePath, folder);
        }

        private static void DeserializeContent(Project project, XmlNode node)
        {
            string relativePath = node.Attributes["Include"].Value;
            ContentProjectItem content = new ContentProjectItem(relativePath);

            XmlNode copyToOutputDirectoryNode = node.SelectSingleNode("CopyToOutputDirectory");
            if(null != copyToOutputDirectoryNode)
            {
                string copyToOutputDirectoryStr = copyToOutputDirectoryNode.InnerText;
                content.CopyToOutputDirectory = CopyToOutputDirectoryExtensions.FromDefault(copyToOutputDirectoryStr);
            }

            project.ProjectItemsByRelativePath.Add(content.IncludePath, content);
        }

        private static void DeserializeProjectReference(Project project, XmlNode node)
        {
            string relativePath = node.Attributes["Include"].Value;

            string guidStr = node.SelectSingleNode("Project").InnerText;
            Guid guid = Guid.Parse(guidStr);

            string name = node.SelectSingleNode("Name").InnerText;

            ProjectReferenceProjectItem reference = new ProjectReferenceProjectItem(relativePath, guid, name);
            project.ProjectItemsByRelativePath.Add(reference.Name, reference);
        }

        private static void DeserializeCompile(Project project, XmlNode node)
        {
            string relativePath = node.Attributes["Include"].Value;
            CompileProjectItem compile = new CompileProjectItem(relativePath);

            XmlNode autoGenNode = node.SelectSingleNode("AutoGen");
            if(null != autoGenNode)
            {
                compile.AutoGen = Boolean.Parse(autoGenNode.InnerText);
            }

            XmlNode dependentUponNode = node.SelectSingleNode("DependentUpon");
            if (null != dependentUponNode)
            {
                compile.DependentUpon = dependentUponNode.InnerText;
            }

            XmlNode designTimeNode = node.SelectSingleNode("DesignTime");
            if (null != designTimeNode)
            {
                compile.DesignTime = Boolean.Parse(designTimeNode.InnerText);
            }

            XmlNode designTimeSharedInputNode = node.SelectSingleNode("DesignTimeSharedInput");
            if (null != designTimeSharedInputNode)
            {
                compile.DesignTimeSharedInput = Boolean.Parse(designTimeSharedInputNode.InnerText);
            }

            project.ProjectItemsByRelativePath.Add(compile.IncludePath, compile);
        }

        private static void DeserializeCOMReference(Project project, XmlNode node)
        {
            string assemblyName = node.Attributes["Include"].Value;
            COMReferenceProjectItem reference = new COMReferenceProjectItem(assemblyName);

            XmlNode guidNode = node.SelectSingleNode("Guid");
            reference.GUID = Guid.Parse(guidNode.InnerText);

            XmlNode versionMajorNode = node.SelectSingleNode("VersionMajor");
            reference.VersionMajor = Int32.Parse(versionMajorNode.InnerText);

            XmlNode versionMinorNode = node.SelectSingleNode("VersionMinor");
            reference.VersionMinor = Int32.Parse(versionMinorNode.InnerText);

            XmlNode lcidNode = node.SelectSingleNode("Lcid");
            reference.LCID = Int32.Parse(lcidNode.InnerText);

            XmlNode wrapperToolNode = node.SelectSingleNode("WrapperTool");
            reference.WrapperTool = wrapperToolNode.InnerText;

            XmlNode isolatedNode = node.SelectSingleNode("Isolated");
            reference.Isolated = Boolean.Parse(isolatedNode.InnerText);

            XmlNode embedInteropTypes = node.SelectSingleNode("EmbedInteropTypes");
            reference.EmbedInteropTypes = Boolean.Parse(embedInteropTypes.InnerText);

            project.ProjectItemsByRelativePath.Add(reference.IncludePath, reference);
        }

        private static void DeserializeReference(Project project, XmlNode referenceNode)
        {
            string assemblyName = referenceNode.Attributes["Include"].Value;
            ReferenceProjectItem reference = new ReferenceProjectItem(assemblyName);

            XmlNode embedInteropTypes = referenceNode.SelectSingleNode("EmbedInteropTypes");
            if(null != embedInteropTypes)
            {
                bool value = Boolean.Parse(embedInteropTypes.InnerText);
                reference.EmbedInteropTypes = value;
            }

            project.ProjectItemsByRelativePath.Add(reference.IncludePath, reference); // OK to treat assembly name like a relative path.
        }

        private static void DeserializePropertyGroupNode(Project project, XmlNode node)
        {
            if(1 > node.Attributes.Count)
            {
                // The project property group.
                CSharpProjectSerializer.DeserializeProjectPropertyGroupNode(project, node);
            }
            else
            {
                // A build configuration info property group.
                CSharpProjectSerializer.DeserializeConfigurationPropertyGroupNode(project, node);
            }
        }

        private static void DeserializeConfigurationPropertyGroupNode(Project project, XmlNode node)
        {
            BuildConfigurationInfo configInfo = new BuildConfigurationInfo();

            string conditionStr = node.Attributes["Condition"].InnerText;
            BuildConfiguration config = CSharpProjectSerializer.ParseBuildConfiguration(conditionStr);
            configInfo.BuildConfiguration = config;

            project.BuildConfigurationInfos.Add(configInfo.BuildConfiguration, configInfo);

            XmlNode debugSymbolsNode = node.SelectSingleNode("DebugSymbols");
            if(null != debugSymbolsNode)
            {
                string debugSymbolsStr = debugSymbolsNode.InnerText;
                configInfo.DebugSymbols = Boolean.Parse(debugSymbolsStr);
            }

            string debugTypeStr = node.SelectSingleNode("DebugType").InnerText;
            configInfo.DebugType = DebugTypeExtensions.FromDefault(debugTypeStr);

            string optimizeStr = node.SelectSingleNode("Optimize").InnerText;
            configInfo.Optimize = Boolean.Parse(optimizeStr);

            configInfo.BinOutputPath = node.SelectSingleNode("OutputPath").InnerText;

            XmlNode baseIntermediatePathNode = node.SelectSingleNode("BaseIntermediateOutputPath");
            if(null != baseIntermediatePathNode)
            {
                configInfo.ObjIntermediatePath = baseIntermediatePathNode.InnerText;
            }

            string defineConstantsStr = node.SelectSingleNode("DefineConstants").InnerText;
            string[] defineConstantsArr = defineConstantsStr.Split(';');
            configInfo.DefinedConstants.AddRange(defineConstantsArr);

            XmlNode allowUnsafeBlocksNode = node.SelectSingleNode("AllowUnsafeBlocks");
            if (null != allowUnsafeBlocksNode)
            {
                configInfo.AllowUnsafeBlocks = SysConvert.ToBoolean(allowUnsafeBlocksNode.InnerText);
            }
        }

        private static BuildConfiguration ParseBuildConfiguration(string condition)
        {
            string[] conditionToken = condition.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string buildTokenUntrimmed = conditionToken[conditionToken.Length - 1];
            string buildToken = buildTokenUntrimmed.Trim('\'');

            string[] buildTokens = buildToken.Split('|');
            string configurationStr = buildTokens[0];
            string platformStr = buildTokens[1];

            Configuration configuration = ConfigurationExtensions.FromDefault(configurationStr);
            Platform platform = PlatformExtensions.FromDefault(platformStr);

            BuildConfiguration output = new BuildConfiguration(configuration, platform);
            return output;
        }

        private static void DeserializeProjectPropertyGroupNode(Project project, XmlNode node)
        {
            string configurationStr = node.SelectSingleNode("Configuration").InnerText;
            Configuration configuration = ConfigurationExtensions.FromDefault(configurationStr);

            string platformStr = node.SelectSingleNode("Platform").InnerText;
            Platform platform = PlatformExtensions.FromDefault(platformStr);

            project.ActiveConfiguration = new BuildConfiguration(configuration, platform);

            string guidStr = node.SelectSingleNode("ProjectGuid").InnerText;
            project.Info.GUID = Guid.Parse(guidStr);

            string outputTypeStr = node.SelectSingleNode("OutputType").InnerText;
            project.OutputType = ProjectOutputTypeExtensions.FromDefault(outputTypeStr);
            project.Info.Type = Logical.ProjectTypeExtensions.FromDefault(project.OutputType);

            project.Info.NamesInfo.RootNamespaceName = node.SelectSingleNode("RootNamespace").InnerText;

            project.Info.NamesInfo.AssemblyName = node.SelectSingleNode("AssemblyName").InnerText;

            string targetFrameworkStr = node.SelectSingleNode("TargetFrameworkVersion").InnerText;
            project.TargetFrameworkVersion = NetFrameworkVersionVFormatExtensions.FromDefault(targetFrameworkStr);
        }

        private static void DeserializeImportNode(Project project, XmlNode importNode)
        {
            Import import = new Import();

            import.ProjectPath = importNode.Attributes["Project"].Value;

            XmlAttribute conditionAttributeNode = importNode.Attributes["Condition"];
            if (null != conditionAttributeNode)
            {
                import.Condition = conditionAttributeNode.InnerText;
            }

            project.Imports.Add(import.ProjectPath, import);
        }

        #endregion


        protected override void Serialize(CSharpProjectSerializationUnit unit)
        {
            CSharpProjectSerializer.Serialize(unit.Path, unit.Project);
        }
    }
}
