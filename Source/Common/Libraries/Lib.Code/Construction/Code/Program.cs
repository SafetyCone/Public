using System;
using System.Collections.Generic;
using System.IO;

using Public.Common.Lib.Code.Logical;
using LogicalSolution = Public.Common.Lib.Code.Logical.Solution;
using LogicalProject = Public.Common.Lib.Code.Logical.Project;
using Public.Common.Lib.Code.Physical;
using Public.Common.Lib.Code.Physical.CSharp;
using PhysicalProject = Public.Common.Lib.Code.Physical.CSharp.Project;
using PhysicalSolution = Public.Common.Lib.Code.Physical.Solution;
using Public.Common.Lib.Code.Serialization;
using Public.Common.Lib.Extensions;


namespace Public.Common.Lib.Code.Construction
{
    class Program
    {
        static void Main(string[] args)
        {
            Program.Test();
        }

        private static void Test()
        {
            Program.TestNewSolutionCreation();
            //Program.TestSolutionTextFileSerialization();
            //Program.TestProjectXmlSerialization();
            //Program.TestProjectXmlDeserialization();
            //Program.TestConsoleProgramCreation();
            //Program.TestWriteProgramFile();
            //Program.TestCreateNewConsoleSolution();
            //Program.TestPathInfoNode();
            //Program.TestOrganizationsDirectoryPathIdentificationCustom();
            //Program.TestOrganizationsDirectoryPathIdentificationDefault();
        }

        private static void TestNewSolutionCreation()
        {
            NewSolutionSpecification specification = new NewSolutionSpecification(
                @"C:\temp\Orgs",
                @"TheOrg",
                @"TheRepo",
                @"Common",
                SolutionType.Experiment,
                @"FirstConsole",
                ProjectType.Console,
                VisualStudioVersion.VS2015,
                Language.CSharp);

            Creation.CreateSolution(specification);
        }

        private static void TestSolutionTextFileSerialization()
        {
            string outputSolutionFilesDirectoryPath = @"C:\temp\Setupper\Solutions";
            if (!Directory.Exists(outputSolutionFilesDirectoryPath))
            {
                Directory.CreateDirectory(outputSolutionFilesDirectoryPath);
            }
            string outputVerifyFilesDirectoryPath = @"C:\temp\Setupper\Verify";
            if (!Directory.Exists(outputVerifyFilesDirectoryPath))
            {
                Directory.CreateDirectory(outputVerifyFilesDirectoryPath);
            }

            List<string> solutionFilePaths = new List<string>(Program.GetAllSolutionFilePaths());

            List<string> outputSolutionFilePaths = new List<string>();
            List<string> verifyFilePaths = new List<string>();
            foreach (string path in solutionFilePaths)
            {
                string fileName = Path.GetFileName(path);

                string outputPath = Path.Combine(outputSolutionFilesDirectoryPath, fileName);
                outputSolutionFilePaths.Add(outputPath);

                string verifyPath = Path.Combine(outputVerifyFilesDirectoryPath, fileName);
                verifyFilePaths.Add(verifyPath);
            }

            List<string> pathsWithDifferences = new List<string>();
            for (int iPath = 0; iPath < verifyFilePaths.Count; iPath++)
            {
                string inputFilePath = solutionFilePaths[iPath];
                string outputFilePath = outputSolutionFilePaths[iPath];
                string testEqualityFilePath = verifyFilePaths[iPath];

                PhysicalSolution solution = SolutionSerializer.Deserialize(inputFilePath);

                SolutionSerializer.Serialize(outputFilePath, solution);

                bool same = Program.VerifyTextFileContentEquality(inputFilePath, outputFilePath, testEqualityFilePath);
                if (!same)
                {
                    pathsWithDifferences.Add(outputFilePath);
                }
            }

            string pathsWithDifferencesFilePath = @"C:\temp\Paths With Differences.txt";
            File.WriteAllLines(pathsWithDifferencesFilePath, pathsWithDifferences.ToArray());
        }

        private static IEnumerable<string> GetAllSolutionFilePaths()
        {
            string[] output = new string[]
            {
                @"C:\Organizations\Minex\Repositories\Minex\Source\Common\Libraries\Lib.Code\Lib.Code\Files\CsConsoleApp.VS2017.sln",
                @"C:\Organizations\Minex\Repositories\Minex\Source\Common\Libraries\Lib.Code\Lib.Code\Files\CsConsoleApplication1.VS2015.sln",
                @"C:\Organizations\Minex\Repositories\Minex\Source\Common\Libraries\Lib.Code\Lib.Code\Files\CsConsoleApplication1.VS2013.sln",
                @"C:\Organizations\Minex\Repositories\Minex\Source\Common\Libraries\Lib.Code\Lib.Code\Files\CsConsoleApplication1.VS2010.sln"
            };
            return output;
        }

