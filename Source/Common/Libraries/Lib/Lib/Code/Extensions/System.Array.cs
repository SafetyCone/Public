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
    }
}
