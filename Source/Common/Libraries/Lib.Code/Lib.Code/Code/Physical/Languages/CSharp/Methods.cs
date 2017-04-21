using System;
using System.Collections.Generic;

using Public.Common.Lib.Code.Logical;
using LogicalMethods = Public.Common.Lib.Code.Logical.Methods;


namespace Public.Common.Lib.Code.Physical.CSharp
{
    /// <summary>
    /// Provides useful constants for important method names (like "Main"). These are substituted for logical placeholder names when serializing a logical class.
    /// </summary>
    public class Methods
    {
        public const string MainMethodName = @"Main";


        #region Static

        private static Dictionary<string, string> GetDefaultNamesByLogicalName()
        {
            Dictionary<string, string> output = new Dictionary<string, string>();

            output.Add(LogicalMethods.MainMethodName, Methods.MainMethodName);

            return output;
        }

        #endregion


        public Dictionary<string, string> PhysicalCSharpNamesByLogicalName { get; private set; }


        public Methods()
        {
            this.PhysicalCSharpNamesByLogicalName = Methods.GetDefaultNamesByLogicalName();
        }
    }
}
