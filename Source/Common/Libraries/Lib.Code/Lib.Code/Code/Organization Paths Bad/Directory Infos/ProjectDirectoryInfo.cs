

namespace Public.Common.Lib.Code.Physical
{
    public class ProjectDirectoryInfo : DirectoryInfoBase
    {
        #region Static

        public static int IdentifyProjectDirectoryIndex(string[] pathTokens)
        {
            int sourceDirectoryIndex = SourceDirectoryInfo.IdentifySourceDirectoryIndex(pathTokens);

            int output = OrganizationalPath.NotFoundIndex;
            if(OrganizationalPath.NotFoundIndex != sourceDirectoryIndex)
            {
                if(pathTokens.Length > sourceDirectoryIndex + 4)
                {
                    output = sourceDirectoryIndex + 4;
                }
            }

            return output;
        }

        #endregion


        public SolutionDirectoryInfo Solution { get; set; }
        public override IPathInfo Parent
        {
            get
            {
                return this.Solution;
            }
        }


        public ProjectDirectoryInfo(string name)
            : base(name)
        {
        }

        public ProjectDirectoryInfo(string name, SolutionDirectoryInfo solution)
            : base(name)
        {
            this.Solution = solution;
        }
    }
}
