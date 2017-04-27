using System;


namespace Public.Common.Lib
{
    public interface ICloneable<T> : ICloneable
    {
        new T Clone();
    }
}
