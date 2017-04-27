using System;


namespace Public.Common.Lib.Code
{
    /// <summary>
    /// The relative path of an empty folder in a project file.
    /// </summary>
    public class FolderProjectItem : ProjectItem
    {
        public FolderProjectItem()
        {
        }

        public FolderProjectItem(string includePath)
            : base(includePath)
        {
        }

        public FolderProjectItem(FolderProjectItem other)
            : this(other.IncludePath)
        {
        }

        public override ProjectItem Clone()
        {
            return new FolderProjectItem(this);
        }
    }
}
