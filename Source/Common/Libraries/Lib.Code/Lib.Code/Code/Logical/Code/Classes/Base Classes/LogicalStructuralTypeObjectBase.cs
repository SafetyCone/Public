using System;
using System.Collections.Generic;


namespace Public.Common.Lib.Code.Logical
{
    /// <summary>
    /// Base class for all logical types that contain member or method logical objects (classes, structures). These logical types serve to structure other logical objects.
    /// </summary>
    public class LogicalStructuralTypeObjectBase : LogicalTypeObjectBase
    {
        public List<Member> Members { get; protected set; }
        public List<Method> Methods { get; protected set; }


        protected LogicalStructuralTypeObjectBase()
            : base()
        {
            this.Setup();
        }

        private void Setup()
        {
            this.Members = new List<Member>();
            this.Methods = new List<Method>();
        }

        protected LogicalStructuralTypeObjectBase(string name, string namespaceName)
            : base(name, namespaceName)
        {
            this.Setup();
        }
    }
}
