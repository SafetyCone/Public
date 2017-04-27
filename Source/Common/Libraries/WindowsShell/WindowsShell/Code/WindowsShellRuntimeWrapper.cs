using System;
using IWshRuntimeLibrary; // For embedded interop types to be true, use the WshShell interface type instead of the WshShellClass type.

using Public.Common.Lib.Extensions;


namespace Public.Common.WindowsShell
{
    public class WindowsShellRuntimeWrapper
    {
        public const string LinkSuffixExtension = @"lnk";
        public const char LinkSuffixExtensionSeparator = PathExtensions.WindowsFileExtensionSeparatorChar;


        #region Static

        /// <summary>
        /// Gets the target path of a shortcut file.
        /// </summary>
        public static string GetShortcutTargetPath(string shortcutFilePath)
        {
            string shortcutLinkFilePath = WindowsShellRuntimeWrapper.MakeFilePathIntoLinkFilePath(shortcutFilePath);

            string output = WindowsShellRuntimeWrapper.GetShortcutLinkTargetPath(shortcutLinkFilePath);
            return output;
        }

        /// <summary>
        /// Gets the target file path of a shortcut link.
        /// </summary>
        public static string GetShortcutLinkTargetPath(string shortcutLinkFilePath)
        {
            WshShell windowsShell = new WshShell();
            IWshShortcut shortcut = windowsShell.CreateShortcut(shortcutLinkFilePath) as IWshShortcut;

            string output = shortcut.TargetPath;
            return output;
        }

        /// <summary>
        /// Creates or overwrites the shortcut file that links to a target file.
        /// </summary>
        public static void CreateShortcut(string shortcutFilePath, string shortcutTargetPath)
        {
            string shortcutLinkFilePath = WindowsShellRuntimeWrapper.MakeFilePathIntoLinkFilePath(shortcutFilePath);

            WshShell windowsShell = new WshShell();
            IWshShortcut shortcut = windowsShell.CreateShortcut(shortcutLinkFilePath) as IWshShortcut;
            shortcut.TargetPath = shortcutTargetPath;
            shortcut.Save();
        }

        /// <summary>
        /// Adds the link suffix to the file path.
        /// </summary>
        /// <remarks>
        /// A link will only display its file name and file extension in Windows Explorer, not the invisible link suffix. This means that to actually get the path of a link file, you have to add the invisible link suffix.
        /// </remarks>
        public static string MakeFilePathIntoLinkFilePath(string filePath)
        {
            string output = String.Format(@"{0}{1}{2}", filePath, WindowsShellRuntimeWrapper.LinkSuffixExtensionSeparator, WindowsShellRuntimeWrapper.LinkSuffixExtension);
            return output;
        }

        #endregion
    }
}
