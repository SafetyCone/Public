using System;


namespace Public.Common.Lib.Code
{
    // Ok.
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
    }
}
