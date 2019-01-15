using System;


namespace ExaminingEquals
{
    public static class ReferenceTypeDemonstrations
    {
        public static void SubMain()
        {
            //ReferenceTypeDemonstrations.ObjectEqualsSameInstanceReferenceEquality();
            ReferenceTypeDemonstrations.ObjectEqualsDifferentInstanceReferenceInequality();

            //ReferenceTypeDemonstrations.ObjectEqualsSameAsObjectReferenceEquals();
        }

        private static void ObjectEqualsSameAsObjectReferenceEquals()
        {

        }

        /// <summary>
        /// Result:
        /// Test whether two reference-type variables that reference different instances are Object.Equals().
        /// Expected: False, because reference-types use reference-equality by default.
        /// 
        /// Statement of reference-types using reference-equality for implementation of Object.Equals(): https://docs.microsoft.com/en-us/dotnet/api/system.object.equals?view=netframework-4.7.2#System_Object_Equals_System_Object_
        /// </summary>
        private static void ObjectEqualsDifferentInstanceReferenceInequality()
        {
            // Note, two constructor calls via 'new'.
            var a1 = new object();
            var a2 = new object();

            var a2EqualsA1 = a2.Equals(a1);
            Console.WriteLine($@"Object a2.Equals(a1)? {a2EqualsA1}");
        }

        /// <summary>
        /// Result: True.
        /// Test whether two reference-type variables that reference the same instance are Object.Equals().
        /// Expected: True, because reference-types use reference-equality by default.
        /// 
        /// Statement of reference-types using reference-equality for implementation of Object.Equals(): https://docs.microsoft.com/en-us/dotnet/api/system.object.equals?view=netframework-4.7.2#System_Object_Equals_System_Object_
        /// </summary>
        private static void ObjectEqualsSameInstanceReferenceEquality()
        {
            // Note, one constructor call via 'new', and one assignment.
            var a1 = new object();
            var a2 = a1;

            var a2EqualsA1 = a2.Equals(a1);
            Console.WriteLine($@"Object a2.Equals(a1)? {a2EqualsA1}");
        }
    }
}
