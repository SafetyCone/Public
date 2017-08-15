using System;

using Public.Common.Lib.IO;
using Public.Common.Lib.IO.Extensions;
using Public.Common.Lib.Logging;


namespace Public.Common.Avon
{
    class Program
    {
        private static void Main(string[] args)
        {
            //Program.SubMain(args);
            Program.DebugSubMain();
            //Construction.SubMain(args):
        }

        private static void DebugSubMain()
        {
            //string[] args =
            //{
            //    @"CreateSolutionSetFromInitialVsVersionSolution",
            //    @"C:\Organizations\Minex\Repositories\Public\Source\Common\Libraries\Lib.Visuals\Lib.Visuals.VS2010.sln",
            //    @"VS_ALL"
            //};

            //string[] args =
            //{
            //    @"DistributeChangesFromDefaultVsVersionSolution",
            //    @"C:\Organizations\Minex\Repositories\Public\Source\Common\Scripts\Avon"
            //};

            string[] args =
            {
                @"SetDefaultVsVersionSolution",
                @"C:\Organizations\Minex\Repositories\Public\Source\Common\Scripts\Avon",
                @"VS_LATEST"
            };

            Program.SubMain(args);
        }

        private static void SubMain(string[] args)
        {
            ConsoleOutputStream console = new ConsoleOutputStream();
            DebugOutputStream debug = new DebugOutputStream();
            MultipleOutputStream debugAndConsole = new MultipleOutputStream(new IOutputStream[] { console, debug });
            debugAndConsole.WriteLineAndBlankLine(Constants.ProgramName);

            string logFilePath = Log.GetAndEnsureDefaultLogFilePath(Constants.ProgramName);
            debugAndConsole.WriteLine(@"Log file path:");
            debugAndConsole.WriteLineAndBlankLine(logFilePath);

            Log log = new Log(logFilePath);
            MultipleOutputStream logAndConsole = new MultipleOutputStream(new IOutputStream[] { log.OutputStream, console });

            Configuration config;
            if (Configuration.TryParseArguments(args, logAndConsole, out config))
            {
                try
                {
                    config.Action();
                }
                catch (Exception ex)
                {
                    logAndConsole.WriteLine(@"ERROR executing action.");
                    logAndConsole.WriteLine(ex.Message);
                }
            }
        }
    }
}
