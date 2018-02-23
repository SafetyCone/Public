using System;
using System.Collections.Generic;


namespace Eshunna.Lib.PLY
{
    public class PlyElementDescriptor
    {
        public string Name { get; set; }
        public int Count { get; set; }
        public List<PlyPropertyDescriptor> PropertyDescriptors { get; }


        public PlyElementDescriptor()
        {
            this.PropertyDescriptors = new List<PlyPropertyDescriptor>();
        }
    }
}
