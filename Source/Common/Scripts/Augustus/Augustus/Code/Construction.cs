using System;
using System.Collections.Generic;

using Public.Common.Lib.IO;

using Public.Common.Augustus.Lib;


namespace Public.Common.Augustus
{
    public static class Construction
    {
        public static void SubMain(string[] args)
        {
            List<string> buildItemSpecifications = Construction.GetBuildItemSpecifications();
            List<BuildItem> buildItems = BuildItem.GetBuildItems(buildItemSpecifications);

            IOutputStream console = new ConsoleOutputStream();

            Dictionary<string, bool> successByBuildItemPath = new Dictionary<string,bool>();
            foreach (BuildItem buildItem in buildItems)
            {
                bool success = Builder.Run(buildItem, console, console);
                successByBuildItemPath.Add(buildItem.FilePath, success);
            }

            Program.WriteResults(successByBuildItemPath);
            Program.OpenResults();
        }

        private static List<string> GetBuildItemSpecifications()
        {
#if (DEBUG)
            //string buildListFileRelativePath = Constants.DebugBuildFileListFileRelativePath;
            string buildListFileRelativePath = Constants.BuildFileListFileRelativePath;
#else
            string buildListFileRelativePath = Constants.BuildFileListFileRelativePath;
#endif

            List<string> output = BuildItemTextFile.GetBuildItemSpecifications(buildListFileRelativePath);
            return output;
        }
    }
}
