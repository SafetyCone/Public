using Log4Level = log4net.Core.Level;

using CommonLevel = Public.Common.Lib.Logging.Level;


namespace Public.Common.Lib.Logging.Log4Net
{
    public class LevelConverter
    {
        #region Static

        public static Log4Level ToLog4Level(CommonLevel commonLevel)
        {
            Log4Level output;
            switch(commonLevel)
            {
                case CommonLevel.Off:
                    output = Log4Level.Off;
                    break;

                case CommonLevel.Info:
                    output = Log4Level.Info;
                    break;

                case CommonLevel.Debug:
                    output = Log4Level.Debug;
                    break;

                case CommonLevel.Warn:
                    output = Log4Level.Warn;
                    break;

                case CommonLevel.Error:
                    output = Log4Level.Error;
                    break;

                case CommonLevel.Fatal:
                    output = Log4Level.Fatal;
                    break;

                case CommonLevel.All:
                default:
                    output = Log4Level.All;
                    break;
            }

            return output;
        }

        public static CommonLevel ToCommonLevel(Log4Level log4Level)
        {
            CommonLevel output;
            switch (log4Level)
            {
                case Log4Level l when log4Level == Log4Level.Off:
                    output = CommonLevel.Off;
                    break;

                case Log4Level l when log4Level == Log4Level.Info:
                    output = CommonLevel.Info;
                    break;

                case Log4Level l when log4Level == Log4Level.Debug:
                    output = CommonLevel.Debug;
                    break;

                case Log4Level l when log4Level == Log4Level.Warn:
                    output = CommonLevel.Warn;
                    break;

                case Log4Level l when log4Level == Log4Level.Error:
                    output = CommonLevel.Error;
                    break;

                case Log4Level l when log4Level == Log4Level.Fatal:
                    output = CommonLevel.Fatal;
                    break;

                case Log4Level l when log4Level == Log4Level.All:
                default:
                    output = CommonLevel.All;
                    break;
            }

            return output;
        }

        #endregion
    }
}
