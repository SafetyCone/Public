using System.Collections.Generic;


namespace Public.Common.Lib.Code.Physical
{
    public class PathInfoRootNode : PathInfoNode
    {
        public PathInfoRootNode()
            : base()
        {
        }

        public void AddPathTokens(string[] pathTokens)
        {
            PathInfoNode.AddPathTokens(pathTokens, this, 0);
        }
    }
}
