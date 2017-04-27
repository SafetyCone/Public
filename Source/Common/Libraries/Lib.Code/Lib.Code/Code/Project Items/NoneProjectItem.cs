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


        public NoneProjectItem()
        {
        }

        public NoneProjectItem(string appConfigFileName)
            : base(appConfigFileName)
        {
        }

        public NoneProjectItem(string appConfigFileName, string generator, string lastGenOutput)
            : base(appConfigFileName)
        {
            this.Generator = generator;
            this.LastGenOutput = lastGenOutput;
        }

        public NoneProjectItem(NoneProjectItem other)
            : this(other.IncludePath, other.Generator, other.LastGenOutput)
        {
        }

        public override ProjectItem Clone()
        {
            return new NoneProjectItem(this);
        }
    }
}
