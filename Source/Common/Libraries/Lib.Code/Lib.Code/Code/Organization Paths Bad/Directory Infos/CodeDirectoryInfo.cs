

namespace Public.Common.Lib.Code.Physical
{
    public class CodeDirectoryInfo : DirectoryInfoBase
    {
        public IDirectoryInfo ParentDirectory { get; set; }
        public override IPathInfo Parent
        {
            get
            {
                return this.ParentDirectory;
            }
        }


        public CodeDirectoryInfo(string name)
            : base(name)
        {
        }

        public CodeDirectoryInfo(string name, IDirectoryInfo parentDirectory)
            : base(name)
        {
            this.ParentDirectory = parentDirectory;
        }
    }
}
