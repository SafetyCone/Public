using System;


namespace Public.Common.Lib.Code
{
    /// <summary>
    /// Represents a reference to a project without its physical path.
    /// </summary>
    public class ProjectReference
    {
        public string Name { get; set; }
        public string RelativePath { get; set; }
        public Guid GUID { get; set; }


        public ProjectReference()
        {
        }

        public ProjectReference(string name, string relativePath, Guid guid)
        {
            this.Name = name;
            this.RelativePath = relativePath;
            this.GUID = guid;
        }

        public ProjectReference(ProjectReference other)
            : this(other.Name, other.RelativePath, other.GUID)
        {
        }
    }
}
