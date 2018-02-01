using Log4Level = log4net.Core.Level;

using CommonLevel = Public.Common.Lib.Logging.Level;


namespace Public.Common.Lib.Logging.Log4Net
{
    public static class LevelExtensions
    {
        public static Log4Level ToLog4Level(this CommonLevel commonLevel)
        {
            var output = LevelConverter.ToLog4Level(commonLevel);
            return output;
        }
    }
}
