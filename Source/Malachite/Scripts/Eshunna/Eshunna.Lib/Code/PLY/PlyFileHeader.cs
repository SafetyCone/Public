using System;
using System.Collections.Generic;


namespace Eshunna.Lib.PLY
{
    public class PlyFileHeader
    {
        #region Static

        public static PlyFileHeader GetDefault()
        {
            var header = new PlyFileHeader
            {
                FileFormatVersion = PlyFile.GetDefaultVersion(),
                FileDataFormat = PlyFileDataFormat.ASCII
            };
            return header;
        }

        #endregion


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
