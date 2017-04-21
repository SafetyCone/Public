using System.Collections.Generic;
using System.IO;
using SysPath = System.IO.Path;


namespace Public.Common.Lib.Code.Physical
{
    public class OrganizationPaths : OrganizationalPathBase
    {
        public const string DefaultRootPath = @"C:\";
        public const string DefaultOrganizationsDirectoryName = @"Organizations";


        #region Static

        public static string DefaultOrganizationsDirectoryPath
        {
            get
            {
                string output = SysPath.Combine(OrganizationPaths.DefaultRootPath, OrganizationPaths.DefaultOrganizationsDirectoryName);
                return output;
            }
        }

        #endregion


        public string RootPath { get; protected set; }
        public string OrganizationsDirectoryName { get; set; }
        public string OrganizationsDirectoryPath
        {
            get
            {
                string output = SysPath.Combine(this.RootPath, this.OrganizationsDirectoryName);
                return output;
            }
        }
        public string OrganizationDirectoryName { get; set; }
        public string OrganizationDirectoryPath
        {
            get
            {
                string output = SysPath.Combine(this.OrganizationsDirectoryPath, this.OrganizationDirectoryName);
                return output;
            }
        }


        public OrganizationPaths(string organizationDirectoryName, string customRootPath, string customOrganizationDirectoryName)
        {
            this.RootPath = customRootPath;
            this.OrganizationsDirectoryName = customOrganizationDirectoryName;
            this.OrganizationDirectoryName = organizationDirectoryName;
        }

        public OrganizationPaths(string organizationDirectoryName)
            : this(organizationDirectoryName, OrganizationPaths.DefaultRootPath, OrganizationPaths.DefaultOrganizationsDirectoryName)
        {
        }

        public OrganizationPaths(string organizationDirectoryName, string organizationsDirectoryPath)
        {
            this.OrganizationDirectoryName = organizationDirectoryName;

            this.RootPath = SysPath.GetDirectoryName(organizationsDirectoryPath);
            this.OrganizationsDirectoryName = SysPath.GetFileName(organizationsDirectoryPath);
        }

        public OrganizationPaths()
            : this(MinexOrganization.OrganizationName)
        {
        }

        public override void AddPathTokens(List<string> pathTokens)
        {
            pathTokens.Add(this.RootPath);
            pathTokens.Add(this.OrganizationsDirectoryName);
            pathTokens.Add(this.OrganizationDirectoryName);
        }
    }
}
