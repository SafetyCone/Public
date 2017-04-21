using System.Collections.Generic;
using System.IO;


namespace Public.Common.Lib.Code.Physical
{
    // Ok.
    public class OrganizationalPaths
    {
        public string OrganizationsDirectoryPath { get; set; }
        public string OrganizationDirectoryPath { get; protected set; }
        public string RepositoriesDirectoryPath { get; protected set; }
        public string RepositoryDirectoryPath { get; protected set; }
        public string SourceDirectoryPath { get; protected set; }
        public string DomainDirectoryPath { get; protected set; }
        public string SolutionTypeDirectoryPath { get; protected set; }


        public List<string> GetPaths()
        {
            string[] paths = new string[]
            {
                this.OrganizationsDirectoryPath,
                this.OrganizationDirectoryPath,
                this.RepositoriesDirectoryPath,
                this.RepositoryDirectoryPath,
                this.SourceDirectoryPath,
                this.DomainDirectoryPath,
                this.SolutionTypeDirectoryPath,
            };

            List<string> output = new List<string>(paths);
            return output;
        }

        public OrganizationalPaths(
            string organizationsPath,
            string organizationName,
            string customRepositoriesDirectoryName,
            string repositoryName,
            string customSourceDirectoryName,
            string domainName,
            string solutionTypeName)
        {
            this.OrganizationsDirectoryPath = organizationsPath;
            this.OrganizationDirectoryPath = Path.Combine(this.OrganizationsDirectoryPath, organizationName);
            this.RepositoriesDirectoryPath = Path.Combine(this.OrganizationDirectoryPath, customRepositoriesDirectoryName);
            this.RepositoryDirectoryPath = Path.Combine(this.RepositoriesDirectoryPath, repositoryName);
            this.SourceDirectoryPath = Path.Combine(this.RepositoryDirectoryPath, customSourceDirectoryName);
            this.DomainDirectoryPath = Path.Combine(this.SourceDirectoryPath, domainName);
            this.SolutionTypeDirectoryPath = Path.Combine(this.DomainDirectoryPath, solutionTypeName);
        }

        public OrganizationalPaths(
            string organizationName,
            string repositoryName,
            string domainName,
            string solutionTypeName)
            : this(
                  Constants.DefaultOrganizationsDirectoryPath,
                  organizationName,
                  Constants.DefaultRepositoriesDirectoryName,
                  repositoryName,
                  Constants.DefaultSourceDirectoryName,
                  domainName,
                  solutionTypeName)
        {
        }

        public OrganizationalPaths(
            string repositoryName,
            string domainName,
            string solutionTypeName)
            : this(
                  Constants.DefaultOrganizationsDirectoryPath,
                  MinexOrganization.OrganizationName,
                  Constants.DefaultRepositoriesDirectoryName,
                  repositoryName,
                  Constants.DefaultSourceDirectoryName,
                  domainName,
                  solutionTypeName)
        {
        }

        public OrganizationalPaths(
            string organizationsDirectoryPath,
            string organizationName,
            string repositoryName,
            string domainName,
            string solutionTypeName)
            : this(
                  organizationsDirectoryPath,
                  organizationName,
                  Constants.DefaultRepositoriesDirectoryName,
                  repositoryName,
                  Constants.DefaultSourceDirectoryName,
                  domainName,
                  solutionTypeName)
        {
        }
    }
}
