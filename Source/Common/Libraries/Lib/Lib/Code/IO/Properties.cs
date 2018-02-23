using System;
using System.Collections.Generic;

using IoUtilities = Public.Common.Lib.IO.Utilities;


namespace Public.Common.Lib.IO
{
    public class Properties
    {
        #region Static

        public static readonly string ProjectsDataDirectoryPathPropertyName = @"Projects Data Directory Path";


        public static Dictionary<string, string> ReadPropertiesFile(string propertiesFileName, string tokenSeparator = Configuration.DefaultTokenSeparator)
        {
            var lines = IoUtilities.ReadAllLines(propertiesFileName);

            var separators = new string[] { tokenSeparator };
            int commentTokenPrefixLength = Configuration.CommentPrefix.Length;

            var output = new Dictionary<string, string>();
            foreach (var line in lines)
            {
                if (Configuration.CommentPrefix == line.Substring(0, commentTokenPrefixLength))
                {
                    continue;
                }

                var tokens = line.Split(separators, StringSplitOptions.None);

                int tokenCount = tokens.Length;
                if (0 < tokenCount)
                {
                    string key = tokens[0];
                    string value = null;
                    if (1 < tokenCount)
                    {
                        value = tokens[1];
                    }

                    output.Add(key, value);
                }
            }

            return output;
        }

        #endregion
    }
}
