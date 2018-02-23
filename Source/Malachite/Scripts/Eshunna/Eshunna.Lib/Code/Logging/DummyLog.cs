using System;


namespace Eshunna.Lib.Logging
{
    public class DummyLog : ILog
    {
        public void WriteLine(string line)
        {
            // Do nothing.
        }
    }
}
