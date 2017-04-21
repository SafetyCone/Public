using System;
using System.Collections.Generic;
using System.Linq;


namespace Public.Common.Lib.Extensions
{
    public static class DictionaryExtensions
    {
        public static void CopyContents<TKey, TValue>(this Dictionary<TKey, TValue> source, Dictionary<TKey, TValue> destination)
        {
            foreach (TKey key in source.Keys)
            {
                TValue value = source[key];
                destination.Add(key, value);
            }
        }
    }
}
