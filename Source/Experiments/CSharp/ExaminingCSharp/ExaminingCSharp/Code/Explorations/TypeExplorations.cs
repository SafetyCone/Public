using System;

using ExaminingClasses.Lib;


namespace ExaminingCSharp
{
    public static class TypeExplorations
    {
        public static void SubMain()
        {

        }

        private static void SelectBestMatch()
        {
            var typeOfObject = typeof(object);
            var typeOfClassA = typeof(ClassA);
            var typeOfClassB = typeof(ClassB);
            var typeOfString = typeof(string);

            var allTypes = new[] { typeOfObject, typeOfClassA, typeOfClassB, typeOfString };
            
            // Find type that best matches a test-type in array of types:
            // Get list of types in the test-types inheritance hierarchy, starting from the test type and going all the way to object.
            // For the inheritance hierachy types, test whether the input types array contains that type.

        }
    }
}
