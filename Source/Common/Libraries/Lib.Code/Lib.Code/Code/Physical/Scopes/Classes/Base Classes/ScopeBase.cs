using System;
using System.Collections.Generic;

using Public.Common.Lib.Code.Logical;


namespace Public.Common.Lib.Code.Physical
{
    /// <summary>
    /// Holds a logical object and any child logical objects.
    /// </summary>
    /// <remarks>
    /// All code is represented by a hierarchy of scopes.
    /// Leaf scopes might be just one line of code, while the highest level scope encompases the whole file (outside of the file header).
    /// </remarks>
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
