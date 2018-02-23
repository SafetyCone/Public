using System;


namespace Eshunna.Lib.PLY
{
    public class PlyPropertyDescriptor
    {
        public string Name { get; set; }
        public PlyDataType DataType { get; set; }
        public bool IsList { get; set; }
        public PlyDataType ListLengthValueDataType { get; set; }


        public PlyPropertyDescriptor(string name, PlyDataType dataType, bool isList = false, PlyDataType listLengthValueDataType = PlyDataType.None)
        {
            this.Name = name;
            this.DataType = dataType;
            this.IsList = isList;
            this.ListLengthValueDataType = listLengthValueDataType;
        }
    }
}
