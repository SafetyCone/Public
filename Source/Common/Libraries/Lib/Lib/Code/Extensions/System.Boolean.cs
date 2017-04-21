using System;


namespace Public.Common.Lib.Extensions
{
    public static class BooleanExtensions
    {
		public static string ToStringLower(this bool value)
        {
            string output = value.ToString().ToLowerInvariant();
            return output;
        }
    }
}
