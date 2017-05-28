using System;
using System.Collections.Generic;

using Public.Common.Lib.Code;
using Public.Common.Lib.Code.Logical;
using Public.Common.Lib.Code.Physical;

using Public.Common.Avon.Lib;
using AvonUtilities = Public.Common.Avon.Lib.Utilities;


namespace Public.Common.Avon
{
    public static class Construction
    {
        public static void SubMain(string[] args)
        {
            //Program.DistributeChangesFromDefaultVsVersionSolution();
            Construction.DistributeChangesFromSpecificVsVersionSolution();
            //Program.EnsureVsVersionedBinAndObjProperties();
            //Program.SetDefaultVsVersionSolution();
            //Program.CreateSolutionSetFromInitialVsVersionSolution();
            //Program.CreateNewSolutionSet();
        }

        public static void DistributeChangesFromDefaultVsVersionSolution()
        {
            string solutionDirectoryPath = @"C:\Organizations\Minex\Repositories\Public\Source\Common\Libraries\Lib";

            AvonUtilities.DistributeChangesFromDefaultVsVersionSolution(solutionDirectoryPath);
        }

        public static void DistributeChangesFromSpecificVsVersionSolution()
        {
            string solutionDirectoryPath = @"C:\Organizations\Minex\Repositories\Public\Source\Common\Libraries\Lib";
            VisualStudioVersion sourceVsVersion = VisualStudioVersion.VS2010;

            AvonUtilities.DistributeChangesFromSpecifiedVsVersionSolution(solutionDirectoryPath, sourceVsVersion);
        }

        public static void EnsureVsVersionedBinAndObjProperties()
        {
            string solutionDirectoryPath = @"C:\Organizations\Minex\Repositories\Public\Source\Experiments\Scripts\Augustus";
            HashSet<string> projectDirectoryPathsAllowedToChange = new HashSet<string>();
            projectDirectoryPathsAllowedToChange.Add(@"C:\Organizations\Minex\Repositories\Public\Source\Experiments\Scripts\Augustus\Augustus");

            AvonUtilities.EnsureVsVersionedBinAndObjProperties(solutionDirectoryPath, projectDirectoryPathsAllowedToChange);
        }

        public static void SetDefaultVsVersionSolution()
        {
            string solutionDirectoryPath = @"C:\Organizations\Minex\Repositories\Public\Source\Common\Libraries\Lib.Email";
            VisualStudioVersion defaultVersion = VisualStudioVersion.VS2010;

            AvonUtilities.SetDefaultVisualStudioVersion(solutionDirectoryPath, defaultVersion);
        }

        public static void CreateSolutionSetFromInitialVsVersionSolution()
        {
            string initialSolutionFilePath = @"C:\Organizations\Minex\Repositories\Public\Source\Common\Experiments\Nahant\Nahant.VS2015.sln";

            VisualStudioVersion[] desiredVsVersions = new VisualStudioVersion[]
            {
                VisualStudioVersion.VS2010,
                VisualStudioVersion.VS2013,
                VisualStudioVersion.VS2015,
                VisualStudioVersion.VS2017,
            };

            Creation.CreateSolutionSet(initialSolutionFilePath, desiredVsVersions);
        }

        public static void CreateNewSolutionSet()
        {
            NewSolutionSpecification specification = new NewSolutionSpecification(
                @"C:\Organizations",
                @"Minex",
                @"Public",
                @"Common",
                SolutionType.Library,
                @"Lib.Email",
                ProjectType.Library,
                VisualStudioVersion.VS2015,
                Language.CSharp);

            Construction.CreateNewSolutionSet(specification);
        }

        public static void CreateNewSolutionSet(NewSolutionSpecification specification)
        {
            VisualStudioVersion[] vsVersions = VisualStudioVersionExtensions.GetAllVisualStudioVersions();

            NewSolutionSetSpecification setSpecification = new NewSolutionSetSpecification(specification, vsVersions);

            AvonUtilities.CreateSolutionSetWithDefault(setSpecification, specification.VisualStudioVersion);
        }
    }
}
