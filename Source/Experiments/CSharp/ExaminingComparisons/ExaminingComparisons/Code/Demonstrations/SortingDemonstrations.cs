using System;
using System.Collections.Generic;

using ExaminingClasses.Lib;


namespace ExaminingComparisons
{
    public static class SortingDemonstrations
    {
        public static void SubMain()
        {
            //SortingDemonstrations.CanNonComparableTypesBeSorted();
            SortingDemonstrations.CanComparableTypesBeSorted();
        }

        /// <summary>
        /// Result: Yes.
        /// If a type implements <see cref="IComparable"/> or <see cref="IComparable{T}"/>, where &lt;T&gt; is the type itself, then it should be sortable.
        /// Expected: Yes.
        /// 
        /// 
        /// </summary>
        private static void CanComparableTypesBeSorted()
        {
            var listOfCustomTypeInstances = new List<ComparableClass>()
            {
                // Created out of sort order.
                new ComparableClass()
                {
                    Value1 = 3,
                    Value2 = 4,
                },
                new ComparableClass()
                {
                    Value1 = 1,
                    Value2 = 2,
                },
            };

            listOfCustomTypeInstances.Sort();
        }

        /// <summary>
        /// Result: No. An exception is thrown!
        /// Can types that do not implement IComparable&lt;T&gt; or IComparable be sorted in collections like lists?
        /// The .NET documentation suggests that if an attempt is made to sort a list of instances of a custom type that does not implement IComparable, then an exception will be thrown.
        /// Expected: Yes, no errors.
        /// 
        /// Suggests that custom types that do not implement IComparable will cause exceptions to be thrown if an attempt is made to sort them: https://docs.microsoft.com/en-us/dotnet/api/system.icomparable.compareto?view=netframework-4.7.2
        /// </summary>
        private static void CanNonComparableTypesBeSorted()
        {
            var listOfCustomTypeInstances = new List<ClassA>()
            {
                // Created out of sort order.
                new ClassA()
                {
                    ReferenceTypeValueA = @"2",
                    ValueTypeValueA = 2,
                },
                new ClassA()
                {
                    ReferenceTypeValueA = @"1",
                    ValueTypeValueA = 1,
                },
            };

            listOfCustomTypeInstances.Sort(); // System.InvalidOperationException: 'Failed to compare two elements in the array.'
        }
    }
}
