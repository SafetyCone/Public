

namespace Public.Common.Lib.Logging
{
    public enum Level
    {
        All = 0,
        Debug = 10,
        Info = 20,
        Warn = 30,
        Error = 40,
        Fatal = 50,
        Off = 100
    }


    public static class LevelExtensions
    {
        public static string ToDescription(this Level level)
        {
            string output = level.ToString();
            return output;
        }
    }
}
