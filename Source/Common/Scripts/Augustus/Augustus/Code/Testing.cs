using System;


namespace Public.Common.Augustus
{
    public static class Testing
    {
        public static void SubMain()
        {
            string[] args = Testing.GetArgs();
            Program.SubMain(args);
        }

        private static string[] GetArgs()
        {
            string[] output = new string[]
            {
                Configuration.DefaultBuildListFilePath,
                Configuration.DefaultOutputFilePath,
                Configuration.EmailResultsToken
            };

            return output;
        }
    }
}
