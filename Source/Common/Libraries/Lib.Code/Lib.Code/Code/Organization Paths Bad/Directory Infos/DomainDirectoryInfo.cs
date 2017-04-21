

namespace Public.Common.Lib.Code.Physical
{
    public class DomainDirectoryInfo : DirectoryInfoBase
    {
        #region Static

        public static int IdentifyDomainDirectoryIndex(string[] pathTokens)
        {
            int sourceDirectoryIndex = SourceDirectoryInfo.IdentifySourceDirectoryIndex(pathTokens);

            int output = OrganizationalPath.NotFoundIndex;
            if (OrganizationalPath.NotFoundIndex != sourceDirectoryIndex)
            {
                if (pathTokens.Length > sourceDirectoryIndex + 1)
                {
                    output = sourceDirectoryIndex + 1;
                }
            }

            return output;
        }

        #endregion


        public SourceDirectoryInfo Source { get ;set;}
        public override IPathInfo Parent
        {
            get
            {
                return this.Source;
            }
        }


        public DomainDirectoryInfo(string name)
            : base(name)
        {
        }

        public DomainDirectoryInfo(string name, SourceDirectoryInfo source)
            : base(name)
        {
            this.Source = source;
        }
    }
}
