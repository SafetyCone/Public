using System;
using System.Collections.Generic;


namespace Public.Common.Lib.Code.Physical
{
    /// <summary>
    /// Used in a physical solution to represent build properties for all projects for a particular solutio build configuration.
    /// </summary>
    public class ProjectBuildConfigurationSet
    {
        public BuildConfiguration BuildConfiguration { get; set; }
        public Dictionary<Guid, ProjectBuildConfigurationInfo> ProjectBuildConfigurationsByProjectGuid { get; protected set; }


        public ProjectBuildConfigurationSet()
        {
            this.ProjectBuildConfigurationsByProjectGuid = new Dictionary<Guid, ProjectBuildConfigurationInfo>();
        }

        public ProjectBuildConfigurationSet(BuildConfiguration buildConfiguration)
            : this()
        {
            this.BuildConfiguration = buildConfiguration;
        }
    }
}
