using System;


namespace Public.Common.Lib.Code
{
    /// <summary>
    /// An interface label for all repositories.
    /// </summary>
    public interface IRepository
    {
        string Name { get; }
        Accessibility Accessibility { get; }
    }
}
