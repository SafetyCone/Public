using System;


namespace Public.Common.Granby.Lib
{
    /// <summary>
    /// A very simple factory interface.
    /// </summary>
    /// <remarks>
    /// Specification type is string. Output type is generic with no variance.
    /// </remarks>
    public interface IAegeanFactory<TOutput>
    {
        TOutput this[string scheduleSpecification] { get; }
    }
}
