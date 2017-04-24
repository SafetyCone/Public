using System;
using System.Collections.Generic;

using Public.Common.Lib.Code.Physical;


namespace Public.Common.Lib.Code
{
    public class NewSolutionSetSpecification
    {
        public NewSolutionSpecification BaseSolutionSpecification { get; set; }
        public List<VisualStudioVersion> VisualStudioVersions { get; protected set; }


        public NewSolutionSetSpecification()
        {
            this.VisualStudioVersions = new List<VisualStudioVersion>();
        }

        public NewSolutionSetSpecification(NewSolutionSpecification baseSolutionSpecification, IEnumerable<VisualStudioVersion> visualStudioVersions)
        {
            this.BaseSolutionSpecification = baseSolutionSpecification;
            this.VisualStudioVersions = new List<VisualStudioVersion>(visualStudioVersions);
        }
    }
}
