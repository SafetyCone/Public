using Log4Level = log4net.Core.Level;

using CommonLevel = Public.Common.Lib.Logging.Level;


namespace Public.Common.Lib.Logging.Log4Net
{
    public static class NetLevelExtensions
    {
        public static CommonLevel ToCommonLevel(this Log4Level log4Level)
        {
            var output = LevelConverter.ToCommonLevel(log4Level);
            return output;
        }
    }
}
