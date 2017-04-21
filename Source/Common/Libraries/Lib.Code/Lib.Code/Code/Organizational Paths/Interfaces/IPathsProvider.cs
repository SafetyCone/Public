using System.Collections.Generic;


namespace Public.Common.Lib.Code.Physical
{
    public interface IPathsProvider
    {
        IEnumerable<string> DirectoryPaths { get; }
        IEnumerable<string> FilePaths { get; }
    }
}
