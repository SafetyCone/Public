using System;

namespace Public.Common.Granby.Lib
{
    /// <summary>
    /// A simple wrapper object around a zero-input zero-output method.
    /// </summary>
    /// <remarks>
    /// Implementations can be made arbitrarily complex, with data supplied upon construction or configuration, but not at runtime.
    /// </remarks>
    public interface ITask
    {
        void Run();
    }
}
