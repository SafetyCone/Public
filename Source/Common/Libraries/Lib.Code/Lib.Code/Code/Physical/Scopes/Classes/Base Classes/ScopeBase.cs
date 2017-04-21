using System;
using System.Collections.Generic;

using Public.Common.Lib.Code.Logical;


namespace Public.Common.Lib.Code.Physical
{
    public abstract class ScopeBase
    {
        public LogicalObjectBase LogicalObject { get; set; }
        public List<ScopeBase> Children { get; protected set; }


        public ScopeBase()
        {
            this.Children = new List<ScopeBase>();
        }

        public ScopeBase(LogicalObjectBase logicalObject)
            : this()
        {
            this.LogicalObject = logicalObject;
        }
    }
}
