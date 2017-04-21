using System;
using System.Collections.Generic;


namespace Public.Common.Lib.Code.Logical
{
    /// <summary>
    /// This class 
    /// </summary>
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
