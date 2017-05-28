using System;
using System.IO;

using Public.Common.Lib.Extensions;
using PathUtilities = Public.Common.Lib.IO.Paths.Utilities;


namespace Public.Common.Lib.Production
{
    public class Production
    {
        public const string ProductionDirectoryName = @"Production";
        public const string ConfigurationDirectoryName = @"Configuration";
        public const string DataDirectoryName = @"Data";
        public const string LogsDirectoryName = @"Logs";
        public const string OutputDirectoryName = @"Output";


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

        public static string UserLogsDirectoryPath
        {
            get
            {
                string userProductionDirectoryPath = Production.UserProductionDirectoryPath;

                string output = Path.Combine(userProductionDirectoryPath, Production.LogsDirectoryName);
                return output;
            }
        }

        public static string UserOutputDirectoryPath
        {
            get
            {
                string userProductionDirectoryPath = Production.UserProductionDirectoryPath;

                string output = Path.Combine(userProductionDirectoryPath, Production.OutputDirectoryName);
                return output;
            }
        }


        public static string GetProgramUserOutputDirectoryPath(string programName)
        {
            string userOutputDirectoryPath = Production.UserOutputDirectoryPath;

            string output = Path.Combine(userOutputDirectoryPath, programName);
            return output;
        }

        public static string GetRunDatedAndTimedPath(string baseDirectoryPath, DateTime dateTime)
        {
            string dateDirectoryName = dateTime.ToYYYYMMDDStr();
            string timeDirectoryName = dateTime.ToHHMMSSStr();

            string output = Path.Combine(baseDirectoryPath, dateDirectoryName, timeDirectoryName);
            return output;
        }

        public static string GetRunDatedAndTimedPath(string baseDirectoryPath)
        {
            string output = Production.GetRunDatedAndTimedPath(baseDirectoryPath, DateTime.Now);
            return output;
        }

        public static string GetProgramRunOutputDirectoryPath(string programName, DateTime runTime)
        {
            string programUserOutputDirectoryPath = Production.GetProgramUserOutputDirectoryPath(programName);

            string output = Production.GetRunDatedAndTimedPath(programUserOutputDirectoryPath, runTime);
            return output;
        }

        public static string GetProgramRunOutputDirectoryPath(string programName)
        {
            string output = Production.GetProgramRunOutputDirectoryPath(programName, DateTime.Now);
            return output;
        }

        #endregion
    }
}
