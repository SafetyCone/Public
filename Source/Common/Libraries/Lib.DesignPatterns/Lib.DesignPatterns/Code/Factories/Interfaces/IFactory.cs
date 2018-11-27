

namespace Public.Common.Lib.DesignPatterns
{
    /// <summary>
    /// The most basic factory interface, functionally equivalent to a Func[TOrder, TValue].
    /// </summary>
    /// <typeparam name="TOrder">The type of the order used by the factory to create the TValue.</typeparam>
    /// <typeparam name="TValue">The type of the value produced by the factory given an order.</typeparam>
    /// <remarks>
    /// There is no abstract base class for this interface since it is so simple, it would be implemented directly.
    /// No internal implementation details are assumed, so there is no infrastructre that a base class could provide.
    /// Additionally, only the interface can be used polymorphically so it would add annoyance to inherit from a base class and try to use the base class!
    /// </remarks>
    public interface IFactory<in TOrder, out TValue>
    {
        TValue this[TOrder order] { get; }
    }
}
