using System;
using System.Collections.Generic;


namespace Public.Common.Lib.Code.Logical
{
    /// <summary>
    /// Base class for all code logical objects (class, enums, methods, members, etc.).
    /// </summary>
    /// <remarks>
    /// Designed for C#.
    /// </remarks>
    public abstract class LogicalObjectBase
    {
        public Accessibility Accessibility { get; set; }
        public string Name { get; set; }
        public List<string> NamespacesUsed { get; protected set; }


        protected LogicalObjectBase(string name, Accessibility accessibility)
        {
            this.NamespacesUsed = new List<string>();

            this.Name = name;
            this.Accessibility = accessibility;
        }

        protected LogicalObjectBase()
            : this(null, Accessibility.Private)
        {
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
