

namespace Public.Common.Lib
{
    /// <summary>
    /// A cache that can persist key-value pairs.
    /// </summary>
    /// <remarks>
    /// Note to implementers: The implementation class is often IDisposable. However, this interface was not assumed.
    /// </remarks>
    public interface IPersistedCache<TKey, TValue> : ICache<TKey, TValue>
    {
        void Persist();
    }
}
