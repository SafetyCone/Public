using System;
using System.Collections.Generic;


namespace Public.Common.Lib.Code.Logical
{
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
