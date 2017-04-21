using System;
using System.Collections.Generic;
using SysPath = System.IO.Path;


namespace Public.Common.Lib.Code.Physical
{
    public class ProjectPaths : OrganizationalPathBase
    {
        public SolutionPaths SolutionPaths { get; set; }
        public string ProjectDirectoryName { get; protected set; }
        public string ProjectDirectoryPath
        {
            get
            {
                string output = SysPath.Combine(this.SolutionPaths.SolutionDirectoryPath, this.ProjectDirectoryName);
                return output;
            }
        }


        public ProjectPaths()
        {
        }

        public ProjectPaths(string projectDirectoryName, SolutionPaths solutionPaths)
        {
            this.ProjectDirectoryName = projectDirectoryName;
            this.SolutionPaths = solutionPaths;
        }

        public ProjectPaths(string projectDirectoryName, string solutionDirectoryName, string solutionTypeDirectoryName, string domainDirectoryName, string repositoryDirectoryName)
            : this(
                  projectDirectoryName,
                  new SolutionPaths(solutionDirectoryName, solutionTypeDirectoryName, domainDirectoryName, repositoryDirectoryName))
        {
        }

        public override void AddPathTokens(List<string> pathTokens)
        {
            this.SolutionPaths.AddPathTokens(pathTokens);

            pathTokens.Add(this.ProjectDirectoryName);
        }
    }
}
