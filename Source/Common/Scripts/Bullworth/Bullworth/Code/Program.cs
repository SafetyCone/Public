using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

//using PathExtensions = Public.Common.Lib.IO.Extensions.PathExtensions;


namespace Bullworth
{
    class Program
    {
        private const int NumberOfRequiredInputArguments = 4;


        static int Main(string[] args)
        {
            // When run from the post-build event of a client project, Console.Out goes to the Visual Studio output window.
            Console.WriteLine(@"Starting the Bullworth file-copier...");
            Console.WriteLine($@"Found {args.Length} input arguments:");
            foreach(string arg in args)
            {
                Console.WriteLine(arg);
            }

            // Parse input arguments.
            if (Program.NumberOfRequiredInputArguments != args.Length)
            {
                Console.WriteLine($@"Incorrect number of input arguments. Expected {Program.NumberOfRequiredInputArguments.ToString()}, found {args.Length.ToString()}.");
                return Program.Return(-1);
            }

            string clientTargetDirectoryPath = args[0];

            string configurationArg = args[1];
            string configurationArgLower = configurationArg.ToLowerInvariant();
            ConfigurationName configuration;
            switch (configurationArgLower)
            {
                case @"debug":
                    configuration = ConfigurationName.Debug;
                    break;

                case @"release":
                    configuration = ConfigurationName.Release;
                    break;

                default:
                    Console.WriteLine($@"Unknown configuration: {configurationArg}. Options are: Debug, Release");
                    return Program.Return(-1);
            }

            string sourceConfigurationDirectoryName;
            switch (configuration)
            {
                case ConfigurationName.Debug:
                    sourceConfigurationDirectoryName = @"Debug";
                    break;

                case ConfigurationName.Release:
                    sourceConfigurationDirectoryName = @"Release";
                    break;

                default:
                    Console.WriteLine($@"Unknown configuration: {configuration.ToString()}. Options are: {ConfigurationName.Debug.ToString()}, {ConfigurationName.Release.ToString()}");
                    return Program.Return(-1);
            }

            string architectureArg = args[2];
            string architectureArgLower = architectureArg.ToLowerInvariant();
            PlatformArchitecture architecture;
            switch(architectureArgLower)
            {
                case @"anycpu":
                case @"x86":
                case @"x32":
                    architecture = PlatformArchitecture.x32;
                    break;

                case @"x64":
                    architecture = PlatformArchitecture.x64;
                    break;

                default:
                    Console.WriteLine($@"Unknown architecture: {architectureArg}. Options are: Any CPU, x86, x32, or x64");
                    return Program.Return(-1);
            }

            string sourceArchitectureDirectoryName;
            switch(architecture)
            {
                case PlatformArchitecture.x32:
                    sourceArchitectureDirectoryName = @"x32";
                    break;

                case PlatformArchitecture.x64:
                    sourceArchitectureDirectoryName = @"x64";
                    break;

                default:
                    Console.WriteLine($@"Unknown architecture: {architecture.ToString()}. Options are: {PlatformArchitecture.x32.ToString()}, {PlatformArchitecture.x64.ToString()}");
                    return Program.Return(-1);
            }
            
            string inputFileUnknownPath = args[3];
            string inputFilePath;
            if(Path.IsPathRooted(inputFileUnknownPath))
            {
                inputFilePath = inputFileUnknownPath;
            }
            else
            {
                //string inputFileUnresolvedPath = 
                inputFilePath = Path.Combine(clientTargetDirectoryPath, inputFileUnknownPath); // PathExtensions.GetResolvedPath(inputFileUnresolvedPath);
            }

            // Create the list of replace codes.
            var replaceCodes = new List<Tuple<string, string>>();
            replaceCodes.Add(Tuple.Create(@"$(Architecture)", sourceArchitectureDirectoryName));
            replaceCodes.Add(Tuple.Create(@"$(Configuration)", sourceConfigurationDirectoryName));

            if (!File.Exists(inputFilePath))
            {
                Console.WriteLine($@"Unable to find input file: {inputFilePath}");
                return Program.Return(-1);
            }

            // Parse the input file and determine source and destination paths.
            var paths = File.ReadAllLines(inputFilePath);

            var sourceDestinationPathPairs = new List<Tuple<string, string>>();
            foreach(string path in paths)
            {
                string replacedPath = path;
                foreach(var replaceCode in replaceCodes)
                {
                    replacedPath = replacedPath.Replace(replaceCode.Item1, replaceCode.Item2);
                }

                string sourceFilePath;
                if(Path.IsPathRooted(replacedPath))
                {
                    sourceFilePath = replacedPath;
                }
                else
                {
                    sourceFilePath = Path.Combine(clientTargetDirectoryPath, replacedPath);
                }

                string fileName = Path.GetFileName(sourceFilePath);
                string destinationFilePath = Path.Combine(clientTargetDirectoryPath, fileName);
                sourceDestinationPathPairs.Add(Tuple.Create(sourceFilePath, destinationFilePath));
            }

            // Now copy all file paths, but only if newer.
            foreach(var sourceDestinationPathPair in sourceDestinationPathPairs)
            {
                string sourceFilePath = sourceDestinationPathPair.Item1;
                string destinationFilePath = sourceDestinationPathPair.Item2;

                if(!File.Exists(sourceFilePath))
                {
                    Console.WriteLine($@"File not found: {sourceFilePath}");
                    return Program.Return(-1);
                }

                bool performCopy = false;
                if(File.Exists(destinationFilePath))
                {
                    DateTime sourceModifiedTime = File.GetLastWriteTimeUtc(sourceFilePath);
                    DateTime destinationModifiedTime = File.GetLastWriteTimeUtc(destinationFilePath);

                    if(sourceModifiedTime > destinationModifiedTime)
                    {
                        performCopy = true;
                    }
                }
                else
                {
                    performCopy = true;
                }

                if(performCopy)
                {
                    Console.WriteLine($@"Copying file {sourceFilePath} to {destinationFilePath}");
                    File.Copy(sourceFilePath, destinationFilePath, true);
                }
            }

            return Program.Return(0);
        }

        private static int Return(int returnCode)
        {
            if (0 != returnCode)
            {
#if (DEBUG)
                throw new Exception();
#endif
            }

            return returnCode;
        }
    }
}
