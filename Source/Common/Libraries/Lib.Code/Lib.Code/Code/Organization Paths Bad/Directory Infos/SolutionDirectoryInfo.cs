
namespace Public.Common.Lib.Code.Physical
{
    public class SolutionDirectoryInfo : DirectoryInfoBase
    {
        #region Static

        public static int IdentifySolutionDirectoryIndex(string[] pathTokens)
        {
            int sourceDirectoryIndex = SourceDirectoryInfo.IdentifySourceDirectoryIndex(pathTokens);

            int output = OrganizationalPath.NotFoundIndex;
            if(OrganizationalPath.NotFoundIndex != sourceDirectoryIndex)
            {
                if(pathTokens.Length > sourceDirectoryIndex + 3)
                {
                    output = sourceDirectoryIndex + 3;
                }
            }

            return output;
        }

        #endregion


        public SolutionTypeDirectoryInfo SolutionType { get; set; }
        public override IPathInfo Parent
        {
            get
            {
                return this.SolutionType;
            }
        }


        public SolutionDirectoryInfo(string name)
            : base(name)
        {
        }

        public SolutionDirectoryInfo(string name, SolutionTypeDirectoryInfo solutionType)
            : base(name)
        {
            this.SolutionType = solutionType;
        }
    }
}
