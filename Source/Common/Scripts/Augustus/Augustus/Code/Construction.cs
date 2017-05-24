using System;
using System.Collections.Generic;

using Public.Common.Augustus.Lib;


namespace Public.Common.Augustus
{
    public static class Construction
    {
        public static void SubMain()
        {
            List<string> buildItemSpecifications = Construction.GetBuildItemSpecifications();
            List<BuildItem> buildItems = BuildItem.GetBuildItems(buildItemSpecifications);
            Dictionary<string, bool> successByBuildItemPath = Builder.Run(buildItems, Console.Out);

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
