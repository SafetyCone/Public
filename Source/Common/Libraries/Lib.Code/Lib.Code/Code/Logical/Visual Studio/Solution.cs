using System;
using System.Collections.Generic;


namespace Public.Common.Lib.Code.Logical
{
    // Ok.
    /// <summary>
    /// Represents a solution in memory, which is nothing more than a dictionary of project keyed by the path to that project.
    /// </summary>
    /// <remarks>
    /// A solution file can be deserialized to a physical solution object, which can then be translated into this logical solution object.
    /// /// The process can be reversed to write out a solution.
    /// </remarks>
    public class Solution
    {
        public SolutionInfo Info { get; set; }
        public Dictionary<string, Project> ProjectsByPath { get; protected set; }


        public Solution()
        {
            this.Info = new SolutionInfo();
            this.ProjectsByPath = new Dictionary<string, Project>();
        }
    }
}
