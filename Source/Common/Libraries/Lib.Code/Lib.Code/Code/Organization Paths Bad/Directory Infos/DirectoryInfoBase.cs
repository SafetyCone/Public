

namespace Public.Common.Lib.Code.Physical
{
    public abstract class DirectoryInfoBase : PathInfoBase, IDirectoryInfo
    {
        public DirectoryInfoBase(string name)
            : base(name)
        {
        }
    }
}
