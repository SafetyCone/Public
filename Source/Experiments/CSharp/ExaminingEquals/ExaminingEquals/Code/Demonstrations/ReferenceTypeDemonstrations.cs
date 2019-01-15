using System;


namespace ExaminingEquals
{
    public static class ReferenceTypeDemonstrations
    {
        public static void SubMain()
        {
            ReferenceTypeDemonstrations.ObjectEqualsReferenceEquality();
            ReferenceTypeDemonstrations.ObjectEqualsSameAsObjectReferenceEquals();
        }

        private static void ObjectEqualsSameAsObjectReferenceEquals()
        {

        }

        /// <summary>
        /// Result: True.
        /// Test whether two references to the same reference-type instance are Object.Equals().
        /// </summary>
        private static void ObjectEqualsReferenceEquality()
        {
            object a1 = new object();
            object a2 = a1;

            var a2EqualsA1 = a2.Equals(a1);
            Console.WriteLine($@"Object a2.Equals(a1)? {a2EqualsA1}");
        }
    }
}
