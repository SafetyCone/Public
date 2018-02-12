using System;


namespace Public.Common.Lib
{
    public static class ObjectExtensions
    {
        public static T ConvertTo<T>(this object obj)
        {
            T output = (T)obj;
            return output;
        }
    }
}
