using System;
using System.Collections.Generic;
using System.IO;

using Public.Common.Lib.Organizational;
using PathsUtilities = Public.Common.Lib.IO.Paths.Utilities;


namespace Public.Common.Lib.IO
{
    /// <summary>
    /// This class provides name-value string pairs stored in a properties file for each repository, domain, or project.
    /// Properties files are saved in the user's documents directory since this directory is private from other users on the system.
    /// These properties files are saved in a single directory (the configuration directory) that can be version controlled (preferably in a private repository!).
    /// </summary>
    public class Configuration
    {
        public const string DefaultTokenSeparator = @"|";
        public const string CommentPrefix = @"//";


        #region Static

        public static readonly string DirectoryName = @"Configuration";
        public static readonly string PropertiesFileName = @"Properties.txt";


        public static string UserDocumentsDirectoryPath
        {
            get
            {
                string output = PathsUtilities.MyDocumentsDirectoryPath;
                return output;
            }
        }
        public static string DirectoryPath
        {
            get
            {
                string userDocuments = Configuration.UserDocumentsDirectoryPath;
                string output = Path.Combine(userDocuments, Configuration.DirectoryName);
                return output;
            }
        }


        public static string GetRepositoryPropertiesFilePath(string repositoryDirectoryName)
        {
            string output = Path.Combine(Configuration.DirectoryPath, repositoryDirectoryName, Configuration.PropertiesFileName);
            return output;
        }

        public static Dictionary<string, string> GetRepositoryProperties(string repositoryDirectoryName)
        {
            string repositoryPropertiesFilePath = Configuration.GetRepositoryPropertiesFilePath(repositoryDirectoryName);

            var output = Properties.ReadPropertiesFile(repositoryPropertiesFilePath);
            return output;
        }

        public static string GetCommonPropertiesFilePath()
        {
            string output = Configuration.GetRepositoryPropertiesFilePath(PublicRepository.RepositoryDirectoryName);
            return output;
        }

        public static Dictionary<string, string> GetCommonProperties()
        {
            string commonPropertiesFilePath = Configuration.GetCommonPropertiesFilePath();

            var output = Properties.ReadPropertiesFile(commonPropertiesFilePath);
            return output;
        }

        public static string GetDomainPropertiesFilePath(string repositoryDirectoryName, string domainDirectoryName)
        {
            string output = Path.Combine(Configuration.DirectoryPath, repositoryDirectoryName, domainDirectoryName, Configuration.PropertiesFileName);
            return output;
        }

        public static Dictionary<string, string> GetDomainProperties(string repositoryDirectoryName, string domainDirectoryName)
        {
            string domainPropertiesFilePath = Configuration.GetDomainPropertiesFilePath(repositoryDirectoryName, domainDirectoryName);

            var output = Properties.ReadPropertiesFile(domainPropertiesFilePath);
            return output;
        }

        public static string GetProjectPropertiesFilePath(string repositoryDirectoryName, string domainDirectoryName, string projectDirectoryName)
        {
            string output = Path.Combine(Configuration.DirectoryPath, repositoryDirectoryName, domainDirectoryName, projectDirectoryName, Configuration.PropertiesFileName);
            return output;
        }

        public static Dictionary<string, string> GetProjectProperties(string repositoryDirectoryName, string domainDirectoryName, string projectDirectoryName)
        {
            string projectPropertiesFilePath = Configuration.GetProjectPropertiesFilePath(repositoryDirectoryName, domainDirectoryName, projectDirectoryName);

            var output = Properties.ReadPropertiesFile(projectPropertiesFilePath);
            return output;
        }

        #endregion
    }
}
