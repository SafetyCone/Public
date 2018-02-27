using System;
using System.Collections.Generic;
using System.Linq;

using Eshunna.Lib.Logging;


namespace Eshunna.Lib.Comparers
{
    public class DictionaryEqualityComparer<TKey, TValue> : IEqualityComparer<Dictionary<TKey, TValue>>
    {
        public IEqualityComparer<TKey> KeyComparer { get; }
        public IEqualityComparer<TValue> ValueComparer { get; }
        public ILog Log { get; }


        public DictionaryEqualityComparer(IEqualityComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer, ILog log)
        {
            this.KeyComparer = keyComparer;
            this.ValueComparer = valueComparer;
            this.Log = log;
        }

        public bool Equals(Dictionary<TKey, TValue> x, Dictionary<TKey, TValue> y)
        {
            bool output = true;

            var keysX = new List<TKey>(x.Keys);
            var keysY = new List<TKey>(y.Keys);

            bool keyCountEqual = keysX.Count == keysY.Count;
            if(!keyCountEqual)
            {
                output = false;

                string message = $@"Key count mismatch: x: {keysX.Count.ToString()}, y: {keysY.Count.ToString()}";
                this.Log.WriteLine(message);
            }
            else
            {
                int keyCount = keysX.Count;
                for (int iKey = 0; iKey < keyCount; iKey++)
                {
                    TKey keyX = keysX[iKey];
                    TKey keyY = keysY[iKey];

                    bool keysEqual = this.KeyComparer.Equals(keyX, keyY);
                    if(!keysEqual)
                    {
                        output = false;

                        string message = $@"Key value mismatch: index: {iKey.ToString()}, x: {keyX.ToString()}, y: {keyY.ToString()}";
                        this.Log.WriteLine(message);
                    }
                }
            }

            if(output) // If the keys were equal.
            {
                foreach (var key in keysX)
                {
                    TValue valueX = x[key];
                    TValue valueY = y[key];

                    bool valuesEqual = this.ValueComparer.Equals(valueX, valueY);
                    if (!valuesEqual)
                    {
                        output = false;

                        string message = $@"Value mismatch: key: {key.ToString()}, x: {valueX.ToString()}, y: {valueY.ToString()}";
                        this.Log.WriteLine(message);
                    }
                }
            }

            return output;
        }

        public int GetHashCode(Dictionary<TKey, TValue> obj)
        {
            int output = obj.GetHashCode(); // Use the default.
            return output;
        }
    }
}
