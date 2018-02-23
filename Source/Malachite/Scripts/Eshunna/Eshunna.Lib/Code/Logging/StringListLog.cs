using System;
using System.Collections.Generic;


namespace Eshunna.Lib.Logging
{
    public class StringListLog : ILog
    {
        public List<string> Lines { get; }


        public StringListLog(List<string> lines)
        {
            this.Lines = lines;
        }

        public StringListLog()
            : this(new List<string>())
        {
        }

        public void WriteLine(string line)
        {
            this.Lines.Add(line);
        }
    }
}
