using System;
using System.Collections.Generic;


namespace Public.Examples.Code
{
    /// <summary>
    /// An example comparable class.
    /// When you implement IComparable you should implement the comparison operators '&lt;', '&gt;', '&lt;=', and '&gt;='.
    /// 
    /// Shows:
    /// * Typed vs. generic field comparisons.
    /// * Also implements the classic non-generic base interface.
    /// * Operators &lt;, &gt;, &lt;=, and &gt;=.
    /// </summary>
    /// <typeparam name="T">A possible generically typed property.</typeparam>
    /// <remarks>
    /// Adapted from Effective C#: 50 Specific Ways to Improve your C#, 3rd Edition - Item 26: Implement Classic Interfaces in Addition to Generic Interfaces.
    /// </remarks>
    public class ComparableClass<T> : IComparable<ComparableClass<T>>, IComparable
    {
        #region Static

        public static bool operator <(ComparableClass<T> lhs, ComparableClass<T> rhs)
        {
            bool output;
            if(lhs == null)
            {
                output = rhs == null;
            }
            else
            {
                output = lhs.CompareTo(rhs) < 0;
            }

            return output;
        }

        public static bool operator >(ComparableClass<T> lhs, ComparableClass<T> rhs)
        {
            bool output;
            if(lhs == null)
            {
                output = false; // If lhs is null, and null is less than any non-null object, then we have our answer.
            }
            else
            {
                output = lhs.CompareTo(rhs) > 0;
            }

            return output;
        }

        public static bool operator <=(ComparableClass<T> lhs, ComparableClass<T> rhs)
        {
            bool output;
            if(lhs == null)
            {
                output = true; // If lhs is null, rhs is either null or non-null. If null, then null is equal to null, and since non-null is greater than any null, this is true.
            }
            else
            {
                output = lhs.CompareTo(rhs) <= 0;
            }

            return output;
        }

        public static bool operator >=(ComparableClass<T> lhs, ComparableClass<T> rhs)
        {
            bool output;
            if (lhs == null)
            {
                output = rhs == null;
            }
            else
            {
                output = lhs.CompareTo(rhs) >= 0;
            }

            return output;
        }

        #endregion


        public double X { get; set; }
        public T Y { get; set; }


        public int CompareTo(ComparableClass<T> other)
        {
            // If the type is a reference-type, use this short-circuit to disquality identical instances.
            if(object.ReferenceEquals(this, other))
            {
                return 0;
            }

            if(other is null)
            {
                return 1; // Any non-null object > null.
            }

            int output = 0;

            // Use either member comparison or...
            output = this.X.CompareTo(other.X);
            if(0 != output)
            {
                return output;
            }

            // Use the default comparer for a generic type.
            output = Comparer<T>.Default.Compare(this.Y, other.Y);
            if (0 != output)
            {
                return output;
            }

            return output;
        }

        int IComparable.CompareTo(object obj)
        {
            // Enforce strict typing or...
            if(obj.GetType() != typeof(ComparableClass<T>))
            {
                throw new ArgumentException($@"Argument is of wrong type. Required: {typeof(ComparableClass<T>).FullName}, found: {obj.GetType().FullName}.");
            }

            // Allow descendents by removing the above.
            int output = this.CompareTo(obj as ComparableClass<T>);
            return output;
        }
    }
}
