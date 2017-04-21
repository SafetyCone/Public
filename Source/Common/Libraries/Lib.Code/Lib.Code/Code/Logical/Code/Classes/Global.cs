using System;
using System.Collections.Generic;


namespace Public.Common.Lib.Code.Logical
{
    /// <summary>
    /// The global object serves as a collection of assemblies.
    /// </summary>
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
