using System;
using System.IO;

using Public.Common.Augustus.Lib;
using ProjectFileNameInfo = Public.Common.Lib.Code.ProjectFileNameInfo;
using Project = Public.Common.Lib.Code.Physical.CSharp.Project;
using ProjectSerializer = Public.Common.Lib.Code.Serialization.CSharpProjectSerializer;
using VsVersionExtensions = Public.Common.Lib.Code.Physical.VisualStudioVersionExtensions;
using Public.Common.Lib.IO;


namespace Public.Common.Maynard
{
    public static class Testing
    {
        public static void SubMain(string[] args)
        {
            Testing.TestOfProjectDetails();
            //Testing.TestBuildOfProject();
        }

        private static void TestOfProjectDetails()
        {
            string projectFilePath = @"C:\Organizations\Minex\Repositories\Public\Source\Common\Scripts\Augustus\Augustus\Augustus.VS2010.csproj";

            Project project = ProjectSerializer.Deserialize(projectFilePath);
            string binDirectoryRelativePath = project.BuildConfigurationInfos[project.ActiveConfiguration].BinOutputPath;
            string projectDirectoryPath = Path.GetDirectoryName(projectFilePath);
            string binDirectoryPath = Path.Combine(projectDirectoryPath, binDirectoryRelativePath);

            ProjectFileNameInfo nameInfo = ProjectFileNameInfo.Parse(projectFilePath);
            string projectDirectoryName = nameInfo.FileNameBase;
            string vsVersionProjectDirectoryName = String.Format(@"{0}.{1}", nameInfo.FileNameBase, VsVersionExtensions.ToDefaultString(nameInfo.VisualStudioVersion));
        }

        private static void TestBuildOfProject()
        {
            string projectFilePath = @"C:\Organizations\Minex\Repositories\Public\Source\Common\Scripts\Augustus\Augustus\Augustus.VS2010.csproj";

            string buildItemSpecification = String.Format(@"{0}|Windows|x86", projectFilePath);

            bool success = Builder.Run(buildItemSpecification, new ConsoleOutputStream(), new ConsoleOutputStream());
        }
    }
}
