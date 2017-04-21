

namespace Public.Common.Lib.Code.Physical
{
    public class ProjectFileInfoBad : FileInfoBase
    {
        public ProjectDirectoryInfo Directory { get; set; }
        public override IPathInfo Parent
        {
            get
            {
                return this.Directory;
            }
        }


        public ProjectFileInfoBad(string name)
            : base(name)
        {
        }

        public ProjectFileInfoBad(string name, ProjectDirectoryInfo directory)
            : base(name)
        {
            this.Directory = directory;
        }
    }
}