        private static void TestProjectXmlSerialization()
        {
            string outputProjectFilesDirectoryPath = @"C:\temp\Setupper\Projects";
            if(!Directory.Exists(outputProjectFilesDirectoryPath))
            {
                Directory.CreateDirectory(outputProjectFilesDirectoryPath);
            }
            string outputVerifyFilesDirectoryPath = @"C:\temp\Setupper\Verify";
            if(!Directory.Exists(outputVerifyFilesDirectoryPath))
            {
                Directory.CreateDirectory(outputVerifyFilesDirectoryPath);
            }

            List<string> projectFilePaths = new List<string>(Program.GetAllProjectPaths());

            List<string> outputProjectFilePaths = new List<string>();
            List<string> verifyFilePaths = new List<string>();
            foreach (string path in projectFilePaths)
            {
                string fileName = Path.GetFileName(path);

                string outputPath = Path.Combine(outputProjectFilesDirectoryPath, fileName);
                outputProjectFilePaths.Add(outputPath);

                string verifyPath = Path.Combine(outputVerifyFilesDirectoryPath, fileName);
                verifyFilePaths.Add(verifyPath);
            }

            List<string> pathsWithDifferences = new List<string>();
            for (int iPath = 0; iPath < verifyFilePaths.Count; iPath++)
            {
                string inputProjectFilePath = projectFilePaths[iPath];
                string outputProjectFilePath = outputProjectFilePaths[iPath];
                string testEqualityFilePath = verifyFilePaths[iPath];

                PhysicalProject project = CSharpProjectSerializer.Deserialize(inputProjectFilePath);

                CSharpProjectSerializer.Serialize(outputProjectFilePath, project);

                bool same = Program.VerifyTextFileContentEquality(inputProjectFilePath, outputProjectFilePath, testEqualityFilePath);
                if(!same)
                {
                    pathsWithDifferences.Add(outputProjectFilePath);
                }
            }

            string pathsWithDifferencesFilePath = @"C:\temp\Paths With Differences.txt";
            File.WriteAllLines(pathsWithDifferencesFilePath, pathsWithDifferences.ToArray());
        }

        private static bool VerifyTextFileContentEquality(string filePath1, string filePath2, string notEqualLinesFilePath)
        {
            bool output = true;

            string[] file1LinesArr = File.ReadAllLines(filePath1);
            List<string> file1Lines = new List<string>();
            foreach (string line in file1LinesArr)
            {
                string trimmedLine = line.Trim(' ');
                if(!String.IsNullOrEmpty(trimmedLine))
                {
                    file1Lines.Add(line); // Add the original line.
                }
            }
            string[] file2LinesArr = File.ReadAllLines(filePath2);
            List<string> file2Lines = new List<string>();
            foreach (string line in file2LinesArr)
            {
                string trimmedLine = line.Trim(' ');
                if (!String.IsNullOrEmpty(trimmedLine))
                {
                    file2Lines.Add(line); // Add the original line.
                }
            }

            using (StreamWriter writer = new StreamWriter(notEqualLinesFilePath))
            {
                writer.WriteLine(@"File1: " + filePath1);
                writer.WriteLine(@"File2: " + filePath2);
                writer.WriteLine();

                if (file1Lines.Count != file2Lines.Count)
                {
                    output = false;
                    writer.WriteLine(@"Differing ling counts - File1: {0}, File2: {1}", file1Lines.Count, file2Lines.Count);
                }
                else
                {
                    for (int iLine = 0; iLine < file1Lines.Count; iLine++)
                    {
                        string file1Line = file1Lines[iLine];
                        string file2Line = file2Lines[iLine];

                        if(file1Line != file2Line)
                        {
                            output = false;
                            writer.WriteLine(String.Format(@"Line {0}", iLine));
                            writer.WriteLine(@"File1: " + file1Line);
                            writer.WriteLine(@"File2: " + file2Line);
                            writer.WriteLine();
                        }
                    }
                }

                if(output)
                {
                    writer.WriteLine(@"True: Files 1 and 2 are the same!");
                }
            }

            return output;
        }

        private static void TestProjectXmlDeserialization()
        {
            IEnumerable<string> projectFilePaths = Program.GetAllProjectPaths();

            Dictionary<string, PhysicalProject> projects = new Dictionary<string, PhysicalProject>();
            foreach (string projectFilePath in projectFilePaths)
            {
                PhysicalProject project = CSharpProjectSerializer.Deserialize(projectFilePath);
                projects.Add(projectFilePath, project);
            }
        }

        private static IEnumerable<string> GetAllProjectPaths()
        {
            List<string> output = new List<string>();
            output.AddRange(Program.GetConsoleProjectPaths());
            output.AddRange(Program.GetLibraryProjectPaths());

            return output;
        }

