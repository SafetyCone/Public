using System;


namespace Public.Common.Granby.Lib
{
    public interface ILog
    {
        void Write(string value);
        void WriteLine(string value);
    }
}
