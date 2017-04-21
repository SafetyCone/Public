

namespace Public.Common.Lib.Code.Physical
{
    public class SourceDirectoryInfo : DirectoryInfoBase
    {
        public const string DefaultSourceDirectoryPath = @"Source";


        #region Static

        public static int IdentifySourceDirectoryIndex(string[] pathTokens)
        {
            int output = OrganizationalPath.IdentifyPathTokenIndex(pathTokens, SourceDirectoryInfo.DefaultSourceDirectoryPath);
            return output;
        }

        #endregion


        public RepositoryDirectoryInfo Repository { get; set; }
        public override IPathInfo Parent
        {
            get
            {
                return this.Repository;
            }
        }


        public SourceDirectoryInfo()
            : base(SourceDirectoryInfo.DefaultSourceDirectoryPath)
        {
        }

        public SourceDirectoryInfo(RepositoryDirectoryInfo repository)
            : base(SourceDirectoryInfo.DefaultSourceDirectoryPath)
        {
            this.Repository = repository;
        }

        public SourceDirectoryInfo(string name)
            : base(name)
        {
        }

        public SourceDirectoryInfo(RepositoryDirectoryInfo repository, string name)
            : base(name)
        {
            this.Repository = repository;
        }
    }
}
