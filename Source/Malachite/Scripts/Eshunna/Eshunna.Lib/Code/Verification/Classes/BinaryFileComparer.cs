using System;
using System.IO;

using Eshunna.Lib.Logging;


namespace Eshunna.Lib.Verification
{
    public class BinaryFileComparer : IFileComparer
    {
        public ILog Log { get; }


        public BinaryFileComparer(ILog log)
        {
            this.Log = log;
        }

        public bool Equals(string filePath1, string filePath2)
        {
            bool output = true;

            byte[] file1 = File.ReadAllBytes(filePath1);
            byte[] file2 = File.ReadAllBytes(filePath2);

            int nBytes1 = file1.Length;
            int nBytes2 = file2.Length;

            bool lengthEquals = nBytes1 == nBytes2;
            if(!lengthEquals)
            {
                output = false;

                string message = $@"File byte count mismatch: file1: {nBytes1.ToString()}, file2: {nBytes2.ToString()}";
                this.Log.WriteLine(message);
            }
            else
            {
                for (int iByte = 0; iByte < nBytes1; iByte++)
                {
                    byte value1 = file1[iByte];
                    byte value2 = file2[iByte];

                    bool valuesEqual = value1 == value2;
                    if(!valuesEqual)
                    {
                        output = false;

                        string message = $@"File byte value mismatch: index: {iByte}, file1: {value1.ToString()}, file2: {value2.ToString()}";
                        this.Log.WriteLine(message);
                    }
                }
            }

            return output;
        }
    }
}
