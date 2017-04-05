using System;
using System.Collections.Generic;


namespace Public.Common.Lib
{
    /// <summary>
    /// Static methods using an algorithm for generating hash codes.
    /// </summary>
    /// <remarks>
    /// Adapted from:
    /// http://stackoverflow.com/questions/263400/what-is-the-best-algorithm-for-an-overridden-system-object-gethashcode
    /// 
    /// The 'unchecked' keyword is used throughout to ignore overflow.
    /// </remarks>
    public partial class HashHelper
    {
        public const int PrimeMultiplier = 31;


        #region Static

        public static implicit operator int(HashHelper hashHelper)
        {
            return hashHelper.Value;
        }

        public static HashHelper StartHash()
        {
            HashHelper output = new HashHelper(0);
            return output;
        }

        public static HashHelper StartHash(int hash)
        {
            HashHelper output = new HashHelper(0);
            return output;
        }

        public static HashHelper StartHash<T>(T arg)
        {
            int value = arg.GetHashCode();

            HashHelper output = new HashHelper(value);
            return output;
        }

        #region GetHashCode Generic Methods

        public static int GetHashCode<T>(T arg)
        {
            return arg.GetHashCode();
        }

        public static int GetHashCode<T>(Tuple<T> tuple)
        {
            return tuple.Item1.GetHashCode();
        }

        public static int GetHashCode<T>(int priorHash, T arg)
        {
            return HashHelper.PrimeMultiplier * priorHash + arg.GetHashCode();
        }

        public static int GetHashCode<T1, T2>(T1 arg1, T2 arg2)
        {
            unchecked
            {
                return HashHelper.PrimeMultiplier * arg1.GetHashCode() + arg2.GetHashCode();
            }
        }

        public static int GetHashCode<T1, T2>(Tuple<T1, T2> tuple)
        {
            unchecked
            {
                return HashHelper.PrimeMultiplier * tuple.Item1.GetHashCode() + tuple.Item2.GetHashCode();
            }
        }

        public static int GetHashCode<T1, T2, T3>(T1 arg1, T2 arg2, T3 arg3)
        {
            unchecked
            {
                int output = arg1.GetHashCode();
                output = HashHelper.PrimeMultiplier * output + arg2.GetHashCode();
                output = HashHelper.PrimeMultiplier * output + arg3.GetHashCode();

                return output;
            }
        }

        public static int GetHashCode<T1, T2, T3>(Tuple<T1, T2, T3> tuple)
        {
            unchecked
            {
                int output = tuple.Item1.GetHashCode();
                output = HashHelper.PrimeMultiplier * output + tuple.Item2.GetHashCode();
                output = HashHelper.PrimeMultiplier * output + tuple.Item3.GetHashCode();

                return output;
            }
        }

        public static int GetHashCode<T1, T2, T3, T4>(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            unchecked
            {
                int output = arg1.GetHashCode();
                output = HashHelper.PrimeMultiplier * output + arg2.GetHashCode();
                output = HashHelper.PrimeMultiplier * output + arg3.GetHashCode();
                output = HashHelper.PrimeMultiplier * output + arg4.GetHashCode();

                return output;
            }
        }

        public static int GetHashCode<T1, T2, T3, T4>(Tuple<T1, T2, T3, T4> tuple)
        {
            unchecked
            {
                int output = tuple.Item1.GetHashCode();
                output = HashHelper.PrimeMultiplier * output + tuple.Item2.GetHashCode();
                output = HashHelper.PrimeMultiplier * output + tuple.Item3.GetHashCode();
                output = HashHelper.PrimeMultiplier * output + tuple.Item4.GetHashCode();

                return output;
            }
        }

        public static int GetHashCode<T1, T2, T3, T4, T5>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            unchecked
            {
                int output = arg1.GetHashCode();
                output = HashHelper.PrimeMultiplier * output + arg2.GetHashCode();
                output = HashHelper.PrimeMultiplier * output + arg3.GetHashCode();
                output = HashHelper.PrimeMultiplier * output + arg4.GetHashCode();
                output = HashHelper.PrimeMultiplier * output + arg5.GetHashCode();

                return output;
            }
        }

        public static int GetHashCode<T1, T2, T3, T4, T5>(Tuple<T1, T2, T3, T4, T5> tuple)
        {
            unchecked
            {
                int output = tuple.Item1.GetHashCode();
                output = HashHelper.PrimeMultiplier * output + tuple.Item2.GetHashCode();
                output = HashHelper.PrimeMultiplier * output + tuple.Item3.GetHashCode();
                output = HashHelper.PrimeMultiplier * output + tuple.Item4.GetHashCode();
                output = HashHelper.PrimeMultiplier * output + tuple.Item5.GetHashCode();

                return output;
            }
        }

