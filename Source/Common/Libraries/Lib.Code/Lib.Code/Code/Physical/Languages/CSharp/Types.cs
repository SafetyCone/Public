using System;
using System.Collections.Generic;

using Public.Common.Lib.Code.Logical;
using LogicalTypes = Public.Common.Lib.Code.Logical.Types;


namespace Public.Common.Lib.Code.Physical.CSharp
{
    public class Types
    {
        public const string ProgramTypeName = @"Program";
        public const string VoidTypeName = Constants.VoidKeyword;

        public const string StringTypeName = Constants.StringKeyword;
        public const string StringBoxedTypeName = @"String";
        public const string StringArrayTypeName = @"string[]";


        #region Static

        public static Dictionary<string, string> GetDefaultPhysicalCSharpNamesByLogicalName()
        {
            Dictionary<string, string> output = new Dictionary<string, string>();

            output.Add(LogicalTypes.ProgramTypeName, Types.ProgramTypeName);
            output.Add(LogicalTypes.VoidTypeName, Types.VoidTypeName);
            output.Add(LogicalTypes.StringTypeName, Types.StringTypeName);
            output.Add(LogicalTypes.StringBoxedTypeName, Types.StringBoxedTypeName);
            output.Add(LogicalTypes.StringArrayTypeName, Types.StringArrayTypeName);

            return output;
        }

        #endregion


        public Dictionary<string, string> PhysicalCSharpNamesByLogicalName { get; protected set; }


        public Types()
        {
            this.PhysicalCSharpNamesByLogicalName = Types.GetDefaultPhysicalCSharpNamesByLogicalName();
        }
    }
}
