using System.IO;


namespace Public.Common.Lib.Code.Physical
{
    public abstract class PathInfoBase : IPathInfo
    {
        public string Name { get; set; }
        public abstract IPathInfo Parent { get; }


        public PathInfoBase()
        {
        }

        public PathInfoBase(string name)
        {
            this.Name = name;
        }

        public virtual string GetPath()
        {
            string output;
            if(null == this.Parent)
            {
                output = this.Name;
            }
            else
            {
                string parentPath = this.Parent.GetPath();
                output = Path.Combine(parentPath, this.Name);
            }

            return output;
        }
    }
}
