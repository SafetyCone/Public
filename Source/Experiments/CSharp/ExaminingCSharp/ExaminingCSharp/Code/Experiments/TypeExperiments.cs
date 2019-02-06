using System;
using System.Collections.Generic;

using ExaminingClasses.Lib;


namespace ExaminingCSharp
{
    public static class TypeExperiments
    {
        public static void SubMain()
        {
            TypeExperiments.HowDoTypesCompare();
        }

        ///// <summary>
        ///// Result:
        ///// Can <see cref="Type"/> instances be compared? Presumably comparison of <see cref="Type"/> instances would work via the inheritance hierarchy.
        ///// Expected: Yes, the comparison operators should work on types in 
        ///// </summary>
        //private static void HowDoTypesCompare()
        //{
        //    var typeOfObject = typeof(object);
        //    var typeOfClassA = typeof(ClassA);
        //    var typeOfClassB = typeof(ClassB);
        //    var typeOfString = typeof(string);

        //    var objectToString = typeOfObject < typeOfString;
        //}

        /// <summary>
        /// Result: UNEXPECTED! The <see cref="Type"/> type does NOT implement IComparable.
        /// Can <see cref="Type"/> instances be compared? Presumably comparison of <see cref="Type"/> instances would work via the inheritance hierarchy.
        /// Expected: Comparison operators should work on <see cref="Type"/> instances.
        /// </summary>
        private static void HowDoTypesCompare()
        {
            var typeOfObject = typeof(object);
            var typeOfString = typeof(string);

            //var objectAndString1 = typeOfObject < typeOfString; // Operator '<' cannot be applied to operands of type 'Type' and 'Type'.

            var defaultTypeComparer = Comparer<Type>.Default;
            var objectAndString2 = defaultTypeComparer.Compare(typeOfObject, typeOfString); // System.ArgumentException: 'At least one object must implement IComparable.'
        }
    }
}
