using System;


namespace Public.Common.Lib.Code.Logical
{
    /// <summary>
    /// Represents the specification for a structure.
    /// </summary>
    public class Struct : LogicalStructuralTypeObjectBase
    {
        public Struct()
            : base()
        {
        }

        public Struct(string name, string namespaceName)
            : base(name, namespaceName)
        {
        }
    }
}
