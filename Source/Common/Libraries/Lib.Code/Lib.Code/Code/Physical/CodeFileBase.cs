using System;
using System.Collections.Generic;


namespace Public.Common.Lib.Code.Physical
{
    /// <summary>
    /// Serves as the base class for all code files.
    /// </summary>
    public class CodeFileBase
    {
        public List<ScopeBase> Scopes { get; protected set; }


        public CodeFileBase()
        {
            this.Scopes = new List<ScopeBase>();
        }
    }
}
