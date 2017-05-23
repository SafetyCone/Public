using System;


namespace Public.Common.Granby.Lib
{
    public interface IOutputStream
    {
        void Write(string value);
        void WriteLine(string value);
    }
}
