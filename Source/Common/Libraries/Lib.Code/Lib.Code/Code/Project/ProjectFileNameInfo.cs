using System;
using System.IO;

using Public.Common.Lib.Code.Physical;
using Public.Common.Lib.Extensions;


namespace Public.Common.Lib.Code
{
    public class ProjectFileNameInfo
    {
        #region Static

        public static ProjectFileNameInfo Parse(string projectFilePath, VisualStudioVersion assumedVisualStudioVersion)
        {
            string projectFileName = Path.GetFileName(projectFilePath);

            string[] tokens = projectFileName.Split(PathExtensions.WindowsFileExtensionSeparatorChar);

            // If the file name has a VS version token, then fewer tokens are part of the base file name.
            string possibleVsVersionToken = tokens[tokens.Length - 2];
            VisualStudioVersion vsVersion;
            string[] baseTokens;
            if (VisualStudioVersionExtensions.TryFromDefault(possibleVsVersionToken, out vsVersion))
            {
                baseTokens = new string[tokens.Length - 2];
            }
            else
            {
                vsVersion = assumedVisualStudioVersion;
                baseTokens = new string[tokens.Length - 1];
            }

            // Language is always determined from the file extension, which is always the last token.
            string languageToken = tokens[tokens.Length - 1];
            Language language = ProjectFileLanguageExtensions.FromDefault(languageToken);

            Array.Copy(tokens, baseTokens, baseTokens.Length);
            string fileNameBase = baseTokens.LinearizeTokens(PathExtensions.WindowsFileExtensionSeparatorChar);

            ProjectFileNameInfo output = new ProjectFileNameInfo(fileNameBase, vsVersion, language);
            return output;
        }

        /// <summary>
        /// Return the base file name of a project file (which lacks the Visual Studio version token and the solution file extension).
        /// </summary>
        public static string GetFileNameBase(string projectFilePath)
        {
            string projectFileName = Path.GetFileName(projectFilePath);

            string[] tokens = projectFileName.Split(PathExtensions.WindowsFileExtensionSeparatorChar);

            string possibleVsVersionToken = tokens[tokens.Length - 2];

            int lastBaseFileNameTokenIndex;
            VisualStudioVersion vsVersion;
            if (VisualStudioVersionExtensions.TryFromDefault(possibleVsVersionToken, out vsVersion))
            {
                lastBaseFileNameTokenIndex = tokens.Length - 2;
            }
            else
            {
                lastBaseFileNameTokenIndex = tokens.Length - 1;
            }

            string[] baseTokens = new string[lastBaseFileNameTokenIndex];
            Array.Copy(tokens, baseTokens, baseTokens.Length);

            string output = baseTokens.LinearizeTokens(PathExtensions.WindowsFileExtensionSeparatorChar);
            return output;
        }

        public static string Format(ProjectFileNameInfo info)
        {
            string[] fileNameTokens = new string[3];
            fileNameTokens[0] = info.FileNameBase;
            fileNameTokens[1] = VisualStudioVersionExtensions.ToDefaultString(info.VisualStudioVersion);
            fileNameTokens[2] = ProjectFileLanguageExtensions.ToDefaultString(info.Language);

            string output = fileNameTokens.LinearizeTokens(PathExtensions.WindowsFileExtensionSeparatorChar);
            return output;
        }

        #endregion


        public string FileNameBase { get; set; }
        public VisualStudioVersion VisualStudioVersion { get; set; }
        public Language Language { get; set; }


        public ProjectFileNameInfo()
        {
        }

        public ProjectFileNameInfo(string fileNameBase, VisualStudioVersion visualStudioVersion, Language language)
        {
            this.FileNameBase = fileNameBase;
            this.VisualStudioVersion = visualStudioVersion;
            this.Language = language;
        }

        public ProjectFileNameInfo(ProjectFileNameInfo other)
            : this(other.FileNameBase, other.VisualStudioVersion, other.Language)
        {
        }

        public override string ToString()
        {
            string output = ProjectFileNameInfo.Format(this);
            return output;
        }
    }
}
