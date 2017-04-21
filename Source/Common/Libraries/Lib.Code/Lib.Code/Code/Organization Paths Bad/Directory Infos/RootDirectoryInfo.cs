using System;


namespace Public.Common.Lib.Code.Physical
{
    public class RootDirectoryInfo : DirectoryInfoBase
    {
        public const string DefaultRootDirectoryName = @"Root";


        #region Static

        public static string DetermineRootPath(string[] pathTokens)
        {
            int organizationsDirectoryIndex = OrganizationsDirectoryInfo.IdentifyOrganizationsDirectoryIndex(pathTokens);

            int rootPathTokensLength = organizationsDirectoryIndex;
            string[] rootPathTokens = new string[rootPathTokensLength];

            Array.Copy(pathTokens, rootPathTokens, rootPathTokensLength);

            string rootDirectoryPath = System.IO.Path.Combine(rootPathTokens);
            return rootDirectoryPath;
        }

        #endregion


        public override IPathInfo Parent
        {
            get
            {
                return null;
            }
        }
        public string Path { get; set; }


        public RootDirectoryInfo(string path)
            : base(RootDirectoryInfo.DefaultRootDirectoryName)
        {
            this.Path = path;
        }

        public RootDirectoryInfo(string path, string name)
            : base(name)
        {
            this.Path = path;
        }

        public override string GetPath()
        {
            return this.Path;
        }
    }
}
