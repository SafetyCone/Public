using System;


namespace Public.Common.Lib.Code
{
    public class Reference
    {
        public string AssemblyName { get; set; }


        public Reference()
        {
        }

        public Reference(string @namespace)
        {
            this.AssemblyName = @namespace;
        }
    }
}
