using System;


namespace Public.Common.Granby.Lib
{
    public class ConsoleOutputStream : IOutputStream
    {
        #region IOutputStream Members

        public void Write(string value)
        {
            Console.Write(value);
        }

        public void WriteLine(string value)
        {
            Console.WriteLine(value);
        }

        #endregion
    }
}