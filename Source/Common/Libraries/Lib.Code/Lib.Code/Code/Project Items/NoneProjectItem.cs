using System;


namespace Public.Common.Lib.Code
{
    /// <summary>
    /// The relative path of a project item with no, or a special purpose (the App.config file for example).
    /// </summary>
    public class NoneProjectItem : ProjectItem
    {
        public string Generator { get; set; }
        public string LastGenOutput { get; set; }
        public CopyToOutputDirectory CopyToOutputDirectory { get; set; }


        public NoneProjectItem()
        {
            this.CopyToOutputDirectory = CopyToOutputDirectory.Blank;
        }

        public NoneProjectItem(string appConfigFileName)
            : base(appConfigFileName)
        {
            this.CopyToOutputDirectory = CopyToOutputDirectory.Blank;
        }

        public NoneProjectItem(
            string appConfigFileName,
            string generator,
            string lastGenOutput,
            CopyToOutputDirectory copyToOutputDirectory)
            : this(appConfigFileName)
        {
            this.Generator = generator;
            this.LastGenOutput = lastGenOutput;
            this.CopyToOutputDirectory = copyToOutputDirectory;
        }

        public NoneProjectItem(NoneProjectItem other)
            : this(
            other.IncludePath,
            other.Generator,
            other.LastGenOutput,
            other.CopyToOutputDirectory)
        {
        }

        public override ProjectItem Clone()
        {
            return new NoneProjectItem(this);
        }
    }
}
