

namespace Augustus
{
    public interface IBuildItem
    {
        string BuildFilePath { get; set; }
        Platform Platform { get; set; }
    }
}
