using System;
using System.Collections.Generic;
using System.IO;


namespace Public.Common.Lib.IO
{
    public class CsvFile
    {
        #region Static

        public static void WriteAllValues<T>(string csvFilePath, IEnumerable<T> values, Func<T, string> lineWriter)
        {
            using (StreamWriter writer = new StreamWriter(csvFilePath))
            {
                foreach (T value in values)
                {
                    string line = lineWriter(value);

                    writer.WriteLine(line);
                }
            }
        }

        public static void WriteAllValues<T>(string csvFilePath, IEnumerable<T> values)
        {
            CsvFile.WriteAllValues(csvFilePath, values, (x) => x.ToString());
        }

        #endregion
    }
}
