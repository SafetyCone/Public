

namespace Public.Common.Lib.Code.Physical
{
    public abstract class FileInfoBase : PathInfoBase, IFileInfo
    {
        public FileInfoBase(string name)
            : base(name)
        {
        }
    }
}