        public static int GetHashCode<T1, T2, T3, T4, T5, T6>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            unchecked
            {
                int output = arg1.GetHashCode();
                output = HashHelper.PrimeMultiplier * output + arg2.GetHashCode();
                output = HashHelper.PrimeMultiplier * output + arg3.GetHashCode();
                output = HashHelper.PrimeMultiplier * output + arg4.GetHashCode();
                output = HashHelper.PrimeMultiplier * output + arg5.GetHashCode();
                output = HashHelper.PrimeMultiplier * output + arg6.GetHashCode();

                return output;
            }
        }

        public static int GetHashCode<T1, T2, T3, T4, T5, T6>(Tuple<T1, T2, T3, T4, T5, T6> tuple)
        {
            unchecked
            {
                int output = tuple.Item1.GetHashCode();
                output = HashHelper.PrimeMultiplier * output + tuple.Item2.GetHashCode();
                output = HashHelper.PrimeMultiplier * output + tuple.Item3.GetHashCode();
                output = HashHelper.PrimeMultiplier * output + tuple.Item4.GetHashCode();
                output = HashHelper.PrimeMultiplier * output + tuple.Item5.GetHashCode();
                output = HashHelper.PrimeMultiplier * output + tuple.Item6.GetHashCode();

                return output;
            }
        }

        public static int GetHashCode<T1, T2, T3, T4, T5, T6, T7>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        {
            unchecked
            {
                int output = arg1.GetHashCode();
                output = HashHelper.PrimeMultiplier * output + arg2.GetHashCode();
                output = HashHelper.PrimeMultiplier * output + arg3.GetHashCode();
                output = HashHelper.PrimeMultiplier * output + arg4.GetHashCode();
                output = HashHelper.PrimeMultiplier * output + arg5.GetHashCode();
                output = HashHelper.PrimeMultiplier * output + arg6.GetHashCode();
                output = HashHelper.PrimeMultiplier * output + arg7.GetHashCode();

                return output;
            }
        }

        public static int GetHashCode<T1, T2, T3, T4, T5, T6, T7>(Tuple<T1, T2, T3, T4, T5, T6, T7> tuple)
        {
            unchecked
            {
                int output = tuple.Item1.GetHashCode();
                output = HashHelper.PrimeMultiplier * output + tuple.Item2.GetHashCode();
                output = HashHelper.PrimeMultiplier * output + tuple.Item3.GetHashCode();
                output = HashHelper.PrimeMultiplier * output + tuple.Item4.GetHashCode();
                output = HashHelper.PrimeMultiplier * output + tuple.Item5.GetHashCode();
                output = HashHelper.PrimeMultiplier * output + tuple.Item6.GetHashCode();
                output = HashHelper.PrimeMultiplier * output + tuple.Item7.GetHashCode();

                return output;
            }
        }

        public static int GetHashCode<T1, T2, T3, T4, T5, T6, T7, TRest>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, TRest arg8)
        {
            unchecked
            {
                int output = arg1.GetHashCode();
                output = HashHelper.PrimeMultiplier * output + arg2.GetHashCode();
                output = HashHelper.PrimeMultiplier * output + arg3.GetHashCode();
                output = HashHelper.PrimeMultiplier * output + arg4.GetHashCode();
                output = HashHelper.PrimeMultiplier * output + arg5.GetHashCode();
                output = HashHelper.PrimeMultiplier * output + arg6.GetHashCode();
                output = HashHelper.PrimeMultiplier * output + arg7.GetHashCode();
                output = HashHelper.PrimeMultiplier * output + arg8.GetHashCode();

                return output;
            }
        }

        public static int GetHashCode<T1, T2, T3, T4, T5, T6, T7, TRest>(Tuple<T1, T2, T3, T4, T5, T6, T7, TRest> tuple)
        {
            unchecked
            {
                int output = tuple.Item1.GetHashCode();
                output = HashHelper.PrimeMultiplier * output + tuple.Item2.GetHashCode();
                output = HashHelper.PrimeMultiplier * output + tuple.Item3.GetHashCode();
                output = HashHelper.PrimeMultiplier * output + tuple.Item4.GetHashCode();
                output = HashHelper.PrimeMultiplier * output + tuple.Item5.GetHashCode();
                output = HashHelper.PrimeMultiplier * output + tuple.Item6.GetHashCode();
                output = HashHelper.PrimeMultiplier * output + tuple.Item7.GetHashCode();
                output = HashHelper.PrimeMultiplier * output + tuple.Rest.GetHashCode();

                return output;
            }
        }
        
        #endregion

        public static int GetHashCode<T>(IEnumerable<T> enumerable)
        {
            unchecked
            {
                int output = 0;
                foreach (T item in enumerable)
                {
                    output = HashHelper.PrimeMultiplier * output + item.GetHashCode();
                }

                return output;
            }
        }

        public static int GetHashCodeOrderIrrelevant<T>(IEnumerable<T> enumerable)
        {
            unchecked
            {
                int output = 0;
                int count = 0;
                foreach (T item in enumerable)
                {
                    output += item.GetHashCode();
                    count++;
                }

                output = HashHelper.PrimeMultiplier * output + count.GetHashCode();

                return output;
            }
        }

        #endregion


        public int Value { get; set; }


        public HashHelper() { }

        public HashHelper(int value)
        {
            this.Value = value;
        }

        public HashHelper CombineHash<T>(T arg)
        {
            unchecked
            {
                this.Value = HashHelper.PrimeMultiplier * this.Value + arg.GetHashCode();
            }

            return this;
        }
    }
}
