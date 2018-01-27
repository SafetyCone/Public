using System;


namespace Public.Common.Lib.Extensions
{
    public static class ArrayExtensions
    {
        public static int[] GetLengths(this Array array)
        {
            int numDimensions = array.Rank;

            int[] output = new int[numDimensions];
            for (int iDimension = 0; iDimension < numDimensions; iDimension++)
            {
                output[iDimension] = array.GetLength(iDimension);
            }

            return output;
        }

        public static Type GetElementType(this Array array)
        {
            Type output = array.GetType().GetElementType();
            return output;
        }

        public static void OnEach<T>(this T[] source, Func<T, T> function)
        {
            int numberOfElements = source.Length;
            for (int iElement = 0; iElement < numberOfElements; iElement++)
            {
                source[iElement] = function(source[iElement]);
            }
        }

        public static T[] Copy<T>(this T[] source)
        {
            int numberOfElements = source.Length;

            T[] output = new T[numberOfElements];
            Array.Copy(source, output, numberOfElements);

            return output;
        }
    }
}
