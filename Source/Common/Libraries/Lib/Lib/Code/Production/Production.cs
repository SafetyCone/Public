using System;
using System.IO;

using PathUtilities = Public.Common.Lib.IO.Paths.Utilities;


namespace Public.Common.Lib.Production
{
    public class Production
    {
        public const string ProductionDirectoryName = @"Production";
        public const string ConfigurationDirectoryName = @"Configuration";
        public const string DataDirectoryName = @"Data";


        #region Static

        public static string UserProductionDirectoryPath
        {
            get
            {
                string userDocumentsDirectoryPath = PathUtilities.MyDocumentsDirectoryPath;

                string output = Path.Combine(userDocumentsDirectoryPath, Production.ProductionDirectoryName);
                return output;
            }
        }

        public static string UserConfigurationDirectoryPath
        {
            get
            {
                string userProductionDirectoryPath = Production.UserProductionDirectoryPath;

                string output = Path.Combine(userProductionDirectoryPath, Production.ConfigurationDirectoryName);
                return output;
            }
        }

        public static string UserConfigurationDataDirectoryPath
        {
            get
            {
                string userConfigurationDirectoryPath = Production.UserConfigurationDirectoryPath;

                string output = Path.Combine(userConfigurationDirectoryPath, Production.DataDirectoryName);
                return output;
            }
        }

        #endregion
    }
}
