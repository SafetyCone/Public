using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

using Solution = Public.Common.Lib.Code.Physical.Solution;
using Public.Common.Lib.Extensions;
using Public.Common.WindowsShell;


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
