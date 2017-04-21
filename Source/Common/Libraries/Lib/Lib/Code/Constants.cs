using System;


namespace Public.Common.Lib
{
    public static class Constants
    {
        /// <summary>
        /// Similar to System.IO.Path.DirectorySeparatorChar, but for separating a file name from its extenstion.
        /// </summary>
        /// <remarks>
        /// The .NET source code simply hard-codes this (to '.'), but I don't like that.
        /// NOTE: There may be multiple periods in a file name. Only the last token when separated is the file extension.
        /// </remarks>
        public const char WindowsFileExtensionSeparatorChar = '.';
    }
}
