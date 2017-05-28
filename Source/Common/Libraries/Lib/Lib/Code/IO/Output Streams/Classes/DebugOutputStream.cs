using System;
using System.Diagnostics;


namespace Public.Common.Lib.IO
{
    public class DebugOutputStream : IOutputStream
    {
        #region IOutputStream Members

        public void Write(string value)
        {
            Debug.Write(value);
        }

        public void WriteLine(string value)
        {
            Debug.WriteLine(value);
        }

        #endregion
    }
}
