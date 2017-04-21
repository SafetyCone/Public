using System;
using System.Collections.Generic;


namespace Public.Common.Lib.IO
{
    /// <summary>
    /// Represents a text file as a list of lines.
    /// </summary>
    public class TextFile
    {
        public List<string> Lines { get; protected set; }


        public TextFile()
        {
            this.Lines = new List<string>();
        }
    }
}
