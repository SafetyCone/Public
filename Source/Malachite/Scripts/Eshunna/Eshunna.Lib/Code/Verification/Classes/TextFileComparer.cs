using System;
using System.IO;

using Eshunna.Lib.Logging;


namespace Eshunna.Lib.Verification
{
    public class TextFileComparer : IFileComparer
    {
        public bool Equals(string filePath1, string filePath2)
        {
            bool output = true;

            string[] lines1 = File.ReadAllLines(filePath1);
            string[] lines2 = File.ReadAllLines(filePath2);

            int nLines1 = lines1.Length;
            int nLines2 = lines2.Length;
            bool lineCountsMatch = nLines1 == nLines2;
            if(!lineCountsMatch)
            {
                output = false;

                string message = $@"File line count mismatch: {nLines1.ToString()} versus {nLines2.ToString()}.";
                this.Log.WriteLine(message);
            }
            else
            {
                for (int iLine = 0; iLine < nLines1; iLine++)
                {
                    string line1 = lines1[iLine];
                    string line2 = lines2[iLine];

                    bool linesMatch = line1 == line2;
                    if(!linesMatch)
                    {
                        output = false;

                        string message = $@"Line mismatch. Line {iLine.ToString()}" + Environment.NewLine + line1 + Environment.NewLine + line2;
                        this.Log.WriteLine(message);
                    }
                }
            }

            return output;
        }


        public ILog Log { get; }


        public TextFileComparer(ILog log)
        {
            this.Log = log;
        }

        public TextFileComparer()
            : this(new DummyLog())
        {
        }
    }
}
