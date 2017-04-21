using System;
using System.Collections.Generic;


namespace Public.Common.Lib.Code.Logical
{
    /// <summary>
    /// Represents an assembly as a dictionary of types keyed by type key (generally {namespaceName}.{className}).
    /// </summary>
    public class Assembly
    {
        public string Name { get; set; }
        public Dictionary<string, LogicalTypeObjectBase> TypesByTypeKey { get; protected set; }


        public Assembly()
        {
            this.TypesByTypeKey = new Dictionary<string, LogicalTypeObjectBase>();
        }

        public Assembly(string name)
            : this()
        {
        }

        public void AddType(LogicalTypeObjectBase logicalType)
        {
            this.TypesByTypeKey.Add(logicalType.TypeKey, logicalType);
        }
    }
}
