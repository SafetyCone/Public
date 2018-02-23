using System.Collections.Generic;

using Public.Common.Lib.IO;
using Public.Common.Lib.Organizational;

using Public.Malachite.Lib.Organizational;


namespace Eshunna
{
    class Program
    {
        static void Main(string[] args)
        {
            Construction.SubMain();
        }

        public static IDictionary<string, string> GetProjectProperties()
        {
            var output = Configuration.GetProjectProperties(PublicRepository.RepositoryDirectoryName, MalachiteDomain.DomainDirectoryName, Constants.ProjectDirectoryName);
            return output;
        }
    }
}
