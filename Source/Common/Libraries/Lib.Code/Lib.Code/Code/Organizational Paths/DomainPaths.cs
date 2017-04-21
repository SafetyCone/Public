using System;
using System.Collections.Generic;
using SysPath = System.IO.Path;


namespace Public.Common.Lib.Code.Physical
{
    public class DomainPaths : OrganizationalPathBase
    {
        public RepositoryPaths RepositoryPaths { get; set; }
        public string DomainDirectoryName { get; set; }
        public string DomainDirectoryPath
        {
            get
            {
                string output = SysPath.Combine(this.RepositoryPaths.SourceDirectoryPath, this.DomainDirectoryName);
                return output;
            }
        }


        public DomainPaths()
        {
        }

        public DomainPaths(string domainDirectoryName, RepositoryPaths repositoryPaths)
        {
            this.RepositoryPaths = repositoryPaths;
            this.DomainDirectoryName = domainDirectoryName;
        }

        public DomainPaths(string domainDirectoryName, string repositoryDirectoryName)
            : this(domainDirectoryName, new RepositoryPaths(repositoryDirectoryName))
        {
        }

        public override void AddPathTokens(List<string> pathTokens)
        {
            this.RepositoryPaths.AddPathTokens(pathTokens);

            pathTokens.Add(this.DomainDirectoryName);
        }
    }
}
