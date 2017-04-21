

namespace Public.Common.Lib.Code.Physical
{
    public class RepositoriesDirectoryInfo : DirectoryInfoBase
    {
        public const string DefaultRepositoriesDirectoryName = @"Repositories";


        #region Static

        public static int IdentifyRepositoriesDirectoryIndex(string[] pathTokens)
        {
            int output = OrganizationalPath.IdentifyPathTokenIndex(pathTokens, RepositoriesDirectoryInfo.DefaultRepositoriesDirectoryName);
            return output;
        }

        #endregion


        public OrganizationDirectoryInfo Organization { get; set; }
        public override IPathInfo Parent
        {
            get
            {
                return this.Organization;
            }
        }


        public RepositoriesDirectoryInfo()
            : base(RepositoriesDirectoryInfo.DefaultRepositoriesDirectoryName)
        {
        }

        public RepositoriesDirectoryInfo(OrganizationDirectoryInfo organization)
            : base(RepositoriesDirectoryInfo.DefaultRepositoriesDirectoryName)
        {
            this.Organization = organization;
        }

        public RepositoriesDirectoryInfo(string name)
            : base(name)
        {
        }

        public RepositoriesDirectoryInfo(OrganizationDirectoryInfo organization, string name)
            : base(name)
        {
            this.Organization = organization;
        }
    }
}