        private static IEnumerable<string> GetLibraryProjectPaths()
        {
            string[] output = new string[]
            {
                @"C:\Organizations\Minex\Repositories\Minex\Source\Common\Libraries\Lib.Code\Lib.Code\Files\CsClassLibrary1.VS2017.csproj",
                @"C:\Organizations\Minex\Repositories\Minex\Source\Common\Libraries\Lib.Code\Lib.Code\Files\CsClassLibrary1.VS2015.csproj",
                @"C:\Organizations\Minex\Repositories\Minex\Source\Common\Libraries\Lib.Code\Lib.Code\Files\CsClassLibrary1.VS2013.csproj",
                @"C:\Organizations\Minex\Repositories\Minex\Source\Common\Libraries\Lib.Code\Lib.Code\Files\CsClassLibrary1.VS2010.csproj",
                @"C:\Organizations\Minex\Repositories\Public\Source\Common\Libraries\Lib.Code\Lib.Code\Public.Common.Lib.Code.csproj"
            };
            return output;
        }

        private static IEnumerable<string> GetConsoleProjectPaths()
        {
            string[] output = new string[]
            {
                @"C:\Organizations\Minex\Repositories\Minex\Source\Common\Libraries\Lib.Code\Lib.Code\Files\CsConsoleApp.VS2017.csproj",
                @"C:\Organizations\Minex\Repositories\Minex\Source\Common\Libraries\Lib.Code\Lib.Code\Files\CsConsoleApplication1.VS2015.csproj",
                @"C:\Organizations\Minex\Repositories\Minex\Source\Common\Libraries\Lib.Code\Lib.Code\Files\CsConsoleApplication1.VS2013.csproj",
                @"C:\Organizations\Minex\Repositories\Minex\Source\Common\Libraries\Lib.Code\Lib.Code\Files\CsConsoleApplication1.VS2010.csproj",
                @"C:\Organizations\Minex\Repositories\Public\Source\Common\Libraries\Lib.Code\Construction\Public.Common.Lib.Code.Construction.csproj",
            };
            return output;
        }

        private static void TestConsoleProgramCreation()
        {
            string namespaceName = @"Test1";

            CreateConsoleProgramClass createProgram = new CreateConsoleProgramClass(namespaceName);
            createProgram.Run();

            Class programClass = createProgram.Class;

            CreateClassCodeFile createProgramFile = new CreateClassCodeFile(programClass);
            createProgramFile.Run();

            CodeFile programFile = createProgramFile.CodeFile;

            string path = @"C:\temp\Orgs\Minex\Repositories\Public\Source\Common\Scripts\Test\Program.cs";
            SerializeCodeFileToPath serializeProgramFile = new SerializeCodeFileToPath(path, programFile);
            serializeProgramFile.Run();
        }

        private static void TestWriteProgramFile()
        {
            string path = @"C:\Users\David\Documents\Visual Studio 2010\Projects\CsConsoleApplication1\CsConsoleApplication1\Program.cs";

            WriteProgramFileSimple command = new WriteProgramFileSimple(path);
            command.Run();
        }

        private static void TestCreateNewConsoleSolution()
        {
            //string desiredPath = @"C:\temp\Orgs\Minex\Repositories\Public\Source\Common\Scripts\Test";

            NewSolutionSpecification newSolutionSpec = new NewSolutionSpecification(
                @"C:\temp\Orgs",
                MinexOrganization.OrganizationName,
                PublicRepository.RepositoryName,
                CommonDomain.DomainName,
                SolutionType.Script,
                @"Test",
                ProjectType.Console,
                VisualStudioVersion.VS2015,
                Language.CSharp);

            CreateNewSolution createNewSolution = new CreateNewSolution(newSolutionSpec);
            createNewSolution.Run();
        }

        private static void TestPathInfoNode()
        {
            string filePath = @"C:\Organizations\Minex\Repositories\Minex\Source\Common\Libraries\Lib.Code\Lib.Code\Files\CsConsoleApplication1.VS2010.csproj";

            string[] filePathTokens = filePath.SplitPath();

            PathInfoRootNode root = new PathInfoRootNode();
            root.AddPathTokens(filePathTokens);
        }

        private static void TestOrganizationsDirectoryPathIdentificationDefault()
        {
            string path = @"C:\temp\Organizations";
            string expected = path;

            string test = OrganizationsDirectoryInfo.IdentifyOrganizationsDirectoryPath(path);

            if(expected != test)
            {
                throw new Exception();
            }
        }

        private static void TestOrganizationsDirectoryPathIdentificationCustom()
        {
            string organizationsDirectoryName = @"AllOrgs";

            string path = Path.Combine(@"C:\temp", organizationsDirectoryName);
            string expected = path;

            string test = OrganizationsDirectoryInfo.IdentifyOrganizationsDirectoryPath(path, organizationsDirectoryName);

            if (expected != test)
            {
                throw new Exception();
            }
        }
    }
}
