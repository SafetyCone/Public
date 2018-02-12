using System;


namespace Public.Common.MATLAB
{
    public enum MatlabDataType
    {
        UNKNOWN,
        Cell,
        Char,
        Double,
        Logical,
        Struct,
    }


    public static class MatlabDataTypeExtensions
    {
        public static MatlabDataType ToMatlabDataType(string typeName)
        {
            MatlabDataType output;
            switch (typeName)
            {
                case string s when s == Matlab.CellTypeName:
                    output = MatlabDataType.Cell;
                    break;

                case string s when s == Matlab.CharTypeName:
                    output = MatlabDataType.Char;
                    break;

                case string s when s == Matlab.DoubleTypeName:
                    output = MatlabDataType.Double;
                    break;

                case string s when s == Matlab.LogicalTypeName:
                    output = MatlabDataType.Logical;
                    break;

                case string s when s == Matlab.StructTypeName:
                    output = MatlabDataType.Struct;
                    break;

                default:
                    output = MatlabDataType.UNKNOWN;
                    break;
            }

            return output;
        }

        public static string ToString(MatlabDataType dataType)
        {
            string output;
            switch(dataType)
            {
                case MatlabDataType.Cell:
                    output = Matlab.CellTypeName;
                    break;

                case MatlabDataType.Char:
                    output = Matlab.CharTypeName;
                    break;

                case MatlabDataType.Double:
                    output = Matlab.DoubleTypeName;
                    break;

                case MatlabDataType.Logical:
                    output = Matlab.LogicalTypeName;
                    break;

                case MatlabDataType.Struct:
                    output = Matlab.StructTypeName;
                    break;

                case MatlabDataType.UNKNOWN:
                    output = Matlab.UnknownTypeName;
                    break;

                default:
                    output = String.Empty;
                    break;
            }

            return output;
        }
    }
}
