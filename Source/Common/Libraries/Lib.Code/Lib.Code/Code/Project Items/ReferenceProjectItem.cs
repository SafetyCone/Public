using System;


namespace Public.Common.Lib.Code
{
    // Ok.
    public class ReferenceProjectItem : ProjectItem
    {
        public ReferenceProjectItem()
        {
        }

        public ReferenceProjectItem(string assemblyName)
            : base(assemblyName)
        {
        }
    }
}
