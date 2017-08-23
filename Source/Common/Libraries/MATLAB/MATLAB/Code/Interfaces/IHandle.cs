using System;


namespace Public.Common.MATLAB
{
    /// <summary>
    /// Represents a MATLAB object handle.
    /// </summary>
    /// <remarks>
    /// Handles are referred to by a unique variable name in the default MATLAB workspace. MATLAB handles are created via various MATLAB functions, and deleted via the MATLAB delete method.
    /// Handles are disposable. This allows them to be used in a using scope, and deleted in MATLAB via the dispose method.
    /// </remarks>
    public interface IHandle : IDisposable
    {
        MatlabApplication Application { get; }
        string HandleName { get; }
    }
}
