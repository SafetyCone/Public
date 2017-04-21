using System;
using System.Collections.Generic;
using System.IO;

using Public.Common.Lib.Extensions;


namespace Public.Common.Lib.Code.Physical
{
    public class OrganizationalPath
    {
        public const int NotFoundIndex = -1;


        #region Static

        public static bool TryIdentifyPathTokenIndex(string[] pathTokens, string pathToken, out int index)
        {
            bool output = false;
            index = OrganizationalPath.NotFoundIndex;
            for (int iToken = 0; iToken < pathTokens.Length; iToken++)
            {
                if (pathToken == pathTokens[iToken])
                {
                    index = iToken;
                    output = true;
                    break;
                }
            }

            return output;
        }

        public static int IdentifyPathTokenIndex(string[] pathTokens, string pathToken)
        {
            int output;
            if (!OrganizationalPath.TryIdentifyPathTokenIndex(pathTokens, pathToken, out output))
            {
                string pathTokensPrettyPrint = pathTokens.PrettyPrint();
                string message = String.Format(@"Unable to identify path token {0} in: {1}", pathToken, pathTokensPrettyPrint);
#if (NETFX_452)
                throw new ArgumentException(message, nameof(pathToken));
#else
                throw new ArgumentException(message, "pathToken");
#endif
            }

            return output;
        }

        public static int DetermineOrganizationsPathDepth(string[] pathTokens)
        {
            int organizationsDirectoryIndex = OrganizationsDirectoryInfo.IdentifyOrganizationsDirectoryIndex(pathTokens);
            int totalDepth = pathTokens.Length - 1;

            int output = totalDepth - organizationsDirectoryIndex;
            return output;
        }

        //private static Dictionary<int, Func<string[], IPathInfo>> GetPathInfoConstructorsByOrganizationPathDepth()
        //{
        //    var output = new Dictionary<int, Func<string[], IPathInfo>>();

        //    output.Add(0, )

        //}

        //public static OrganizationsDirectoryInfo ConstructOrganizations(string[] pathTokens, int organizationsTokenIndex)
        //{
        //    // Build the root.
        //    int rootPathTokensLength = organizationsTokenIndex;
        //    string[] rootPathTokens = new string[rootPathTokensLength];

        //    Array.Copy(pathTokens, rootPathTokens, rootPathTokensLength);

        //    string rootDirectoryPath = Path.Combine(rootPathTokens);

        //    RootDirectoryInfo root = new RootDirectoryInfo(rootDirectoryPath);

        //    OrganizationsDirectoryInfo output = new OrganizationsDirectoryInfo()
        //}

        //public static IPathInfo GetPathInfo(string[] pathTokens)
        //{
        //    int organizationsPathDepth = OrganizationalPath.DetermineOrganizationsPathDepth(pathTokens);


        //}

        #endregion


        public BasePathInfo Base { get; set; }
        public RootDirectoryInfo Root { get; set; }
        public OrganizationsDirectoryInfo Organizations { get; set; }
        public OrganizationDirectoryInfo Organization { get; set; }
        public RepositoriesDirectoryInfo Repositories { get; set; }
        public RepositoryDirectoryInfo Repository { get; set; }
        public SourceDirectoryInfo Source { get; set; }
        public DomainDirectoryInfo Domain { get; set; }
        public SolutionTypeDirectoryInfo SolutionType { get; set; }
        public SolutionDirectoryInfo Solution { get; set; }
        public ProjectDirectoryInfo Project { get; set; }
        public List<CodeDirectoryInfo> CodeDirectories { get; set; }
        public IFileInfo File { get; set; }
    }
}
