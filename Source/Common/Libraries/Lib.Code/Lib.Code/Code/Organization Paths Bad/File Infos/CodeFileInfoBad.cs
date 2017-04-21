

namespace Public.Common.Lib.Code.Physical
{
    public class CodeFileInfoBad : FileInfoBase
    {
        public IDirectoryInfo ParentDirectory { get; set; }
        public override IPathInfo Parent
        {
            get
            {
                return this.ParentDirectory;
            }
        }


        public CodeFileInfoBad(string name)
            : base(name)
        {
        }

        public CodeFileInfoBad(string name, IDirectoryInfo parentDirectory)
            : base(name)
        {
            this.ParentDirectory = parentDirectory;
        }
    }
}
