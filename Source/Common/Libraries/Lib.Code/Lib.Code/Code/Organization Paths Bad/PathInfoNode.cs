

namespace Public.Common.Lib.Code.Physical
{
    public class PathInfoNode : Node<IPathInfo>
    {
        #region Static

        protected static void AddPathTokens(string[] pathTokens, PathInfoNode parentNode, int currentIndex)
        {
            string currentNodeName = pathTokens[currentIndex];
            PathInfoNode currentNode = new PathInfoNode(currentNodeName, parentNode);

            parentNode.Children.Add(currentNode.Name, currentNode);

            if (pathTokens.Length - 1 != currentIndex)
            {
                PathInfoNode.AddPathTokens(pathTokens, currentNode, currentIndex + 1);
            }
        }

        #endregion


        protected PathInfoNode()
            : base()
        {
        }

        public PathInfoNode(string name, INode<IPathInfo> parent)
            : base(name, parent)
        {
        }
    }
}
