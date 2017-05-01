using System;
using System.Collections.Generic;
using System.IO;

using Public.Common.Lib.Code.Physical;
using Public.Common.Lib.Extensions;


namespace Public.Common.Lib.Code
{
    public class VisualStudioVersionSet
    {
        #region Static

        /// <summary>
        /// Determines whether a project file path contains a Visual Studio version token in the standard location.
        /// </summary>
        public static bool IsVisualStudioVersioned(string projectFilePath)
        {
            string projectFileName = Path.GetFileName(projectFilePath);

            string[] tokens = projectFileName.Split(PathExtensions.WindowsFileExtensionSeparatorChar);

            string possibleVsVersionToken = tokens[tokens.Length - 2];

            VisualStudioVersion dummy;
            bool output = VisualStudioVersionExtensions.TryFromDefault(possibleVsVersionToken, out dummy);
            return output;
        }

        public static string[] GetAllProjectVisualStudioVersionFilePaths(string visualStudioVersionedProjectFilePath)
        {
            VisualStudioVersion[] allVsVersions = VisualStudioVersionExtensions.GetAllVisualStudioVersions();

            string directoryPath = Path.GetDirectoryName(visualStudioVersionedProjectFilePath);
            string fileName = Path.GetFileName(visualStudioVersionedProjectFilePath);
            string[] fileNameTokens = fileName.Split(PathExtensions.WindowsFileExtensionSeparatorChar);

            int numVsVersions = allVsVersions.Length;
            string[] output = new string[numVsVersions];
            for (int iVsVersion = 0; iVsVersion < numVsVersions; iVsVersion++)
            {
                VisualStudioVersion curVsVersion = allVsVersions[iVsVersion];
                string curVsVersionStr = VisualStudioVersionExtensions.ToDefaultString(curVsVersion);

                fileNameTokens[fileNameTokens.Length - 2] = curVsVersionStr;
                string curVsVersionProjectFileName = fileNameTokens.LinearizeTokens(PathExtensions.WindowsFileExtensionSeparatorChar);

                output[iVsVersion] = Path.Combine(directoryPath, curVsVersionProjectFileName);
            }

            return output;
        }

        #endregion
    }
}
