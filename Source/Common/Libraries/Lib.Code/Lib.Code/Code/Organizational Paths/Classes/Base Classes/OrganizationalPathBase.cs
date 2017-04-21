using System.Collections.Generic;
using SysPath = System.IO.Path;


namespace Public.Common.Lib.Code.Physical
{
    public abstract class OrganizationalPathBase : IOrganizationalPath
    {
        public string Path
        {
            get
            {
                List<string> pathTokens = new List<string>();
                this.AddPathTokens(pathTokens);

                string output = SysPath.Combine(pathTokens.ToArray());
                return output;
            }
        }

        public abstract void AddPathTokens(List<string> pathTokens);
    }
}
