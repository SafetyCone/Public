using System;
using System.IO;

using Eshunna.Lib.Logging;


namespace Eshunna.Lib.Verification
{
    public class TextFileComparer : IFileComparer
    {
        public ILog Log { get; }


        public TextFileComparer(ILog log)
        {
            this.Log = log;
        }

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
                    if (!linesMatch)
                    {
                        output = false;

                        string message = $@"Line mismatch. Line {(iLine + 1).ToString()}" + Environment.NewLine + line1 + Environment.NewLine + line2; // +1 to convert from zero-based to one-based line numbers.
                        this.Log.WriteLine(message);

                        int line1Length = line1.Length;
                        int line2Length = line2.Length;

                        bool lineLengthEqual = line1Length == line2Length;
                        if (!lineLengthEqual)
                        {
                            string messageLength = $@"Line length mismatch: line1: {line1Length.ToString()}, line2: {line2Length.ToString()}";
                            this.Log.WriteLine(messageLength);
                        }
                        else
                        {
                            for (int iChar = 0; iChar < line1Length; iChar++)
                            {
                                char line1Char = line1[iChar];
                                char line2Char = line2[iChar];

                                bool charMismatch = line1Char == line2Char;
                                if(!charMismatch)
                                {
                                    string messageChar = $@"Char mismatch: index: {iChar.ToString()}, line1: {line1Char.ToString()}, line2: {line2Char.ToString()}";
                                    this.Log.WriteLine(messageChar);
                                }
                            }
                        }
                    }
                }
            }

            return output;
        }
    }
}
