using System;
using Public.Common.Lib.DesignPatterns;


namespace Public.Examples.Code
{
    public class FactoryClass : IFactory<string, int>
    {
        #region Static

        private static int StaticValueConstructionFunction(string order)
        {
            int output = Int32.Parse(order);
            return output;
        }

        #endregion

        #region IFactory<string, int> Members

        public int this[string order]
        {
            get
            {
                int output = this.InstanceValueConstructionFunction(order);
                return output;
            }
        }

        #endregion

        private int InstanceValueConstructionFunction(string order)
        {
            int output = FactoryClass.StaticValueConstructionFunction(order);
            return output;
        }
    }
}
