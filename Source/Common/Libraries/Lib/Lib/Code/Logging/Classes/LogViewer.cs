using Public.Common.Lib.Programs;


namespace Public.Common.Lib.Logging
{
    public class LogViewer
    {
        #region Static

        public static void Show(string logFilePath)
        {
            NotepadPlusPlus.Open(logFilePath);
        }

        #endregion
    }
}
