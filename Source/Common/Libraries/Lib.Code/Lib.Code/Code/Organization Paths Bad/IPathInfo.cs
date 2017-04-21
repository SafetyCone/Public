

namespace Public.Common.Lib.Code.Physical
{
    public interface IPathInfo
    {
        string Name { get; set; }
        IPathInfo Parent { get; }


        string GetPath();
    }
}
