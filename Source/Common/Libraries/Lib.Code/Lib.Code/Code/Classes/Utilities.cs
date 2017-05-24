﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

using Public.Common.Lib.Code.Physical;
using Solution = Public.Common.Lib.Code.Physical.Solution;
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
    }
}
