using System;


namespace Public.Common.Lib.Code
{
    // Ok.
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
