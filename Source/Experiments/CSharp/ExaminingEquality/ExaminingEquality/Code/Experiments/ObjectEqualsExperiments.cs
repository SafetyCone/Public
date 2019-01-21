using System;


namespace ExaminingEquality
{
    /// <summary>
    /// Experiments with <see cref="Object.Equals(object)"/> and <see cref="Object.Equals(object, object)"/>, in order from most basic to most complex.
    /// </summary>
    public static class ObjectEqualsExperiments
    {
        public static void SubMain()
        {
            ObjectEqualsExperiments.NullObjectEqualsNull();
            //ObjectEquals.ReferenceTypeSameInstanceEquality();
            //ObjectEquals.ReferenceTypeDifferentInstanceInequality();
        }

        /// <summary>
        /// Result: True.
        /// Does null <see cref="Object.Equals(object, object)"/> null?
        /// Expected: True, because null is a reference (and thus, if it had a type, would be a reference-type) and <see cref="Object.Equals(object)"/> uses reference-equality for reference-types.
        /// 
        /// 
        /// </summary>
        private static void NullObjectEqualsNull()
        {
            var isEqual = Object.Equals(null, null);

            Console.WriteLine($@"Object.Equals(null, null): {isEqual}");
        }

        // does null Object.ReferenceEquals() null?

        /// <summary>
        /// Result: True.
        /// Are two reference-type variables that reference the same instance <see cref="Object.Equals(object)"/>?
        /// Expected: True, because reference-types use reference-equality by default.
        /// 
        /// Statement of reference-types using reference-equality for implementation of <see cref="Object.Equals(object)"/>: https://docs.microsoft.com/en-us/dotnet/api/system.object.equals?view=netframework-4.7.2#System_Object_Equals_System_Object_
        /// </summary>
        private static void ReferenceTypeSameInstanceEquality()
        {
            // Note, one constructor call via 'new', and one assignment.
            var a1 = new object();
            var a2 = a1;

            var a2EqualsA1 = a2.Equals(a1);

            Console.WriteLine($@"Object a2.Equals(a1)? {a2EqualsA1}");
        }

        /// <summary>
        /// Result: False.
        /// Are two reference-type variables that reference different instances <see cref="Object.Equals(object)"/>?
        /// Expected: False, because reference-types use reference-equality by default.
        /// 
        /// Statement of reference-types using reference-equality for implementation of <see cref="Object.Equals(object)"/>: https://docs.microsoft.com/en-us/dotnet/api/system.object.equals?view=netframework-4.7.2#System_Object_Equals_System_Object_
        /// </summary>
        private static void ReferenceTypeDifferentInstanceInequality()
        {
            // Note, two constructor calls via 'new'.
            var a1 = new object();
            var a2 = new object();

            var a2EqualsA1 = a2.Equals(a1);

            Console.WriteLine($@"Object a2.Equals(a1)? {a2EqualsA1}");
        }
    }
}
