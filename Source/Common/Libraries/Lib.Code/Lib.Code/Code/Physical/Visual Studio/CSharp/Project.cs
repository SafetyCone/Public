using System;
using System.Collections.Generic;

using Public.Common.Lib.Extensions;
using LogicalProject = Public.Common.Lib.Code.Logical.Project;


namespace Public.Common.Lib.Code.Physical.CSharp
{
    // Ok.
    /// <summary>
    /// This class models the physical layout of a C# project (.csproj) file, and can be serialized to/deserialized from a project file.
    /// </summary>
    /// <remarks>
    /// A logical project object can be created from a physical project object, and vice-versa.
    /// </remarks>
    public class Project
    {
        public ProjectInfo Info { get; set; }
        public Dictionary<string, ProjectItem> ProjectItemsByRelativePath { get; protected set; } // App.config is a NoneProjectItem.

        public VisualStudioVersion VisualStudioVersion { get; set; }
        public NetFrameworkVersion TargetFrameworkVersion { get; set; }
        public ProjectOutputType OutputType { get; set; }

        public BuildConfiguration ActiveConfiguration { get; set; }
        public Dictionary<BuildConfiguration, BuildConfigurationInfo> BuildConfigurationInfos { get; protected set; }

        public Dictionary<string, Import> Imports { get; protected set; }


        public Project()
        {
            this.Info = new ProjectInfo();
            this.ProjectItemsByRelativePath = new Dictionary<string, ProjectItem>();

            this.Setup();
        }

        private void Setup()
        {
            this.BuildConfigurationInfos = new Dictionary<BuildConfiguration, BuildConfigurationInfo>();
            this.Imports = new Dictionary<string, Import>();
        }

        public Project(LogicalProject logicalProject)
        {
            this.Info = new ProjectInfo(logicalProject.Info);
            this.ProjectItemsByRelativePath = new Dictionary<string, ProjectItem>(logicalProject.ProjectItemsByRelativePath);

            this.Setup();
        }
    }
}
