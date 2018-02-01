using System.IO;


namespace Public.Common.Lib.Logging.AttemptA
{
    public class DefaultLogEntrySerializer
    {
        #region Static

        public static string Serialize(Level level, object message)
        {
            string output = $@"{level.ToString()} - {message.ToString()}";
            return output;
        }

        #endregion
    }
}
