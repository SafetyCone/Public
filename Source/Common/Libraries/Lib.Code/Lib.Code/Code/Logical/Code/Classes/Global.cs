using System;
using System.Collections.Generic;


namespace Public.Common.Lib.Code.Logical
{
    public class Global
    {
        public Dictionary<string, Assembly> AssembliesByName { get; set; }


        public Global()
        {
            this.AssembliesByName = new Dictionary<string, Assembly>();
        }

        public void AddAssembly(Assembly assembly)
        {
            this.AssembliesByName.Add(assembly.Name, assembly);
        }
    }
}
