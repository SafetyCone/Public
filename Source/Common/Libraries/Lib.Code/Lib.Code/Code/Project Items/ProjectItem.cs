using System;


namespace Public.Common.Lib.Code
{
    /// <summary>
    /// The base class for all project items.
    /// </summary>
    public abstract class ProjectItem : ICloneable<ProjectItem>
    {
        public const string Compile = @"Compile";
        public const string Content = @"Content";
        public const string Folder = @"Folder";
        public const string None = @"None";
        public const string ProjectReference = @"ProjectReference";
        public const string Reference = @"Reference";


        public string IncludePath { get; set; }


        public ProjectItem()
        {
        }

        public ProjectItem(string includePath)
        {
            this.IncludePath = includePath;
        }

        protected ProjectItem(ProjectItem other)
            : this(other.IncludePath)
        {
        }

        public abstract ProjectItem Clone();

        object ICloneable.Clone()
        {
            return this.Clone();
        }
    }
}
