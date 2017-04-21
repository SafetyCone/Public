using System;


namespace Public.Common.Lib.Code
{
    /// <summary>
    /// The relative path and properties of a project referenced by a project.
    /// </summary>
    public class ProjectReferenceProjectItem : ProjectItem
    {
        public Guid GUID { get; set; }
        public string Name { get; set; }


        public ProjectReferenceProjectItem()
        {
        }

        public ProjectReferenceProjectItem(string includePath, Guid guid, string name)
            : base(includePath)
        {
            this.GUID = guid;
            this.Name = name;
        }
    }
}
