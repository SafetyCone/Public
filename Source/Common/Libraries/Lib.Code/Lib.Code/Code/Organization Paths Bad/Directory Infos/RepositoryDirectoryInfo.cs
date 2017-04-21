

namespace Public.Common.Lib.Code.Physical
{
    public class RepositoryDirectoryInfo : DirectoryInfoBase
    {
        #region Static

        public static int IdentifyRepositoryDirectoryIndex(string[] pathTokens)
        {
            int repositoriesDirectoryIndex = RepositoriesDirectoryInfo.IdentifyRepositoriesDirectoryIndex(pathTokens);

            int output = OrganizationalPath.NotFoundIndex;
            if(OrganizationalPath.NotFoundIndex != repositoriesDirectoryIndex)
            {
                if(pathTokens.Length > repositoriesDirectoryIndex + 1)
                {
                    output = repositoriesDirectoryIndex + 1;
                }
            }

            return output;
        }

        #endregion


        public RepositoriesDirectoryInfo Repositories { get; set; }
        public override IPathInfo Parent
        {
            get
            {
                return this.Repositories;
            }
        }


        public RepositoryDirectoryInfo(string name)
            : base(name)
        {
        }

        public RepositoryDirectoryInfo(string name, RepositoriesDirectoryInfo repositories)
            : base(name)
        {
            this.Repositories = repositories;
        }
    }
}
