using System;
using System.Collections.Generic;


namespace Public.Common.Avon.Lib
{
    /// <summary>
    /// A collection of Visual Studio versioned projects, identified by directory.
    /// </summary>
    /// <remarks>
    /// Note, this assumes that two different will never be in the same directory!
    /// </remarks>
    public class ProjectSetCollection
    {
        public Dictionary<string, VisualStudioVersionedFilePathSet> ProjectSetsByDirectoryPath { get; protected set; }


        public ProjectSetCollection()
        {
            this.ProjectSetsByDirectoryPath = new Dictionary<string, VisualStudioVersionedFilePathSet>();
        }
    }
}
