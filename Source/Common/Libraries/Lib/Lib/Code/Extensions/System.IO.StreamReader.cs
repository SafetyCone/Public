using System.IO;


namespace Public.Common.Lib.Extensions
{
    public static class StreamReaderExtensions
    {
		public static string ReadLineTrim(this StreamReader reader)
        {
            var output = reader.ReadLine().Trim();
            return output;
        }

		public static string ReadNextNonBlankLine(this StreamReader reader)
        {
            string line;
			while(true)
            {
                line = reader.ReadLine();
				if(!string.IsNullOrWhiteSpace(line))
                {
                    break;
                }
            }

            return line;
        }
    }
}
