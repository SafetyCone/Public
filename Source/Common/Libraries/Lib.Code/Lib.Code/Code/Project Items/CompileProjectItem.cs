using System;


namespace Public.Common.Lib.Code
{
    // Ok.
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
