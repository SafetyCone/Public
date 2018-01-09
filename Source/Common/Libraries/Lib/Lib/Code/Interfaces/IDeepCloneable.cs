using System;


namespace Public.Common.Lib
{
    public interface IDeepCloneable<T> : ICloneable
    {
        T CloneDeep();
    }
}
