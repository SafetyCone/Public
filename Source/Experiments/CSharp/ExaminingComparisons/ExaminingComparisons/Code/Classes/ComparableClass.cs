using System;


namespace ExaminingComparisons
{
    /// <summary>
    /// A basic comparable class that implements <see cref="IComparable{T}"/>.
    /// </summary>
    public class ComparableClass : IComparable<ComparableClass>
    {
        public int Value1 { get; set; }
        public int Value2 { get; set; }


        public int CompareTo(ComparableClass other)
        {
            if (object.ReferenceEquals(this, other))
            {
                return 0;
            }

            if (other is null)
            {
                return 1; // Any non-null object > null.
            }

            int output = 0;

            // Use either member comparison or...
            output = this.Value1.CompareTo(other.Value1);
            if (0 != output)
            {
                return output;
            }

            output = this.Value2.CompareTo(other.Value2);
            return output;
        }
    }
}
