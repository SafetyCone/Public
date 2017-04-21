using System;

namespace Public.Common.Lib.Code.Logical
{
    /// <summary>
    /// Represents the specification of a delegate type, which just contains a method.
    /// </summary>
    public class Delegate : LogicalTypeObjectBase
    {
        public Method Method { get; set; }


        public Delegate()
            : base()
        {
        }

        public Delegate(string name, string namespaceName)
            : base(name, namespaceName)
        {
        }

        public Delegate(string name, string namespaceName, Method method)
            : this(name, namespaceName)
        {
            this.Method = method;
        }
    }
}
