using System;
using System.Collections.Generic;

using Public.Common.Lib.Code.Logical;


namespace Public.Common.Lib.Code.Physical.CSharp
{
    public class Header
    {
        public HashSet<UsingDeclaration> Usings { get; protected set; }


        public Header()
        {
            this.Usings = new HashSet<UsingDeclaration>();
            this.Usings.Add(UsingDeclaration.System); // Add system by default.
        }

        public void AddNamespacesUsed(LogicalObjectBase logicalObjectBase)
        {
            foreach (string namespaceName in logicalObjectBase.NamespacesUsed)
            {
                this.Usings.Add(new UsingDeclaration(namespaceName));
            }
        }
    }
}
