using System;
using System.Collections.Generic;


namespace Eshunna.Lib.PLY
{
    public class PlyFileHeader
    {
        public PlyFileDataFormat FileDataFormat { get; set; }
        public Version FileFormatVersion { get; set; }
        public List<string> Comments { get; }
        public List<PlyElementDescriptor> Elements { get; }


        public PlyFileHeader()
        {
            this.Comments = new List<string>();
            this.Elements = new List<PlyElementDescriptor>();
        }
    }
}
