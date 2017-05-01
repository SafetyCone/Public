using System;
using System.IO;

using Public.Common.Lib.Code.Physical;
using Public.Common.Lib.Extensions;


namespace Public.Common.Lib.Code
{
    public class ProjectFilePathInfo
    {
        #region Static

        public static string ChangeVisualStudioVersion(string visualStudioVersionedProjectFilePath, VisualStudioVersion desiredVisualStudioVersion)
        {
            string directoryPath = Path.GetDirectoryName(visualStudioVersionedProjectFilePath);
            string fileName = Path.GetFileNameWithoutExtension(visualStudioVersionedProjectFilePath);
            string fileExtension = PathExtensions.GetExtensionOnly(visualStudioVersionedProjectFilePath);

            string[] fileNameTokens = fileName.Split(PathExtensions.WindowsFileExtensionSeparatorChar);
            fileNameTokens[fileNameTokens.Length - 1] = VisualStudioVersionExtensions.ToDefaultString(desiredVisualStudioVersion);

            string changedFileName = fileNameTokens.LinearizeTokens(PathExtensions.WindowsFileExtensionSeparatorChar);
            string changedFullFileName = String.Format(@"{0}{1}{2}", changedFileName, PathExtensions.WindowsFileExtensionSeparatorChar, fileExtension);

            string output = Path.Combine(directoryPath, changedFullFileName);
            return output;
        }

        #endregion
    }
}
