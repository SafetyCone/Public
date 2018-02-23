using System;
using System.Collections.Generic;


namespace Eshunna.Lib.PLY
{
    // http://paulbourke.net/dataformats/ply/
    public class PlyFile
    {
        public const string charTypeName = @"char"; // 1 byte.
        public const string ucharTypeName = @"uchar"; // 1 byte.
        public const string shortTypeName = @"short"; // 2 bytes.
        public const string uShortTypeName = @"ushort"; // 2 bytes.
        public const string intTypeName = @"int"; // 4 bytes.
        public const string uintTypeName = @"uint"; // 4 bytes.
        public const string floatTypeName = @"float"; // 4 bytes.
        public const string doubleTypeName = @"double"; // 8 bytes.

        public const string PlyFileMarker = @"ply";
        public const string PlyFileHeaderBegin = PlyFile.PlyFileMarker;
        public const string PlyFileHeaderEnd = @"end_header";

        public const string asciiFormatMarker = @"ascii";
        public const string littleEndianFormatMarker = @"binary_little_endian";
        public const string bigEndianFormatMarker = @"binary_big_endian";

        public const string formatKeyword = @"format";
        public const string commentKeyword = @"comment";
        public const string elementKeyword = @"element";
        public const string propertyKeyword = @"property";
        public const string listKeyword = @"list";

        public const string vertexElementName = @"vertex";
        public const string faceElementName = @"face";


        public PlyFileHeader Header { get; }
        public Dictionary<string, Dictionary<string, object>> Values { get; }


        public PlyFile(PlyFileHeader header)
        {
            this.Values = new Dictionary<string, Dictionary<string, object>>();

            this.Header = header;
        }
    }
}
