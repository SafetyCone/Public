using System;
using System.IO;


namespace Public.Common.Lib.IO
{
    public class TextWriterOutputStream : IOutputStream
    {
        #region IOutputStream Members

        public void Write(string value)
        {
            this.zTextWriter.Write(value);
        }

        public void WriteLine(string value)
        {
            this.zTextWriter.WriteLine(value);
        }

        #endregion


        private TextWriter zTextWriter;


        public TextWriterOutputStream(TextWriter textWriter)
        {
            this.zTextWriter = textWriter;
        }
    }
}
