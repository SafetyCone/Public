using System;


namespace Public.Common.Lib.Organizational
{
    /// <summary>
    /// An interface label for all repositories.
    /// </summary>
    public interface IRepository
    {
        string Name { get; }
        bool Public { get; }
    }
}
