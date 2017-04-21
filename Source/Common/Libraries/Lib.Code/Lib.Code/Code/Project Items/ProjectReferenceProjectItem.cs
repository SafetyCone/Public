using System;


namespace Public.Common.Lib.Code
{
    // Ok.
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
