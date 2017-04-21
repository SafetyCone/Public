using System;
using System.Collections.Generic;


namespace Public.Common.Lib.Code.Physical
{
    /// <summary>
    /// The parent scope for all code.
    /// </summary>
    public class NamespaceScope : ScopeBase
    {
        public string Name { get; set; }


        public NamespaceScope()
            : base()
        {
        }

        public NamespaceScope(string name)
            : base()
        {
            this.Name = name;
        }
    }
}
