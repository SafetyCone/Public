using System;
using System.Collections.Generic;
using System.IO;


namespace Public.Common.Lib.Code.Physical
{
    public class DirectoryHierarchyPathsProvider : IPathsProvider
    {
        public List<string> PathTokens { get; set; }

        public IEnumerable<string> DirectoryPaths
        {
            get
            {
                string path = String.Empty;
                foreach (string pathToken in this.PathTokens)
                {
                    path = Path.Combine(path, pathToken);
                    yield return path;
                }
            }
        }

        public IEnumerable<string> FilePaths
        {
            get
            {
                return new string[] { }; // None.
            }
        }

        public DirectoryHierarchyPathsProvider()
        {
        }

        public DirectoryHierarchyPathsProvider(IEnumerable<string> pathTokens)
        {
            this.PathTokens = new List<string>(pathTokens);
        }

        public DirectoryHierarchyPathsProvider(OrganizationalPathBase organizationalPath)
        {
            this.PathTokens = new List<string>();

            organizationalPath.AddPathTokens(this.PathTokens);
        }
    }
}
