using System;
using System.Collections.Generic;


namespace Public.Common.Lib.Code.Physical
{
    // Ok.
    /// <summary>
    /// A physical solution represents a solution file, and can be serialized to/deserialized from a solution file.
    /// </summary>
    /// <remarks>
    /// A logical solution object can be created from a physical solution object, and vice-versa.
    /// </remarks>
    public class Solution
    {
        public SolutionInfo Info { get; set; }
        public VisualStudioVersion VisualStudioVersion { get; set; }

        public Dictionary<Guid, ProjectReference> ProjectsByGuid { get; set; }
        public Dictionary<BuildConfiguration, ProjectBuildConfigurationSet> ProjectBuildConfigurationsBySolutionBuildConfiguration { get; protected set; }


        public Solution()
        {
            this.Info = new SolutionInfo();
            this.ProjectsByGuid = new Dictionary<Guid, ProjectReference>();
            this.ProjectBuildConfigurationsBySolutionBuildConfiguration = new Dictionary<BuildConfiguration, ProjectBuildConfigurationSet>();
        }
    }
}
