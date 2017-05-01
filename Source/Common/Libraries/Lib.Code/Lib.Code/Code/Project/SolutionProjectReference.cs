using System;


namespace Public.Common.Lib.Code
{
    /// <summary>
    /// Represents a reference to a project without its physical path.
    /// </summary>
    public class SolutionProjectReference
    {
        public string Name { get; set; }
        public string RelativePath { get; set; }
        public Guid GUID { get; set; }


        public SolutionProjectReference()
        {
        }

        public SolutionProjectReference(string name, string relativePath, Guid guid)
        {
            this.Name = name;
            this.RelativePath = relativePath;
            this.GUID = guid;
        }

        public SolutionProjectReference(SolutionProjectReference other)
            : this(other.Name, other.RelativePath, other.GUID)
        {
        }
    }
}
