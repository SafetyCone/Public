using System;


namespace Public.Common.Lib.Code
{
    /// <summary>
    /// The assembly name of an assembly in the Global Assembly Cache referenced by a project.
    /// </summary>
    public class ReferenceProjectItem : ProjectItem
    {
        public bool EmbedInteropTypes { get; set; }


        public ReferenceProjectItem()
        {
        }

        public ReferenceProjectItem(string assemblyName)
            : base(assemblyName)
        {
        }

        public ReferenceProjectItem(string assemblyName, bool embedInteropTypes)
            : base(assemblyName)
        {
            this.EmbedInteropTypes = embedInteropTypes;
        }
    }
}
