using System;
using System.IO;
using System.Text;


// Standard namespace for all extensions.
namespace Public.Common.Lib.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Splits a line based on a separator.
        /// </summary>
        /// <remarks>
        /// This is just a named call to String.Split().
        /// </remarks>
        public static string[] TokenizeLine(this string line, char separator)
        {
            string[] tokens = line.Split(separator); // StringSplitOptions.None is used by default.
            return tokens;
        }

        /// <summary>
        /// Concatenates tokens into a single line using a separator.
        /// </summary>
        /// <remarks>
        /// This is more than just string concatenation. Some optimizations with regard to the length of the tokens array are performed.
        /// </remarks>
        public static string LinearizeTokens(this string[] tokens, char separator)
        {
            string output = tokens.LinearizeTokens(separator.ToString());
            return output;
        }

        /// <summary>
        /// Concatenates tokens into a single line using a separator.
        /// </summary>
        /// <remarks>
        /// This is more than just string concatenation. Some optimizations with regard to the length of the tokens array are performed.
        /// </remarks>
        public static string LinearizeTokens(this string[] tokens, string separator)
        {
            if (0 == tokens.Length)
            {
                return string.Empty;
            }

            if (1 == tokens.Length)
            {
                return tokens[0];
            }

            StringBuilder builder = new StringBuilder(tokens[0]);
            for (int iToken = 1; iToken < tokens.Length; iToken++)
            {
                string appendage = string.Format(@"{0}{1}", separator, tokens[iToken]);
                builder.Append(appendage);
            }

            string output = builder.ToString();
            return output;
        }

        /// <summary>
        /// String concatenation optimized for the length of the substring array.
        /// </summary>
        public static string Concatenate(this string[] subStrings, char separator)
        {
            string output = StringExtensions.Concatenate(subStrings, separator.ToString());
            return output;
        }

        /// <summary>
        /// String concatenation optimized for the length of the substring array.
        /// </summary>
        public static string Concatenate(this string[] subStrings, string separator)
        {
            string output = StringExtensions.LinearizeTokens(subStrings, separator);
            return output;
        }

        /// <summary>
        /// Splits a line based on a path separator.
        /// </summary>
        /// <remarks>
        /// This is just a named call to String.Split().
        /// </remarks>
        public static string[] SplitPath(this string path, char pathSeparator)
        {
            string[] pathParts = path.Split(pathSeparator); // StringSplitOptions.None is used by default.
            return pathParts;
        }

        /// <summary>
        /// Splits a path using the Path.DirectorySeparatorChar.
        /// </summary>
        public static string[] SplitPath(this string path)
        {
            string[] output = path.SplitPath(Path.DirectorySeparatorChar);
            return output;
        }

        /// <remarks>
        /// This is a call into the DateTime functions of the common utilities class.
        /// </remarks>
        public static DateTime ToDateTimeFromYYYYMMDD(this string yyyymmdd)
        {
            DateTime output = Utilities.DateTimeFromYYYYMMDD(yyyymmdd);
            return output;
        }

        public static string PrettyPrint(this string[] strings)
        {
            if (1 > strings.Length)
            {
                return @"{ } <Empty>";
            }

            if (2 > strings.Length)
            {
                return string.Format(@" {{ {0} }}", strings[0]);
            }

            string appendix;

            appendix = string.Format(@"{{ {0}", strings[0]);
            StringBuilder builder = new StringBuilder(appendix);
            for (int iString = 1; iString < strings.Length; iString++)
            {
                appendix = string.Format(@", {0}", strings[iString]);
                builder.Append(appendix);
            }
            builder.Append(@" }");

            string output = builder.ToString();
            return output;
        }
    }
}
