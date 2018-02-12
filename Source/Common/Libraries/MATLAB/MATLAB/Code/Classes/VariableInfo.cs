using System.Diagnostics;

using Public.Common.Lib.Extensions;
using Public.Common.MATLAB.Extensions;


namespace Public.Common.MATLAB
{
    [DebuggerDisplay("{DebuggerDisplay, nq}")]
    public struct VariableInfo
    {
        public string Name { get; }
        public MatlabDataType DataType { get; }
        public bool IsScalar { get; }
        public int[] Size { get; }
        private string DebuggerDisplay
        {
            get
            {
                string sizeString;
                if(this.IsScalar)
                {
                    sizeString = @"Scalar";
                }
                else
                {
                    sizeString = this.Size.ToSizeString();
                }

                string output = $@"{this.Name}-{MatlabDataTypeExtensions.ToString(this.DataType)}-{sizeString}";
                return output;
            }
        }


        public VariableInfo(string name, MatlabDataType dataType, bool isScalar, int[] size)
        {
            this.Name = name;
            this.DataType = dataType;
            this.IsScalar = isScalar;
            this.Size = size;
        }

        public VariableInfo(string newName, VariableInfo other)
        {
            this.Name = newName;
            this.DataType = other.DataType;
            this.IsScalar = other.IsScalar;
            this.Size = other.Size;
        }
    }


    public static class VariableInfoExtensions
    {
        public static int NumberOfDimensions(this VariableInfo variableInfo)
        {
            int output;
            if(variableInfo.IsScalar)
            {
                output = 0;
            }
            else
            {
                int nSizeDimensions = variableInfo.Size.Length;
                if(2 == nSizeDimensions)
                {
                    int rows = variableInfo.Size[0];
                    int columns = variableInfo.Size[1];

                    if(1 == rows || 1 == columns)
                    {
                        output = 1;
                    }
                    else
                    {
                        output = 2;
                    }
                }
                else
                {
                    output = nSizeDimensions;
                }
            }

            return output;
        }

        public static int Numel(this VariableInfo variableInfo)
        {
            int output;
            if (variableInfo.IsScalar)
            {
                output = 1;
            }
            else
            {
                output = variableInfo.Size.Product();
            }

            return output;
        }
    }
}
