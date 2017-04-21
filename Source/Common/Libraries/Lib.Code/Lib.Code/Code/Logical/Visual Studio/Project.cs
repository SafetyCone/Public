using System;
using System.Collections.Generic;


namespace Public.Common.Lib.Code.Logical
{
    // Ok.
    /// <summary>
    /// This class represents a project, which is basically a dictionary of project items by relative path.
    /// </summary>
    /// <remarks>
    /// A project file can be deserialized to a physical project object, then translated from a physical project object to this logical project object.
    /// The process can be reversed to write out a project.
    /// </remarks>
    public class Project
    {
        public ProjectInfo Info { get; set; }
        public Dictionary<string, ProjectItem> ProjectItemsByRelativePath { get; protected set; }


        public Project()
        {
            this.Info = new ProjectInfo();
            this.ProjectItemsByRelativePath = new Dictionary<string, ProjectItem>();
        }
    }
}
