using System;
using System.Collections.Generic;
using System.IO;

using Prod = Public.Common.Lib.Production.Production;


namespace Public.Common.Lib.Security
{
    public class AuthenticationsTextFile
    {
        public const string DefaultAuthenticationsTextFileName = @"Authentications.txt";
        public const char AuthenticationsTextFileTokenSeparator = '|';


        #region Static

        public static string DefaultAuthenticationsTextFilePath
        {
            get
            {
                string userConfigDataDirectoryPath = Prod.UserConfigurationDataDirectoryPath;

                string output = Path.Combine(userConfigDataDirectoryPath, AuthenticationsTextFile.DefaultAuthenticationsTextFileName);
                return output;
            }
        }

        public static Dictionary<string, Authentication> DeserializeTextAuthenticationsByName(string authenticationsTextFilePath, char tokenSeparator)
        {
            string[] lines = File.ReadAllLines(authenticationsTextFilePath);

            Dictionary<string, Authentication> output = new Dictionary<string, Authentication>();
            foreach (string line in lines)
            {
                string[] tokens = line.Split(tokenSeparator);

                string nameToken = tokens[0];
                string userNameToken = tokens[1];
                string passwordToken = tokens[2];

                Authentication authentication = new Authentication(nameToken, userNameToken, passwordToken);
                output.Add(authentication.Name, authentication);
            }

            return output;
        }

        public static Dictionary<string, Authentication> DeserializeTextAuthenticationsByName(string authenticationsTextFilePath)
        {
            Dictionary<string, Authentication> output = AuthenticationsTextFile.DeserializeTextAuthenticationsByName(authenticationsTextFilePath, AuthenticationsTextFile.AuthenticationsTextFileTokenSeparator);
            return output;
        }

        #endregion
    }
}
