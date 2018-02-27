using System;
using System.Collections.Generic;

using Eshunna.Lib.Logging;


namespace Eshunna.Lib.Comparers
{
    public class IntegerEqualityComparer : IEqualityComparer<int>
    {
        public ILog Log { get; }


        public IntegerEqualityComparer(ILog log)
        {
            this.Log = log;
        }

        public bool Equals(int x, int y)
        {
            bool output = true;

            bool valuesEqual = x == y;
            if(!valuesEqual)
            {
                output = false;

                string message = $@"Integer value mismatch: x: {x.ToString()}, y: {y.ToString()}";
                this.Log.WriteLine(message);
            }

            return output;
        }

        public int GetHashCode(int obj)
        {
            int output = obj.GetHashCode(); // Use default.
            return output;
        }
    }
}
