

namespace Public.Common.Lib.Code.Physical
{
    public class OrganizationDirectoryInfo : DirectoryInfoBase
    {
        #region Static

        public static int IdentifyOrganizationDirectoryIndex(string[] pathTokens)
        {
            int organizationsDirectoryIndex = OrganizationsDirectoryInfo.IdentifyOrganizationsDirectoryIndex(pathTokens);

            int output = OrganizationalPath.NotFoundIndex;
            if (OrganizationalPath.NotFoundIndex != organizationsDirectoryIndex)
            {
                if (pathTokens.Length > organizationsDirectoryIndex + 1)
                {
                    output = organizationsDirectoryIndex + 1;
                }
            }

            return output;
        }

        #endregion


        public OrganizationsDirectoryInfo Organizations { get; set; }
        public override IPathInfo Parent
        {
            get
            {
                return this.Organizations;
            }
        }


        public OrganizationDirectoryInfo(string name)
            : base(name)
        {
        }

        public OrganizationDirectoryInfo(string name, OrganizationsDirectoryInfo organizations)
            : base(name)
        {
            this.Organizations = organizations;
        }
    }
}
