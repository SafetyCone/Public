using System;

using Public.Common.Lib;


namespace Eshunna.Lib.PLY
{
    public static class PlyDataTypeExtensions
    {
        public static PlyDataType ToPlyDataType(this string plyFileDataTypeToken)
        {
            PlyDataType output;
            switch (plyFileDataTypeToken)
            {
                case PlyFile.charTypeName:
                    output = PlyDataType.Character;
                    break;

                case PlyFile.ucharTypeName:
                    output = PlyDataType.CharacterUnsigned;
                    break;

                case PlyFile.doubleTypeName:
                    output = PlyDataType.Double;
                    break;

                case PlyFile.floatTypeName:
                    output = PlyDataType.Float;
                    break;

                case PlyFile.intTypeName:
                    output = PlyDataType.Integer;
                    break;

                case PlyFile.uintTypeName:
                    output = PlyDataType.IntegerUnsigned;
                    break;

                case PlyFile.shortTypeName:
                    output = PlyDataType.Short;
                    break;

                case PlyFile.uShortTypeName:
                    output = PlyDataType.ShortUnsigned;
                    break;

                default:
                    throw new ArgumentException($@"Unrecognized data type: {plyFileDataTypeToken}.", nameof(plyFileDataTypeToken));
            }
            return output;
        }

        public static string ToPlyFileDataTypeToken(this PlyDataType plyDataType)
        {
            string output;
            switch (plyDataType)
            {
                case PlyDataType.Character:
                    output = PlyFile.charTypeName;
                    break;

                case PlyDataType.CharacterUnsigned:
                    output = PlyFile.ucharTypeName;
                    break;

                case PlyDataType.Double:
                    output = PlyFile.doubleTypeName;
                    break;

                case PlyDataType.Float:
                    output = PlyFile.floatTypeName;
                    break;

                case PlyDataType.Integer:
                    output = PlyFile.intTypeName;
                    break;

                case PlyDataType.IntegerUnsigned:
                    output = PlyFile.uintTypeName;
                    break;

                case PlyDataType.Short:
                    output = PlyFile.shortTypeName;
                    break;

                case PlyDataType.ShortUnsigned:
                    output = PlyFile.shortTypeName;
                    break;

                default:
                    throw new UnexpectedEnumerationValueException<PlyDataType>(plyDataType);
            }
            return output;
        }
    }
}
