using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace Public.Common.Granby.Lib
{
    public class StringListOutputStream : IOutputStream
    {
        #region Static

        public static StringListOutputStream Deserialize(string filePath)
        {
            StringListOutputStream output = new StringListOutputStream();
            using (StreamReader reader = new StreamReader(filePath))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    output.Lines.Add(line);
                }
            }

            return output;
        }

        public static void Serialize(string filePath, StringListOutputStream outputStream)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (string line in outputStream.Lines)
                {
                    writer.WriteLine(line);
                }
            }
        }

        #endregion

        #region IOutputStream Members

        public void Write(string value)
        {
            this.zBuilder.Append(value);
        }

        public void WriteLine(string value)
        {
            string line;
            if (0 < this.zBuilder.Length)
            {
                this.zBuilder.Append(value);

                line = this.zBuilder.ToString();
                this.zBuilder.Clear();
            }
            else
            {
                line = value;
            }

            this.Lines.Add(line);
        }

        #endregion


        private StringBuilder zBuilder;
        public List<string> Lines { get; private set; }


        public StringListOutputStream()
        {
            this.Lines = new List<string>();
            this.zBuilder = new StringBuilder();
        }
    }
}
