using System;
using System.Collections.Generic;


namespace Public.Common.Lib
{
    /// <summary>
    /// Static methods using the prime-multiplier algorithm for generating hash codes.
    /// </summary>
    /// <remarks>
    /// Adapted from: http://stackoverflow.com/questions/263400/what-is-the-best-algorithm-for-an-overridden-system-object-gethashcode
    /// 
    /// The 'unchecked' keyword is used throughout to ignore overflow.
    /// </remarks>
    public partial class HashHelperPrimeMultiplier
    {
        #region Static

        private const int zPrimeMultiplier = 31;
        public static int PrimeMultiplier => HashHelperPrimeMultiplier.zPrimeMultiplier;


        public static implicit operator int(HashHelperPrimeMultiplier hashHelper)
        {
            return hashHelper.Value;
        }

        public static HashHelperPrimeMultiplier StartHash()
        {
            HashHelperPrimeMultiplier output = new HashHelperPrimeMultiplier(0);
            return output;
        }

        public static HashHelperPrimeMultiplier StartHash(int hash)
        {
            HashHelperPrimeMultiplier output = new HashHelperPrimeMultiplier(0);
            return output;
        }

        public static HashHelperPrimeMultiplier StartHash<T>(T arg)
        {
            int value = arg.GetHashCode();

            HashHelperPrimeMultiplier output = new HashHelperPrimeMultiplier(value);
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
            return HashHelperPrimeMultiplier.zPrimeMultiplier * priorHash + arg.GetHashCode();
        }

        public static int GetHashCode<T1, T2>(T1 arg1, T2 arg2)
        {
            unchecked
            {
                return HashHelperPrimeMultiplier.zPrimeMultiplier * arg1.GetHashCode() + arg2.GetHashCode();
            }
        }

        public static int GetHashCode<T1, T2>(Tuple<T1, T2> tuple)
        {
            unchecked
            {
                return HashHelperPrimeMultiplier.zPrimeMultiplier * tuple.Item1.GetHashCode() + tuple.Item2.GetHashCode();
            }
        }

        public static int GetHashCode<T1, T2, T3>(T1 arg1, T2 arg2, T3 arg3)
        {
            unchecked
            {
                int output = arg1.GetHashCode();
                output = HashHelperPrimeMultiplier.zPrimeMultiplier * output + arg2.GetHashCode();
                output = HashHelperPrimeMultiplier.zPrimeMultiplier * output + arg3.GetHashCode();

                return output;
            }
        }

        public static int GetHashCode<T1, T2, T3>(Tuple<T1, T2, T3> tuple)
        {
            unchecked
            {
                int output = tuple.Item1.GetHashCode();
                output = HashHelperPrimeMultiplier.zPrimeMultiplier * output + tuple.Item2.GetHashCode();
                output = HashHelperPrimeMultiplier.zPrimeMultiplier * output + tuple.Item3.GetHashCode();

                return output;
            }
        }

        public static int GetHashCode<T1, T2, T3, T4>(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            unchecked
            {
                int output = arg1.GetHashCode();
                output = HashHelperPrimeMultiplier.zPrimeMultiplier * output + arg2.GetHashCode();
                output = HashHelperPrimeMultiplier.zPrimeMultiplier * output + arg3.GetHashCode();
                output = HashHelperPrimeMultiplier.zPrimeMultiplier * output + arg4.GetHashCode();

                return output;
            }
        }

        public static int GetHashCode<T1, T2, T3, T4>(Tuple<T1, T2, T3, T4> tuple)
        {
            unchecked
            {
                int output = tuple.Item1.GetHashCode();
                output = HashHelperPrimeMultiplier.zPrimeMultiplier * output + tuple.Item2.GetHashCode();
                output = HashHelperPrimeMultiplier.zPrimeMultiplier * output + tuple.Item3.GetHashCode();
                output = HashHelperPrimeMultiplier.zPrimeMultiplier * output + tuple.Item4.GetHashCode();

                return output;
            }
        }

        public static int GetHashCode<T1, T2, T3, T4, T5>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            unchecked
            {
                int output = arg1.GetHashCode();
                output = HashHelperPrimeMultiplier.zPrimeMultiplier * output + arg2.GetHashCode();
                output = HashHelperPrimeMultiplier.zPrimeMultiplier * output + arg3.GetHashCode();
                output = HashHelperPrimeMultiplier.zPrimeMultiplier * output + arg4.GetHashCode();
                output = HashHelperPrimeMultiplier.zPrimeMultiplier * output + arg5.GetHashCode();

                return output;
            }
        }

        public static int GetHashCode<T1, T2, T3, T4, T5>(Tuple<T1, T2, T3, T4, T5> tuple)
        {
            unchecked
            {
                int output = tuple.Item1.GetHashCode();
                output = HashHelperPrimeMultiplier.zPrimeMultiplier * output + tuple.Item2.GetHashCode();
                output = HashHelperPrimeMultiplier.zPrimeMultiplier * output + tuple.Item3.GetHashCode();
                output = HashHelperPrimeMultiplier.zPrimeMultiplier * output + tuple.Item4.GetHashCode();
                output = HashHelperPrimeMultiplier.zPrimeMultiplier * output + tuple.Item5.GetHashCode();

                return output;
            }
        }

