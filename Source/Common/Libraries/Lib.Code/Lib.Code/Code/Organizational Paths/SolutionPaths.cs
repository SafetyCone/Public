using System;
using System.Collections.Generic;
using SysPath = System.IO.Path;


namespace Public.Common.Lib.Code.Physical
{
    public class SolutionPaths : OrganizationalPathBase
    {
        public DomainPaths DomainPaths { get; set; }
        public string SolutionTypeDirectoryName { get; set; }
        public string SolutionTypeDirectoryPath
        {
            get
            {
                string output = SysPath.Combine(this.DomainPaths.DomainDirectoryPath, this.SolutionTypeDirectoryName);
                return output;
            }
        }
        public string SolutionDirectoryName { get; set; }
        public string SolutionDirectoryPath
        {
            get
            {
                string output = SysPath.Combine(this.SolutionTypeDirectoryPath, this.SolutionDirectoryName);
                return output;
            }
        }


        public SolutionPaths()
        {
        }

        public SolutionPaths(string solutionDirectoryName, string solutionTypeDirectoryName, DomainPaths domainPaths)
        {
            this.DomainPaths = domainPaths;
            this.SolutionDirectoryName = solutionDirectoryName;
            this.SolutionTypeDirectoryName = solutionTypeDirectoryName;
        }

        public SolutionPaths(
            string solutionDirectoryName,
            string solutionTypeDirectoryName,
            string domainDirectoryName,
            string repositoryDirectoryName)
            : this(
                  solutionDirectoryName,
                  solutionTypeDirectoryName,
                  new DomainPaths(domainDirectoryName, repositoryDirectoryName))
        {
        }

        public SolutionPaths(
            string solutionDirectoryName,
            string solutionTypeDirectoryName,
            string domainDirectoryName,
            string repositoryDirectoryName,
            OrganizationPaths organizationPaths)
            : this(
                  solutionDirectoryName,
                  solutionTypeDirectoryName,
                  new DomainPaths(domainDirectoryName, new RepositoryPaths(repositoryDirectoryName, organizationPaths)))
        {
        }

        public override void AddPathTokens(List<string> pathTokens)
        {
            this.DomainPaths.AddPathTokens(pathTokens);

            pathTokens.Add(this.SolutionTypeDirectoryName);
            pathTokens.Add(this.SolutionDirectoryName);
        }
    }
}
