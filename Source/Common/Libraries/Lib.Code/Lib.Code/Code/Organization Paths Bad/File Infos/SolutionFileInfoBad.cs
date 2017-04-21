

namespace Public.Common.Lib.Code.Physical
{
    public class SolutionFileInfoBad : FileInfoBase
    {
        public SolutionDirectoryInfo Directory { get; set; }
        public override IPathInfo Parent
        {
            get
            {
                return this.Directory;
            }
        }


        public SolutionFileInfoBad(string name)
            : base(name)
        {
        }

        public SolutionFileInfoBad(string name, SolutionDirectoryInfo directory)
            :base(name)
        {
            this.Directory = directory;
        }
    }
}
