using System;
using IWshRuntimeLibrary;


namespace WindowsShortCutCreation
{
    class Program
    {
        static void Main(string[] args)
        {
            string shortcutPath = @"C:\temp\Orgs\TheOrg\Repositories\TheRepo\Source\Common\Experiments\SecondConsole\SecondConsole.sln";
            string shortcutLinkPath = String.Format(@"{0}.lnk", shortcutPath);
            string targetpath = @"C:\temp\Orgs\TheOrg\Repositories\TheRepo\Source\Common\Experiments\SecondConsole\SecondConsole.VS2010.sln";

            WshShellClass windowsShell = new WshShellClass();
            IWshShortcut shortcut = windowsShell.CreateShortcut(shortcutLinkPath) as IWshShortcut;
            shortcut.TargetPath = targetpath;
            shortcut.Save();
        }
    }
}
