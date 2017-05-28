using System;


namespace Public.Common.Lib.IO
{
    public interface IOutputStream
    {
        void Write(string value);
        void WriteLine(string value);
    }
}
