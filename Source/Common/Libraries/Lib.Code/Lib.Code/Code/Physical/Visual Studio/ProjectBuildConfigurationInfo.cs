using System;


namespace Public.Common.Lib.Code.Physical
{
    /// <summary>
    /// Used in a physical solution to represent build properties for a project for a particular solution buiild configuration.
    /// </summary>
    public class ProjectBuildConfigurationInfo
    {
        /// <summary>
        /// Specifies whether the given project should be built under the given build configuration.
        /// </summary>
        public bool Build { get; set; }
        public BuildConfiguration ProjectActiveConfiguration { get; set; }


        public ProjectBuildConfigurationInfo()
        {
        }

        public ProjectBuildConfigurationInfo(bool build, BuildConfiguration projectActiveConfiguration)
        {
            this.Build = build;
            this.ProjectActiveConfiguration = projectActiveConfiguration;
        }

        public ProjectBuildConfigurationInfo(ProjectBuildConfigurationInfo other)
            : this(other.Build, other.ProjectActiveConfiguration)
        {
        }
    }
}
