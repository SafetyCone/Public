using System.Collections.Generic;
using SysPath = System.IO.Path;


namespace Public.Common.Lib.Code.Physical
{
    public class RepositoryPaths : OrganizationalPathBase
    {
        public const string DefaultRepositoriesDirectoryName = @"Repositories";
        public const string DefaultSourceDirectoryName = @"Source";


        public OrganizationPaths OrganizationPaths { get; set; }
        public string RepositoriesDirectoryName { get; set; }
        public string RepositoriesDirectoryPath
        {
            get
            {
                string output = SysPath.Combine(this.OrganizationPaths.OrganizationDirectoryPath, this.RepositoriesDirectoryName);
                return output;
            }
        }
        public string RepositoryDirectoryName { get; set; }
        public string RepositoryDirectoryPath
        {
            get
            {
                string output = SysPath.Combine(this.RepositoriesDirectoryPath, this.RepositoryDirectoryName);
                return output;
            }
        }
        public string SourceDirectoryName { get; set; }
        public string SourceDirectoryPath
        {
            get
            {
                string output = SysPath.Combine(this.RepositoryDirectoryPath, this.SourceDirectoryName);
                return output;
            }
        }


        public RepositoryPaths()
        {
        }

        public RepositoryPaths(string repositoryDirectoryName, OrganizationPaths organizationPaths, string customRepositoriesDirectoryName, string customSourceDirectoryName)
        {
            this.OrganizationPaths = organizationPaths;
            this.RepositoriesDirectoryName = customRepositoriesDirectoryName;
            this.RepositoryDirectoryName = repositoryDirectoryName;
            this.SourceDirectoryName = customSourceDirectoryName;
        }

        public RepositoryPaths(string repositoryDirectoryName, OrganizationPaths organizationPaths)
            : this(repositoryDirectoryName, organizationPaths, RepositoryPaths.DefaultRepositoriesDirectoryName, RepositoryPaths.DefaultSourceDirectoryName)
        {
        }

        public RepositoryPaths(string repositoryDirectoryName)
            : this(repositoryDirectoryName, new OrganizationPaths())
        {
        }

        public override void AddPathTokens(List<string> pathTokens)
        {
            this.OrganizationPaths.AddPathTokens(pathTokens);

            pathTokens.Add(this.RepositoriesDirectoryName);
            pathTokens.Add(this.RepositoryDirectoryName);
            pathTokens.Add(this.SourceDirectoryName);
        }
    }
}
