using System;
using System.Collections.Generic;


namespace Public.Common.Lib.IO
{
    public class TextFile
    {
        public List<string> Lines { get; protected set; }


        public TextFile()
        {
            this.Lines = new List<string>();
        }
    }
}
