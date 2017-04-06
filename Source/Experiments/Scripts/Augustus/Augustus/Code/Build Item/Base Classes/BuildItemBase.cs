using Augustus.Extensions;


namespace Augustus
{
    public abstract class BuildItemBase : IBuildItem
    {
        public string BuildFilePath { get; set; }
        public Platform Platform { get; set; }


        public BuildItemBase()
        {
        }

        public BuildItemBase(string buildFilePath, string platform)
            : this(buildFilePath, PlatformExtensions.FromDefault(platform))
        {
        }

        public BuildItemBase(string buildFilePath, Platform platform)
        {
            this.BuildFilePath = buildFilePath;
            this.Platform = platform;
        }
    }
}
