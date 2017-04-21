using System.Collections.Generic;

using Public.Common.Lib.Extensions;


namespace Public.Common.Lib.Code.Physical
{
    public class BasePathInfo
    {
        public Dictionary<string, RootDirectoryInfo> RootsByRootPath { get; protected set; }


        public BasePathInfo()
        {
            this.RootsByRootPath = new Dictionary<string, RootDirectoryInfo>();
        }

        public void AddPath(string path)
        {
            string[] pathTokens = path.SplitPath();

            this.AddPath(pathTokens);
        }

        public void AddPath(string[] pathTokens)
        {
            string rootPath = RootDirectoryInfo.DetermineRootPath(pathTokens);

            RootDirectoryInfo root = new RootDirectoryInfo(rootPath);
            this.RootsByRootPath.Add(root.Path, root);


        }
    }
}
