using System;


namespace Public.Common.Lib.Logging
{
    public interface ILog
    {
        void Write(string value);
        void WriteLine(string value);
    }
}
