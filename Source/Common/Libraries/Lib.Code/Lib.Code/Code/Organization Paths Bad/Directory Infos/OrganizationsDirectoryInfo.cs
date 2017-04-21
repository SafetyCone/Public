using System;


namespace Public.Common.Lib.Code.Physical
{
    public class OrganizationsDirectoryInfo : DirectoryInfoBase
    {
        public const string DefaultOrganizationsDirectoryName = @"Organizations";
        public const string DefaultPath = @"C:\Organizations";


        #region Static

        public static string IdentifyOrganizationsDirectoryPath(string path)
        {
            string output = OrganizationsDirectoryInfo.IdentifyOrganizationsDirectoryPath(path, OrganizationsDirectoryInfo.DefaultOrganizationsDirectoryName);
            return output;
        }

        public static string IdentifyOrganizationsDirectoryPath(string path, string customOrganizationsDirectoryName)
        {
            string output;
            if(!OrganizationsDirectoryInfo.TryIdentifyOrganizationsDirectoryPath(path, customOrganizationsDirectoryName, out output))
            {
                string message = String.Format(@"Unable to identify Organizations directory path in: {0}", path);
#if (VS2010 || VS2013)
                throw new ArgumentException(message, "path");
#else
                throw new ArgumentException(message, nameof(path));
#endif
            }

            return output;
        }

        public static bool TryIdentifyOrganizationsDirectoryPath(string path, out string organizationsDirectoryPath)
        {
            bool output = OrganizationsDirectoryInfo.TryIdentifyOrganizationsDirectoryPath(path, OrganizationsDirectoryInfo.DefaultOrganizationsDirectoryName, out organizationsDirectoryPath);
            return output;
        }

        public static bool TryIdentifyOrganizationsDirectoryPath(string path, string customOrganizationsDirectoryName, out string organizationsDirectoryPath)
        {
            bool output = true;

            int index = path.IndexOf(customOrganizationsDirectoryName);
            if (-1 == index)
            {
                output = false;
                organizationsDirectoryPath = string.Empty;
            }
            else
            {
                organizationsDirectoryPath = path.Substring(0, index + customOrganizationsDirectoryName.Length);
            }

            return output;
        }


        public static int IdentifyOrganizationsDirectoryIndex(string[] pathTokens)
        {
            int output = OrganizationalPath.IdentifyPathTokenIndex(pathTokens, OrganizationsDirectoryInfo.DefaultOrganizationsDirectoryName);
            return output;
        }

        #endregion


        public string Path { get; set; }
        public RootDirectoryInfo Root { get; set; }
        public override IPathInfo Parent
        {
            get
            {
                return this.Root;
            }
        }


        public OrganizationsDirectoryInfo()
            : base(OrganizationsDirectoryInfo.DefaultOrganizationsDirectoryName)
        {
        }

        public OrganizationsDirectoryInfo(RootDirectoryInfo root)
            : base(OrganizationsDirectoryInfo.DefaultOrganizationsDirectoryName)
        {
            this.Root = root;
        }

        public OrganizationsDirectoryInfo(string name)
            : base(name)
        {
        }

        public OrganizationsDirectoryInfo(RootDirectoryInfo root, string name)
            : base(name)
        {
            this.Root = root;
        }
    }
}
