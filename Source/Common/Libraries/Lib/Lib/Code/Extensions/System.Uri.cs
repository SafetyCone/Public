using System;


namespace Public.Common.Lib
{
    public static class UriExtensions
    {
        public const char DefaultUriPathSeparator = '/';


		public static string Combine(string uri1, string uri2)
        {
            string trimmedUri1 = uri1.TrimEnd(UriExtensions.DefaultUriPathSeparator);
            string trimmedUri2 = uri2.TrimStart(UriExtensions.DefaultUriPathSeparator);

            string output = String.Format(@"{0}{1}{2}", trimmedUri1, UriExtensions.DefaultUriPathSeparator, trimmedUri2);
            return output;
        }
    }
}
