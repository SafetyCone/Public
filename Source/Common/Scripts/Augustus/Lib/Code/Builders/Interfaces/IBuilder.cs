using System;


namespace Public.Common.Augustus.Lib
{
    public interface IBuilder
    {
        string SuccessRegexPattern { get; }


        BuildInfo GetBuildInfo(BuildItem buildItem);
    }
}
