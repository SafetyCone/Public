using System;
using System.Collections.Generic;

using Eshunna.Lib.Logging;


namespace Eshunna.Lib.Comparers
{
    public class StringEqualityComparer : IEqualityComparer<string>
    {
        public ILog Log { get; }


        public StringEqualityComparer(ILog log)
        {
            this.Log = log;
        }

        public bool Equals(string x, string y)
        {
            bool output = true;

            bool stringsEqual = x == y;
            if(!stringsEqual)
            {
                output = false;

                string message = $@"String value mismatch: x: {x}, y: {y}";
                this.Log.WriteLine(message);
            }

            return output;
        }

        public int GetHashCode(string obj)
        {
            int output = obj.GetHashCode(); // Use defaul.
            return output;
        }
    }
}
