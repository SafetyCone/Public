using System;
using System.Text;


namespace Public.Common.Lib.Extensions
{
    public static class StringBuilderExtensions
    {
        public static void RemoveLast(this StringBuilder builder)
        {
            builder.Remove(builder.Length - 1, 1);
        }
    }
}
