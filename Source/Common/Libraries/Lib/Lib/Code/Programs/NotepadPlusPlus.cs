using Public.Common.Lib.OS;


namespace Public.Common.Lib.Programs
{
    public class NotepadPlusPlus
    {
        #region Static

        public static readonly string DefaultNotepadPlusPlusExecutablePath = @"C:\Program Files (x86)\Notepad++\notepad++.exe";


        public static string NotepadPlusPlusExecutablePath { get; set; } = NotepadPlusPlus.DefaultNotepadPlusPlusExecutablePath;


        public static void Open(string filePath)
        {
            ProcessStarter.StartProcess(NotepadPlusPlus.NotepadPlusPlusExecutablePath, filePath);
        }

        #endregion
    }
}
