using System;
using System.Collections.Generic;


namespace Public.Common.Lib.Extensions
{
    public static class IEnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T element in source)
            {
                action(element);
            }
        }

        public static IEnumerable<T> EveryNth<T>(this IEnumerable<T> source, int nth)
        {
            int count = 0;
            foreach (T element in source)
            {
                if(count % nth == 0)
                {
                    yield return element;
                }
                count++;
            }
        }

        public static IEnumerable<T> Select<T>(this IEnumerable<T> source, ISelector selector)
        {
            int count = 0;
            foreach(T element in source)
            {
                if(selector[count])
                {
                    yield return element;
                }

                count++;
            }
        }

        public static int Product(this IEnumerable<int> source)
        {
            int output = 1;
            source.ForEach((x) => output *= x);

            return output;
        }
    }
}
