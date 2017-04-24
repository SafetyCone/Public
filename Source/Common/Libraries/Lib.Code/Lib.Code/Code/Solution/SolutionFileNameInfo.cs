using System;
using System.IO;

using Public.Common.Lib.Code.Physical;
using Public.Common.Lib.Code.Serialization;
using Public.Common.Lib.Extensions;


namespace Public.Common.Lib.Code
{
    public class SolutionFileNameInfo
    {
        public const string SolutionFileExtension = @"sln";


        #region Static

        public static SolutionFileNameInfo Parse(string solutionFilePath)
        {
            string solutionFileName = Path.GetFileName(solutionFilePath);

            string[] tokens = solutionFileName.Split(PathExtensions.WindowsFileExtensionSeparatorChar);

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
                vsVersion = SolutionSerializer.DetermineSolutionFileVsVersion(solutionFilePath);
                baseTokens = new string[tokens.Length - 1];
            }

            Array.Copy(tokens, baseTokens, baseTokens.Length);
            string fileNameBase = baseTokens.LinearizeTokens(PathExtensions.WindowsFileExtensionSeparatorChar);

            SolutionFileNameInfo output = new SolutionFileNameInfo(fileNameBase, vsVersion);
            return output;
        }

        public static string Format(SolutionFileNameInfo info)
        {
            string[] fileNameTokens = new string[3];
            fileNameTokens[0] = info.FileNameBase;
            fileNameTokens[1] = VisualStudioVersionExtensions.ToDefaultString(info.VisualStudioVersion);
            fileNameTokens[2] = SolutionFileNameInfo.SolutionFileExtension;

            string output = fileNameTokens.LinearizeTokens(PathExtensions.WindowsFileExtensionSeparatorChar);
            return output;
        }

        #endregion


        public string FileNameBase { get; set; }
        public VisualStudioVersion VisualStudioVersion { get; set; }


        public SolutionFileNameInfo()
        {
        }

        public SolutionFileNameInfo(string fileNameBase, VisualStudioVersion visualStudioVersion)
        {
            this.FileNameBase = fileNameBase;
            this.VisualStudioVersion = visualStudioVersion;
        }

        public SolutionFileNameInfo(SolutionFileNameInfo other)
            : this(other.FileNameBase, other.VisualStudioVersion)
        {
        }

        public override string ToString()
        {
            string output = SolutionFileNameInfo.Format(this);
            return output;
        }
    }
}
