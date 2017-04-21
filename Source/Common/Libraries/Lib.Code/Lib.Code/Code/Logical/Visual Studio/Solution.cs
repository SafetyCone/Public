using System;
using System.Collections.Generic;


namespace Public.Common.Lib.Code.Logical
{
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
