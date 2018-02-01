using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace Public.Common.Lib.IO
{
    public static class Utilities
    {
        ///// <summary>
        ///// Read a text file containing a list of image file paths. Each of the paths contained in the file will be processed.
        ///// </summary>
        ///// 

        /// <summary>
        /// Read a text file containing lines, removing blank lines.
        /// </summary>
        /// <param name="textFilePath">The path of a text file.</param>
        /// <param name="removeBlankLines">Optionally remove blank lines.</param>
        /// <returns>A plain string array of lines within a file.</returns>
        public static string[] GetLines(string textFilePath, bool removeBlankLines = true)
        {
            string[] allLines = File.ReadAllLines(textFilePath);

            string[] output;
            if(removeBlankLines)
            {
                output = allLines.Where((line) => !String.IsNullOrWhiteSpace(line)).ToArray();
            }
            else
            {
                output = allLines; // Just provide all lines.
            }

            return output;
        }
    }
}
