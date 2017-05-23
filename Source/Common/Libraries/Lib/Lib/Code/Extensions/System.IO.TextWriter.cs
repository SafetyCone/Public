using System;
using System.IO;


namespace Public.Common.Lib.Extensions
{
    public static class TextWriterExtensions
    {
		public static void WriteLineAndBlankLine(this TextWriter writer, string value)
        {
            writer.WriteLine(value);
            writer.WriteLine();
        }
    }
}
