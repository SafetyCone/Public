using System;

using Public.Common.Lib.Code.Logical;


namespace Public.Common.Lib.Code.Physical
{
    /// <summary>
    /// A non-parent scope.
    /// </summary>
    public class Scope : ScopeBase
    {
        public Scope()
        {
        }

        public Scope(LogicalObjectBase logicalObject)
            : base(logicalObject)
        {
            this.LogicalObject = logicalObject;
        }
    }
}
