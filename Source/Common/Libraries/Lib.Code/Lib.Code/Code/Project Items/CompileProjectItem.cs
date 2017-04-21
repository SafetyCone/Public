using System;


namespace Public.Common.Lib.Code
{
    /// <summary>
    /// The relative path of compiled file in a project.
    /// </summary>
    public class CompileProjectItem : ProjectItem
    {
        public CompileProjectItem()
        {
        }

        public CompileProjectItem(string includePath)
            : base(includePath)
        {
        }
    }
}
