using System.Collections.Generic;


namespace Public.Common.Lib.Code
{
    public interface INode<T>
    {
        string Name { get; set; }
        INode<T> Parent { get; }
        Dictionary<string, INode<T>> Children { get; }
        T Value { get; set; }
    }
}
