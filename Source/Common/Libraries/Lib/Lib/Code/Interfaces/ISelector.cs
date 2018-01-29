

namespace Public.Common.Lib
{
    /// <summary>
    /// Selects integer indices.
    /// </summary>
    public interface ISelector
    {
        bool this[int index] { get; }
    }
}
