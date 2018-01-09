

namespace Public.Common.Lib
{
    /// <summary>
    /// Static methods using the XOR operator for generating hash codes.
    /// </summary>
    /// <remarks>
    /// Adapted from: Effective C#: 50 Specific Ways to Improve your C#, 3rd Edition - Item 26: Implement Classic Interfaces in Addition to Generic Interfaces.
    /// </remarks>
    public class HashHelper
    {
        #region Static

        public static int GetHashCode<T>(int priorHash, T arg)
        {
            int output = priorHash ^ arg.GetHashCode();
            return output;
        }

        public static int GetHashCode<T1, T2>(T1 arg1, T2 arg2)
        {
            int output = 0;
            output ^= arg1.GetHashCode();
            output ^= arg2.GetHashCode();

            return output;
        }

        public static int GetHashCode<T1, T2, T3>(T1 arg1, T2 arg2, T3 arg3)
        {
            int output = 0;
            output ^= arg1.GetHashCode();
            output ^= arg2.GetHashCode();
            output ^= arg3.GetHashCode();

            return output;
        }

        public static int GetHashCode<T1, T2, T3, T4>(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            unchecked
            {
                int output = 0;
                output ^= arg1.GetHashCode();
                output ^= arg2.GetHashCode();
                output ^= arg3.GetHashCode();
                output ^= arg4.GetHashCode();

                return output;
            }
        }

        #endregion
    }
}
