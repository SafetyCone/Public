using System;
using System.Collections.Generic;

using Public.Common.Lib.Code.Logical;
using Public.Common.Lib.Code.Physical;


namespace Public.Common.Lib.Code
{
    // Ok.
    public class NewProjectSpecification
    {
        public OrganizationalInfo OrganizationalInfo { get; set; }
        /// <summary>
        /// Solution type is included since if this project is going into a library solution, there is more complicated naming scheme.
        /// </summary>
        public SolutionType SolutionType { get; set; }
        public string ProjectName { get; set; }
        public ProjectType ProjectType { get; set; }
        public VisualStudioVersion VisualStudioVersion { get; set; }
        public Language Language { get; set; }
        public Dictionary<string, ProjectInfo> ReferencedProjectsByPath { get; protected set; }


        public NewProjectSpecification()
        {
            this.ReferencedProjectsByPath = new Dictionary<string, ProjectInfo>();
        }

        public NewProjectSpecification(
            string organization,
            string repository,
            string domain,
            SolutionType solutionType,
            string projectName,
            ProjectType projectType,
            VisualStudioVersion visualStudioVersion,
            Language language)
            : this()
        {
            this.SolutionType = solutionType;
            this.ProjectName = projectName;
            this.ProjectType = projectType;
            this.VisualStudioVersion = VisualStudioVersion;
            this.Language = Language;

            OrganizationalInfo orgInfo = new OrganizationalInfo();
            this.OrganizationalInfo = orgInfo;
            orgInfo.Organization = organization;
            orgInfo.Repository = repository;
            orgInfo.Domain = domain;
        }

        public NewProjectSpecification(NewSolutionSpecification solutionSpecification, ProjectType projectType)
            : this()
        {
            this.OrganizationalInfo = solutionSpecification.OrganizationalInfo;
            this.SolutionType = solutionSpecification.SolutionType;
            this.ProjectName = solutionSpecification.SolutionName; // Project name is just set to the solution name since the name of the project file is complicated and will be determine later.
            this.ProjectType = projectType;
            this.VisualStudioVersion = solutionSpecification.VisualStudioVersion;
            this.Language = solutionSpecification.Language;
        }
    }
}
