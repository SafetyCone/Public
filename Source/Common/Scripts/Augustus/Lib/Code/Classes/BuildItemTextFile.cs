using System;
using System.Collections.Generic;
using System.IO;


namespace Public.Common.Augustus.Lib
{
    public class BuildItemTextFile
    {
        #region Static

        public static List<string> GetBuildItemSpecifications(string buildListFilePath)
        {
            string[] lines = File.ReadAllLines(buildListFilePath);

            var output = new List<string>();
            foreach (string line in lines)
            {
                if (!String.IsNullOrEmpty(line))
                {
                    output.Add(line);
                }
            }

            return output;
        }

        public static List<BuildItem> GetBuildItems(string buildListFilePath)
        {
            List<string> buildItemSpecifications = BuildItemTextFile.GetBuildItemSpecifications(buildListFilePath);

            List<BuildItem> output = BuildItem.GetBuildItems(buildItemSpecifications);
            return output;
        }

        #endregion
    }
}
