
namespace Public.Common.Lib.Code.Physical
{
    public class SolutionTypeDirectoryInfo : DirectoryInfoBase
    {
        #region Static

        public static int IdentifySolutionDirectoryInfo(string[] pathTokens)
        {
            int sourceDirectoryIndex = SourceDirectoryInfo.IdentifySourceDirectoryIndex(pathTokens);

            int output = OrganizationalPath.NotFoundIndex;
            if(OrganizationalPath.NotFoundIndex != sourceDirectoryIndex)
            {
                if(pathTokens.Length > sourceDirectoryIndex + 2)
                {
                    output = sourceDirectoryIndex + 2;
                }
            }

            return output;
        }

        #endregion


        public DomainDirectoryInfo Domain { get; set; }
        public override IPathInfo Parent
        {
            get
            {
                return this.Domain;
            }
        }


        public SolutionTypeDirectoryInfo(string name)
            : base(name)
        {
        }

        public SolutionTypeDirectoryInfo(string name, DomainDirectoryInfo domain)
            : base(name)
        {
            this.Domain = domain;
        }
    }
}
