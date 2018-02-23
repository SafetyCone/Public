using System;

using Public.Common.Lib;


namespace Eshunna.Lib.PLY
{
    public static class PlyFileDataFormatExtensions
    {
        public static PlyFileDataFormat ToPlyFileDataFormat(this string fileFormatToken)
        {
            PlyFileDataFormat output;
            switch (fileFormatToken)
            {
                case PlyFile.asciiFormatMarker:
                    output = PlyFileDataFormat.ASCII;
                    break;

                case PlyFile.bigEndianFormatMarker:
                    output = PlyFileDataFormat.BinaryBigEndian;
                    break;

                case PlyFile.littleEndianFormatMarker:
                    output = PlyFileDataFormat.BinaryLittleEndian;
                    break;

                default:
                    throw new ArgumentException(@"Unknown format.", nameof(fileFormatToken));
            }
            return output;
        }

        public static string ToPlyFileDataFormatToken(this PlyFileDataFormat plyFileDataFormat)
        {
            string output;
            switch (plyFileDataFormat)
            {
                case PlyFileDataFormat.ASCII:
                    output = PlyFile.asciiFormatMarker;
                    break;

                case PlyFileDataFormat.BinaryBigEndian:
                    output = PlyFile.bigEndianFormatMarker;
                    break;

                case PlyFileDataFormat.BinaryLittleEndian:
                    output = PlyFile.littleEndianFormatMarker;
                    break;

                default:
                    throw new UnexpectedEnumerationValueException<PlyFileDataFormat>(plyFileDataFormat);
            }
            return output;
        }
    }
}
