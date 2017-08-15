using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;

using Public.Common.Lib.Code.Physical;
using PhysicalSolution = Public.Common.Lib.Code.Physical.Solution;
using Public.Common.Lib.Code.Serialization;
using Public.Common.Lib.Extensions;


namespace Public.Common.Lib.Code
{
    public static class Utilities
    {
        /// <summary>
        /// Gets the paths of all solution files in the top directory only of the specified solution directory path.
        /// </summary>
        public static string[] GetSolutionFilePaths(string solutionDirectoryPath)
        {
            // *.sln
            string searchPattern = String.Format(@"{0}{1}{2}", SearchPatternHelper.Wildcard, PathExtensions.WindowsFileExtensionSeparatorChar, SolutionFileNameInfo.SolutionFileExtension);

            string[] output = Directory.GetFiles(solutionDirectoryPath, searchPattern, SearchOption.TopDirectoryOnly);
            return output;
        }

        public static bool GetSolutionFilePath(string solutionDirectoryPath, VisualStudioVersion visualStudioVersion, out string visualStudioVersionSolutionFilePath)
        {
            // *.sln
            string searchPattern = String.Format(@"{0}{1}{2}", SearchPatternHelper.Wildcard, PathExtensions.WindowsFileExtensionSeparatorChar, SolutionFileNameInfo.SolutionFileExtension);
            string[] solutionFilePaths = Directory.GetFiles(solutionDirectoryPath, searchPattern, SearchOption.TopDirectoryOnly);

            visualStudioVersionSolutionFilePath = String.Empty;
            bool output = false;
            foreach(string solutionFilePath in solutionFilePaths)
            {
                SolutionFileNameInfo solutionInfo = SolutionFileNameInfo.Parse(solutionFilePath);
                if(visualStudioVersion == solutionInfo.VisualStudioVersion)
                {
                    visualStudioVersionSolutionFilePath = solutionFilePath;

                    output = true;
                    break;
                }
            }

            return output;
        }

        /// <summary>
        /// Determines whether a file path contains one a .VS{YEAR}. token.
        /// </summary>
        public static bool IsVisualStudioVersioned(string filePath)
        {
            bool output = Regex.IsMatch(filePath, @"\.VS[0-9]{4}\.");
            return output;
        }

        public static List<string> GetProjectFilePaths(string solutionFilePath, Solution solution)
        {
            List<string> output = new List<string>();
            foreach(Guid guid in solution.ProjectsByGuid.Keys)
            {
                SolutionProjectReference project = solution.ProjectsByGuid[guid];

                string projectFilePath = PathExtensions.GetPath(solutionFilePath, project.RelativePath);
                output.Add(projectFilePath);
            }

            return output;
        }

        #region Ensure VS-Versioned Bin And Obj Project Properties

        public static void EnsureVsVersionedBinAndObjProperties(string solutionDirectoryPath, HashSet<string> projectDirectoryPathsAllowedToChange)
        {
            string[] solutionFilePaths = Utilities.GetSolutionFilePaths(solutionDirectoryPath);
            foreach (string solutionFilePath in solutionFilePaths)
            {
                PhysicalSolution solution = SolutionSerializer.Deserialize(solutionFilePath);

                List<string> projectFilePaths = Utilities.GetProjectFilePaths(solutionFilePath, solution);
                foreach (string projectFilePath in projectFilePaths)
                {
                    string projectDirectoryPath = Path.GetDirectoryName(projectFilePath);
                    if (projectDirectoryPathsAllowedToChange.Contains(projectDirectoryPath))
                    {
                        Utilities.EnsureProjectVsVersionedBinAndObjProperties(projectFilePath);
                    }
                }
            }
        }

        public static void EnsureVsVersionedBinAndObjProperties(string solutionDirectoryPath)
        {
            string[] solutionFilePaths = Utilities.GetSolutionFilePaths(solutionDirectoryPath);
            foreach (string solutionFilePath in solutionFilePaths)
            {
                PhysicalSolution solution = SolutionSerializer.Deserialize(solutionFilePath);

                List<string> projectFilePaths = Utilities.GetProjectFilePaths(solutionFilePath, solution);
                foreach (string projectFilePath in projectFilePaths)
                {
                    string projectDirectoryPath = Path.GetDirectoryName(projectFilePath);
                    Utilities.EnsureProjectVsVersionedBinAndObjProperties(projectFilePath);
                }
            }
        }

        private static void EnsureProjectVsVersionedBinAndObjProperties(string projectFilePath)
        {
            ProjectFileNameInfo projectInfo = ProjectFileNameInfo.Parse(projectFilePath);

            // Load project XML.
            XmlDocument doc = new XmlDocument();
            doc.LoadNoNamespaces(projectFilePath);

            if (VisualStudioVersion.VS_UNKNOWN == projectInfo.VisualStudioVersion)
            {
                XmlNode toolsVersionNode = doc.SelectSingleNode(@"/Project/@ToolsVersion");
                string toolsVersion = toolsVersionNode.InnerText;

                switch (toolsVersion)
                {
                    case @"12.0":
                        projectInfo.VisualStudioVersion = VisualStudioVersion.VS2013;
                        break;

                    case @"14.0":
                        projectInfo.VisualStudioVersion = VisualStudioVersion.VS2015;
                        break;

                    case @"15.0":
                        projectInfo.VisualStudioVersion = VisualStudioVersion.VS2017;
                        break;

                    default:
                        projectInfo.VisualStudioVersion = VisualStudioVersion.VS2010;
                        break;
                }
            }

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
    }
}
