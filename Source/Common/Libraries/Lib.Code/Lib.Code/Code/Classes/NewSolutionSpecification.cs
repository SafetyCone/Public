using System;

using Public.Common.Lib.Code.Logical;
using Public.Common.Lib.Code.Physical;


namespace Public.Common.Lib.Code
{
    /// <summary>
    /// Contains information used in creating (construction and serializing) a new Visual Studio solution.
    /// </summary>
    public class NewSolutionSpecification
    {
        public string OrganizationsDirectoryPath { get; set; }
        public OrganizationalInfo OrganizationalInfo { get; set; }
        public SolutionType SolutionType { get; set; }
        public string SolutionName { get; set; }
        /// <summary>
        /// A solution also specifies a project type since we might want a WinForms based Application, or a WPF Experiment.
        /// </summary>
        public ProjectType ProjectType { get; set; }
        public VisualStudioVersion VisualStudioVersion { get; set; }
        public Language Language { get; set; }


        public NewSolutionSpecification(
            string organizationsDirectoryPath,
            string organization,
            string repository,
            string domain,
            SolutionType solutionType,
            string solutionName,
            ProjectType projectType,
            VisualStudioVersion visualStudioVersion,
            Language language)
        {
            this.OrganizationsDirectoryPath = organizationsDirectoryPath;
            this.SolutionType = solutionType;
            this.SolutionName = solutionName;
            this.ProjectType = projectType;
            this.VisualStudioVersion = visualStudioVersion;
            this.Language = language;

            OrganizationalInfo orgInfo = new OrganizationalInfo();
            this.OrganizationalInfo = orgInfo;
            orgInfo.Organization = organization;
            orgInfo.Repository = repository;
            orgInfo.Domain = domain;
        }

        public NewSolutionSpecification(
            string organization,
            string repository,
            string domain,
            SolutionType solutionType,
            string solutionName,
            ProjectType projectType,
            VisualStudioVersion visualStudioVersion)
            : this(
                  OrganizationalPaths.DefaultOrganizationsDirectoryPath,
                  organization,
                  repository,
                  domain,
                  solutionType,
                  solutionName,
                  projectType,
                  visualStudioVersion,
                  Language.CSharp)
        {
        }

        public NewSolutionSpecification(NewSolutionSpecification other)
        {
            this.Language = other.Language;
            this.OrganizationalInfo = new OrganizationalInfo(other.OrganizationalInfo);
            this.OrganizationsDirectoryPath = other.OrganizationsDirectoryPath;
            this.ProjectType = other.ProjectType;
            this.SolutionName = other.SolutionName;
            this.SolutionType = other.SolutionType;
            this.VisualStudioVersion = other.VisualStudioVersion;
        }
    }
}
