using System;
using System.Collections.Generic;
using SysPath = System.IO.Path;


namespace Public.Common.Lib.Code.Physical
{
    public class IncrementalDirectoryPaths : OrganizationalPathBase
    {
        public OrganizationalPathBase Parent { get; set; }
        public string IncrementalDirectoryName { get; set; }
        public string IncrementalDirectoryPath
        {
            get
            {
                string output = SysPath.Combine(this.Parent.Path, this.IncrementalDirectoryName);
                return output;
            }
        }


        public IncrementalDirectoryPaths()
        {
        }

        public IncrementalDirectoryPaths(string directoryName, OrganizationalPathBase parent)
        {
            this.IncrementalDirectoryName = directoryName;
            this.Parent = parent;
        }

        public override void AddPathTokens(List<string> pathTokens)
        {
            this.Parent.AddPathTokens(pathTokens);

            pathTokens.Add(this.IncrementalDirectoryName);
        }
    }
}
