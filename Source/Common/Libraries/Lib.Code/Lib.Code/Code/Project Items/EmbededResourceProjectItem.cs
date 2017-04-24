using System;


namespace Public.Common.Lib.Code
{
    public class EmbededResourceProjectItem : ProjectItem
    {
        public string Generator { get; set; }
        public string LastGenOutput { get; set; }
        public string SubType { get; set; }


        public EmbededResourceProjectItem()
            : this(null)
        {
        }

        public EmbededResourceProjectItem(string includePath)
            : base(includePath)
        {
        }

        public EmbededResourceProjectItem(string includePath, string generator, string lastGenOutput, string subType)
            : base(includePath)
        {
            this.Generator = generator;
            this.LastGenOutput = lastGenOutput;
            this.SubType = subType;
        }
    }
}
