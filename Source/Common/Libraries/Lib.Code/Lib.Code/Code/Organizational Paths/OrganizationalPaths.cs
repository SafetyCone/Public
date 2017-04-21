using System.Collections.Generic;
using System.IO;


namespace Public.Common.Lib.Code.Physical
{
    // Ok.
    public class OrganizationalPaths
    {
        public const string DefaultOrganizationsDirectoryPath = @"C:\Organizations";
        public const string DefaultRepositoriesDirectoryName = @"Repositories";
        public const string DefaultSourceDirectoryName = @"Source";


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
                  OrganizationalPaths.DefaultOrganizationsDirectoryPath,
                  organizationName,
                  OrganizationalPaths.DefaultRepositoriesDirectoryName,
                  repositoryName,
                  OrganizationalPaths.DefaultSourceDirectoryName,
                  domainName,
                  solutionTypeName)
        {
        }

        public OrganizationalPaths(
            string repositoryName,
            string domainName,
            string solutionTypeName)
            : this(
                  OrganizationalPaths.DefaultOrganizationsDirectoryPath,
                  MinexOrganization.OrganizationName,
                  OrganizationalPaths.DefaultRepositoriesDirectoryName,
                  repositoryName,
                  OrganizationalPaths.DefaultSourceDirectoryName,
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
                  OrganizationalPaths.DefaultRepositoriesDirectoryName,
                  repositoryName,
                  OrganizationalPaths.DefaultSourceDirectoryName,
                  domainName,
                  solutionTypeName)
        {
        }
    }
}
