using System;
using System.Collections.Generic;


namespace Public.Common.Lib.Code.Logical
{
    public class Enumeration : LogicalTypeObjectBase
    {
        public List<string> Values { get; protected set; }


        public Enumeration()
            : base()
        {
            this.Setup();
        }

        private void Setup()
        {
            this.Values = new List<string>();
        }

        public Enumeration(string name, string namespaceName)
            : base(name, namespaceName)
        {
            this.Setup();
        }

        public Enumeration(string name, string namespaceName, IEnumerable<string> values)
            : this(name, namespaceName)
        {
            this.Values.AddRange(values);
        }
    }
}
