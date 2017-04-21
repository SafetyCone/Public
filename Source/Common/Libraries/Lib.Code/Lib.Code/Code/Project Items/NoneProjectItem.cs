using System;


namespace Public.Common.Lib.Code
{
    /// <summary>
    /// The relative path of a project item with no, or a special purpose (the App.config file for example).
    /// </summary>
    public class NoneProjectItem : ProjectItem
    {
        public NoneProjectItem()
        {
        }

        public NoneProjectItem(string appConfigFileName)
            : base(appConfigFileName)
        {
        }
    }
}
