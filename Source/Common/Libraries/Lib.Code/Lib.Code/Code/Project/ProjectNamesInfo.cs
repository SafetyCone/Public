using System;


namespace Public.Common.Lib.Code
{
    /// <summary>
    /// Holds the names for important properties of a project, both logical and physical, like project name, directory name, project file name, root namespace, and assembly name.
    /// </summary>
    /// /// <remarks>
    /// This class is dumb data. The functionality for determining these values lives elsewhere.
    /// </remarks>
    public class ProjectNamesInfo
    {
        public string Name { get; set; }
        public string DirectoryName { get; set; }
        public string FileName { get; set; }
        public string RootNamespaceName { get; set; }
        public string AssemblyName { get; set; }


        public ProjectNamesInfo()
        {
        }

        public ProjectNamesInfo(string name, string directoryName, string fileName, string rootNamespaceName, string assemblyName)
        {
            this.Name = name;
            this.DirectoryName = directoryName;
            this.FileName = FileName;
            this.RootNamespaceName = rootNamespaceName;
            this.AssemblyName = assemblyName;
        }

        public ProjectNamesInfo(ProjectNamesInfo other)
            : this(other.Name, other.DirectoryName, other.FileName, other.RootNamespaceName, other.AssemblyName)
        {
        }
    }
}