        public static int GetHashCode<T1, T2, T3, T4, T5, T6>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            unchecked
            {
                int output = arg1.GetHashCode();
                output = HashHelperPrimeMultiplier.zPrimeMultiplier * output + arg2.GetHashCode();
                output = HashHelperPrimeMultiplier.zPrimeMultiplier * output + arg3.GetHashCode();
                output = HashHelperPrimeMultiplier.zPrimeMultiplier * output + arg4.GetHashCode();
                output = HashHelperPrimeMultiplier.zPrimeMultiplier * output + arg5.GetHashCode();
                output = HashHelperPrimeMultiplier.zPrimeMultiplier * output + arg6.GetHashCode();

                return output;
            }
        }

        public static int GetHashCode<T1, T2, T3, T4, T5, T6>(Tuple<T1, T2, T3, T4, T5, T6> tuple)
        {
            unchecked
            {
                int output = tuple.Item1.GetHashCode();
                output = HashHelperPrimeMultiplier.zPrimeMultiplier * output + tuple.Item2.GetHashCode();
                output = HashHelperPrimeMultiplier.zPrimeMultiplier * output + tuple.Item3.GetHashCode();
                output = HashHelperPrimeMultiplier.zPrimeMultiplier * output + tuple.Item4.GetHashCode();
                output = HashHelperPrimeMultiplier.zPrimeMultiplier * output + tuple.Item5.GetHashCode();
                output = HashHelperPrimeMultiplier.zPrimeMultiplier * output + tuple.Item6.GetHashCode();

                return output;
            }
        }

        public static int GetHashCode<T1, T2, T3, T4, T5, T6, T7>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        {
            unchecked
            {
                int output = arg1.GetHashCode();
                output = HashHelperPrimeMultiplier.zPrimeMultiplier * output + arg2.GetHashCode();
                output = HashHelperPrimeMultiplier.zPrimeMultiplier * output + arg3.GetHashCode();
                output = HashHelperPrimeMultiplier.zPrimeMultiplier * output + arg4.GetHashCode();
                output = HashHelperPrimeMultiplier.zPrimeMultiplier * output + arg5.GetHashCode();
                output = HashHelperPrimeMultiplier.zPrimeMultiplier * output + arg6.GetHashCode();
                output = HashHelperPrimeMultiplier.zPrimeMultiplier * output + arg7.GetHashCode();

                return output;
            }
        }

        public static int GetHashCode<T1, T2, T3, T4, T5, T6, T7>(Tuple<T1, T2, T3, T4, T5, T6, T7> tuple)
        {
            unchecked
            {
                int output = tuple.Item1.GetHashCode();
                output = HashHelperPrimeMultiplier.zPrimeMultiplier * output + tuple.Item2.GetHashCode();
                output = HashHelperPrimeMultiplier.zPrimeMultiplier * output + tuple.Item3.GetHashCode();
                output = HashHelperPrimeMultiplier.zPrimeMultiplier * output + tuple.Item4.GetHashCode();
                output = HashHelperPrimeMultiplier.zPrimeMultiplier * output + tuple.Item5.GetHashCode();
                output = HashHelperPrimeMultiplier.zPrimeMultiplier * output + tuple.Item6.GetHashCode();
                output = HashHelperPrimeMultiplier.zPrimeMultiplier * output + tuple.Item7.GetHashCode();

                return output;
            }
        }

        public static int GetHashCode<T1, T2, T3, T4, T5, T6, T7, TRest>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, TRest arg8)
        {
            unchecked
            {
                int output = arg1.GetHashCode();
                output = HashHelperPrimeMultiplier.zPrimeMultiplier * output + arg2.GetHashCode();
                output = HashHelperPrimeMultiplier.zPrimeMultiplier * output + arg3.GetHashCode();
                output = HashHelperPrimeMultiplier.zPrimeMultiplier * output + arg4.GetHashCode();
                output = HashHelperPrimeMultiplier.zPrimeMultiplier * output + arg5.GetHashCode();
                output = HashHelperPrimeMultiplier.zPrimeMultiplier * output + arg6.GetHashCode();
                output = HashHelperPrimeMultiplier.zPrimeMultiplier * output + arg7.GetHashCode();
                output = HashHelperPrimeMultiplier.zPrimeMultiplier * output + arg8.GetHashCode();

                return output;
            }
        }

        public static int GetHashCode<T1, T2, T3, T4, T5, T6, T7, TRest>(Tuple<T1, T2, T3, T4, T5, T6, T7, TRest> tuple)
        {
            unchecked
            {
                int output = tuple.Item1.GetHashCode();
                output = HashHelperPrimeMultiplier.zPrimeMultiplier * output + tuple.Item2.GetHashCode();
                output = HashHelperPrimeMultiplier.zPrimeMultiplier * output + tuple.Item3.GetHashCode();
                output = HashHelperPrimeMultiplier.zPrimeMultiplier * output + tuple.Item4.GetHashCode();
                output = HashHelperPrimeMultiplier.zPrimeMultiplier * output + tuple.Item5.GetHashCode();
                output = HashHelperPrimeMultiplier.zPrimeMultiplier * output + tuple.Item6.GetHashCode();
                output = HashHelperPrimeMultiplier.zPrimeMultiplier * output + tuple.Item7.GetHashCode();
                output = HashHelperPrimeMultiplier.zPrimeMultiplier * output + tuple.Rest.GetHashCode();

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
                    output = HashHelperPrimeMultiplier.zPrimeMultiplier * output + item.GetHashCode();
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

                output = HashHelperPrimeMultiplier.zPrimeMultiplier * output + count.GetHashCode();

                return output;
            }
        }

        #endregion


        public int Value { get; set; }


        public HashHelperPrimeMultiplier() { }

        public HashHelperPrimeMultiplier(int value)
        {
            this.Value = value;
        }

        public HashHelperPrimeMultiplier CombineHash<T>(T arg)
        {
            unchecked
            {
                this.Value = HashHelperPrimeMultiplier.zPrimeMultiplier * this.Value + arg.GetHashCode();
            }

            return this;
        }
    }
}
