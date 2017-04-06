using Augustus.Extensions;


namespace Augustus
{
    public class WindowsBuildItem : BuildItemBase
    {
        public Architecture Architecture { get; set; }


        public WindowsBuildItem()
        {
        }

        public WindowsBuildItem(string buildFilePath, string platform, string architecture)
            : this(buildFilePath, PlatformExtensions.FromDefault(platform), ArchitectureExtensions.FromDefault(architecture))
        {
        }

        public WindowsBuildItem(string buildFilePath, Platform platform, Architecture architecture) : base(buildFilePath, platform)
        {
        }
    }
}
