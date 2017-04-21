using System;
using System.Collections.Generic;


namespace Public.Common.Lib.Code.Logical
{
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
