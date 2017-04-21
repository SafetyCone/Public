using System;
using System.Collections.Generic;


namespace Public.Common.Lib.Code.Logical
{
    /// <summary>
    /// Represents the specification for a class type.
    /// </summary>
    public class Class : LogicalStructuralTypeObjectBase
    {
        public bool IsStatic { get; set; } // Static cannot be abstract or sealed.
        public bool IsAbstract { get; set; } // Abstract cannot be sealed.
        public bool IsSealed { get; set; } // Sealed can only be sealed.
        public string BaseClassTypeName { get; set; }
        public List<string> InterfacesImplemented { get; protected set; }


        public Class()
            : base()
        {
            this.Setup();
        }

        private void Setup()
        {
            this.InterfacesImplemented = new List<string>();
        }

        public Class(string name, string namespaceName)
            : base(name, namespaceName)
        {
            this.Setup();
        }

        public Class(string name, string namespaceName, Accessibility accessibility)
            : base(name, namespaceName)
        {
            this.Setup();

            this.Accessibility = accessibility;
        }
    }
}
