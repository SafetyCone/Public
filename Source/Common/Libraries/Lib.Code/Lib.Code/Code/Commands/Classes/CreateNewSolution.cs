using System.IO;

using Public.Common.Lib.Code.Physical;


using Public.Common.Lib.Code.Logical;


namespace Public.Common.Lib.Code
{
    public class CreateNewSolution : ParentCommandBase
    {
        #region Static

        public static void EnsureSolutionDirectories(NewSolutionSpecification newSolutionSpecification)
        {
            OrganizationalInfo organizationalInfo = newSolutionSpecification.OrganizationalInfo;
            OrganizationPaths orgPaths = new OrganizationPaths(organizationalInfo.Organization, newSolutionSpecification.OrganizationsDirectoryPath);

            SolutionPaths solutionPaths = new SolutionPaths(
                newSolutionSpecification.SolutionName,
                newSolutionSpecification.SolutionType.ToDefaultPluralString(),
                organizationalInfo.Domain,
                organizationalInfo.Repository,
                orgPaths);

            DirectoryHierarchyPathsProvider pathsProvider = new DirectoryHierarchyPathsProvider(solutionPaths);

            foreach(string directoryPath in pathsProvider.DirectoryPaths)
            {
                if(!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
            }
        }

        #endregion


        public NewSolutionSpecification NewSolutionSpecification { get; set; }


        public CreateNewSolution(NewSolutionSpecification newSolutionSpecification)
        {
            this.ChildCommands.Add(new ActionCommand(() => CreateNewSolution.EnsureSolutionDirectories(newSolutionSpecification)));

            this.NewSolutionSpecification = newSolutionSpecification;
        }
    }
}
