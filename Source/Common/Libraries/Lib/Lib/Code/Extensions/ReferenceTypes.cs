using System;


namespace Public.Common.Lib.Extensions
{
    public static class ReferenceTypeExtensions
    {
        public static void ThrowIfNull<T>(this T obj)
            where T: class
        {
            if(null == obj)
            {
                throw new NullReferenceException();
            }
        }

        public static void ThrowIfNull<T>(this T obj, string objectName)
            where T : class
        {
            if (null == obj)
            {
                throw new NullReferenceException($@"{objectName} was null.");
            }
        }
    }
}
