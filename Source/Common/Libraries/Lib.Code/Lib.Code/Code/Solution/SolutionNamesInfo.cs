using System;


namespace Public.Common.Lib.Code
{
    /// <summary>
    /// Holds the names for important properties of a solution, both logical and physical, like solution name, solution folder name, and solution file name.
    /// </summary>
    /// /// <remarks>
    /// This class is dumb data. The functionality for determining these values lives elsewhere.
    /// </remarks>
    public class SolutionNamesInfo
    {
        public string Name { get; set; }
        public string DirectoryName { get; set; }
        public string FileName { get; set; }


        public SolutionNamesInfo()
        {
        }

        public SolutionNamesInfo(string name, string directoryName, string fileName)
        {
            this.Name = name;
            this.DirectoryName = directoryName;
            this.FileName = FileName;
        }

        public SolutionNamesInfo(SolutionNamesInfo other)
            : this(other.Name, other.DirectoryName, other.FileName)
        {
        }
    }
}
