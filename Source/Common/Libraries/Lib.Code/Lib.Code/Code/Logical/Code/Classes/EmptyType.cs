using System;
using System.Collections.Generic;


namespace Public.Common.Lib.Code.Logical
{
    /// <summary>
    /// Represents a non-existent class, allowing codefiles without namespaces or classes.
    /// </summary>
    /// <remarks>
    /// The AssemblyInfo file is a code file, but it contains no namespace and no class.
    /// We want to use the serialization logic for C# class, but without specifying a class. This empty class type allows that.
    /// </remarks>
    public class EmptyType : LogicalTypeObjectBase
    {
        public List<string> Lines { get; protected set; }


        public EmptyType(string name)
            : base(name, String.Empty)
        {
            this.Lines = new List<string>();
        }
    }
}
