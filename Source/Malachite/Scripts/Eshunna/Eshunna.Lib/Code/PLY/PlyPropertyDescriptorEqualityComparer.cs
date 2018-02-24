using System;
using System.Collections.Generic;

using Public.Common.Lib;

using Eshunna.Lib.Logging;


namespace Eshunna.Lib.PLY
{
    public class PlyPropertyDescriptorEqualityComparer : IEqualityComparer<PlyPropertyDescriptor>
    {
        public ILog Log { get; }


        public PlyPropertyDescriptorEqualityComparer(ILog log)
        {
            this.Log = log;
        }

        public bool Equals(PlyPropertyDescriptor x, PlyPropertyDescriptor y)
        {
            bool output = true;

            bool nameEquals = x.Name == y.Name;
            if(!nameEquals)
            {
                output = false;

                string message = $@"Property name mismatch - x: {x.Name}, y: {y.Name}";
                this.Log.WriteLine(message);
            }

            bool dataTypeEquals = x.DataType == y.DataType;
            if(!dataTypeEquals)
            {
                output = false;

                string message = $@"Property data type mismatch - x: {x.DataType.ToPlyFileDataTypeToken()}, y: {y.DataType.ToPlyFileDataTypeToken()}";
                this.Log.WriteLine(message);
            }

            bool isListEquals = x.IsList == y.IsList;
            if(!isListEquals)
            {
                output = false;

                string message = $@"Property is list mismatch: x:{x.IsList.ToString()}, y:{y.IsList.ToString()}";
                this.Log.WriteLine(message);
            }

            bool listLengthDataTypeEquals = x.ListLengthValueDataType == y.ListLengthValueDataType;
            if(!listLengthDataTypeEquals)
            {
                output = false;

                string message = $@"Property list length value data type mismatch: x: {x.ListLengthValueDataType.ToPlyFileDataTypeToken()}, y: {y.ListLengthValueDataType.ToPlyFileDataTypeToken()}";
                this.Log.WriteLine(message);
            }

            return output;
        }

        public int GetHashCode(PlyPropertyDescriptor obj)
        {
            int output = HashHelper.GetHashCode(obj.Name);
            return output;
        }
    }
}
