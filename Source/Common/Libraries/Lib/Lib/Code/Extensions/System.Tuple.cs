using System;


namespace Public.Common.Lib.Extensions
{
    public static class TupleExtensions
    {
        public static string ToCsvLine<T1, T2>(this Tuple<T1, T2> tuple)
        {
            string output = $@"{tuple.Item1.ToString()},{tuple.Item2.ToString()}";
            return output;
        }

        public static string ToCsvLine<T1, T2, T3>(this Tuple<T1, T2, T3> tuple)
        {
            string output = $@"{tuple.Item1.ToString()},{tuple.Item2.ToString()},{tuple.Item3.ToString()}";
            return output;
        }

        public static string ToCsvLine<T1, T2, T3, T4>(this Tuple<T1, T2, T3, T4> tuple)
        {
            string output = $@"{tuple.Item1.ToString()},{tuple.Item2.ToString()},{tuple.Item3.ToString()},{tuple.Item4.ToString()}";
            return output;
        }
    }
}
