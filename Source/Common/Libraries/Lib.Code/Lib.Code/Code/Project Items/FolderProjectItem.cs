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
    }
}
