using System;


namespace Public.Common.Lib
{
    public interface IShallowClone<T>
    {
        T CloneShallow();
    }
}
