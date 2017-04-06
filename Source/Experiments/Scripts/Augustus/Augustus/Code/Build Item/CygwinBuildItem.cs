

namespace Augustus
{
    public class CygwinBuildItem : BuildItemBase
    {
        public CygwinBuildItem()
        {
        }

        public CygwinBuildItem(string buildFilePath, string platform) : base(buildFilePath, platform)
        {
        }

        public CygwinBuildItem(string buildFilePath, Platform platform) : base(buildFilePath, platform)
        {
        }
    }
}
